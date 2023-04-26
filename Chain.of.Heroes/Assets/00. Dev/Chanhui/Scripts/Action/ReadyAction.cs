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
        SwingingArcherAiming,
        SwingingArcherShooting,
        SwingingArcherCooloff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    private int maxReadyDistance = 2;

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
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                state = State.SwingingArcherBeforeCamera;

                break;
            case State.SwingingArcherBeforeCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_0 = 0.5f;
                stateTimer = afterHitStateTime_0;
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
                AttackCameraComplete();
                float afterHitStateTime_1 = 1.0f;
                stateTimer = afterHitStateTime_1;
                state = State.SwingingArcherAiming;

                break;
            case State.SwingingArcherAiming:
               
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                    AttackActionSystem.Instance.SetIsAtk(false);
                }

                float afterHitStateTime_2 = 1.0f;
                stateTimer = afterHitStateTime_2;
                state = State.SwingingArcherShooting;
                /*
                if (AttackActionSystem.Instance.GetChainStart())
                {
                    if (!AttackActionSystem.Instance.GetIsChainAtk())
                    {
                        float afterHitStateTime_2 = 1.0f;
                        stateTimer = afterHitStateTime_2;
                        state = State.SwingingArcherShooting;
                    }
                    else
                    {
                        float afterHitStateTime_2 = 0.1f;
                        stateTimer = afterHitStateTime_2;
                    }
                }
                else
                {
                    float afterHitStateTime_2 = 1.0f;
                    stateTimer = afterHitStateTime_2;
                    state = State.SwingingArcherShooting;
                }*/

                break;
            case State.SwingingArcherShooting:
                StageUI.Instance.Fade();
                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingArcherCooloff;

                break;
            case State.SwingingArcherCooloff:
                ActionCameraComplete();
                if (unit.IsEnemy())
                {
                    AttackActionSystem.Instance.OffAtLocationMove(targetUnit, unit);
                }
                else
                {
                    AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);
                }

                ActionComplete();
                break;
        }

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

                

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxReadyDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
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

        state = State.SwingingArcherAttackCameraStart;
        float aimingStateTime = 0.7f;
        stateTimer = aimingStateTime;

        canShootBullet = true;
        if(!unit.IsEnemy())
        {
            AttackActionSystem.Instance.SetIsAtk(true);
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
        if (unit.IsEnemy())
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }
    public override string GetActionName()
    {
        return "Ready";
    }

    public override string GetSingleActionPoint()
    {
        return "1";
    }
}
