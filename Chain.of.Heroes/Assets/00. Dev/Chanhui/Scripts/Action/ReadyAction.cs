using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyAction : BaseAction
{

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    private enum State
    {
        SwingingArcherAttackCameraStart,
        SwingingArcherBeforeCamera,
        SwingingArcherAttackCameraEnd,
        SwingingArcherAttackStand,
        SwingingArcherAiming,
        SwingingArcherShooting,
        SwingingArcherCooloff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxReadyDistance = 1;

    private State state;
    private float stateTimer;
    private  Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch(state)
        {
            case State.SwingingArcherAttackCameraStart:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingArcherBeforeCamera:

                break;
            case State.SwingingArcherAttackCameraEnd:

                break;
            case State.SwingingArcherAttackStand:

                break;
            case State.SwingingArcherAiming:
                

                break;
            case State.SwingingArcherShooting:
                
                break;
            case State.SwingingArcherCooloff:
                
                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingArcherAttackCameraStart:
                AttackCameraStart();
                UnitActionSystem.Instance.SetCameraPointchange(true);
                TimeAttack(0.5f);
                state = State.SwingingArcherBeforeCamera;

                break;
            case State.SwingingArcherBeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingArcherAttackCameraEnd;

                break;
            case State.SwingingArcherAttackCameraEnd:
                if (unit.IsEnemy())
                {
                    AttackActionSystem.Instance.OnAtLocationMove(targetUnit, unit);
                }
                else
                {
                    AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                }
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingArcherAttackStand;

                break;
            case State.SwingingArcherAttackStand:
                AttackCameraComplete();

                TimeAttack(0.3f);
                state = State.SwingingArcherAiming;

                break;
            case State.SwingingArcherAiming:
               
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }

                TimeAttack(1.0f);
                state = State.SwingingArcherShooting;

                break;
            case State.SwingingArcherShooting:
                if (unit.IsEnemy())
                {
                    ScreenManager._instance._LoadScreenTextuer();
                    TimeAttack(0.1f);
                    state = State.SwingingArcherCooloff;
                }
                else
                {
                    if (!AttackActionSystem.Instance.GetChainStart())
                    {
                        ScreenManager._instance._LoadScreenTextuer();

                        TimeAttack(0.1f);
                        state = State.SwingingArcherCooloff;
                    }
                    else
                    {
                        AttackActionSystem.Instance.SetIsChainAtk_1(true);
                        AttackActionSystem.Instance.Camera();
                        AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);

                        TimeAttack(0.4f);
                        state = State.SwingingArcherCooloff;
                    }
                    AttackActionSystem.Instance.SetIsAtk(false);
                }

                break;
            case State.SwingingArcherCooloff:

                if (unit.IsEnemy())
                {
                    AttackActionSystem.Instance.OffAtLocationMove(targetUnit, unit);
                    ActionCameraComplete();
                }
                else
                {
                    if (!AttackActionSystem.Instance.GetChainStart())
                    {
                        ActionCameraComplete();
                    }
                    AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);
                }

                ActionComplete();
                break;
        }

    }
    void TimeAttack(float StateTime)
    {
        float afterHitStateTime = StateTime;
        stateTimer = afterHitStateTime;
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });

    }


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxReadyDistance; x <= maxReadyDistance; x++)
        {
            for (int z = -maxReadyDistance; z <= maxReadyDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                /*
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxReadyDistance)
                {
                    continue;
                }*/

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                if (isProvoke)
                {
                    if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                    {
                        Unit Prunit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                        if (Prunit.GetUnitName() == "�ö�ƾ")
                        {

                        }
                        else
                        {

                            continue;
                        }
                    }
                }

                if (!unit.IsEnemy())
                {
                    if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).IsEnemy())
                    {
                        LevelGrid.Instance.GetChainStateGridPosition(testGridPosition);
                    }
                    else
                    {
                        continue;
                    }
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same 'team'
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if( Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                {
                    // Blocked by an Obstacle
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        TimeAttack(0.7f);
        state = State.SwingingArcherAttackCameraStart;

        canShootBullet = true;

        if (!unit.IsEnemy())
        {
            AttackActionSystem.Instance.SetIsAtk(true);
            AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        }
        else
        {
            AttackActionSystem.Instance.SetUnitChainFind(unit, targetUnit);
        }


        ActionStart(onActionComplete);
    }

    public int GetMaxShootDistance()
    {
        return maxReadyDistance;
    }

    

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f),
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }
    public override string GetActionName()
    {
        return "����";
    }

    public override string GetSingleActionPoint()
    {
        return "2";
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
