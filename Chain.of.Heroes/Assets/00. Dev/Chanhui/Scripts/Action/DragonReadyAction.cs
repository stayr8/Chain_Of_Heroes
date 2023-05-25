using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonReadyAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }


    private enum State
    {
        SwingingDragon_LookAt,
        SwingingDragon_Attacking,
        SwingingDragon_AfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxReadyDistance = 1;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingDragon_LookAt:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 40f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingDragon_Attacking:


                break;
            case State.SwingingDragon_AfterHit:

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
            case State.SwingingDragon_LookAt:
                TimeAttack(0.5f);
                state = State.SwingingDragon_Attacking;

                break;
            
            case State.SwingingDragon_Attacking:

                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }

                TimeAttack(2.0f);
                state = State.SwingingDragon_AfterHit;

                break;
            
            case State.SwingingDragon_AfterHit:

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
                        if (Prunit.GetUnitName() == "플라틴")
                        {
                            Debug.Log("가디언");
                        }
                        else
                        {
                            Debug.Log(Prunit);
                            continue;
                        }
                    }
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
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
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir,
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
        state = State.SwingingDragon_LookAt;

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
        return 1;
    }
    public override string GetActionName()
    {
        return "드래곤";
    }

    public override string GetSingleActionPoint()
    {
        return "1";
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
