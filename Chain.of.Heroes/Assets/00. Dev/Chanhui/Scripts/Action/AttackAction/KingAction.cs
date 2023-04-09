using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAction : BaseAction
{

    public event EventHandler OnKingActionStarted;
    public event EventHandler OnKingActionCompleted;

    private enum State
    {
        SwingingKingAttackCameraStart,
        SwingingKingBeforeCamera,
        SwingingKingAttackCameraEnd,
        SwingingKingBeforeHit,
        SwingingKingAfterCamera,
        SwingingKingAfterHit,
    }

    private int maxKingDistance = 1;

    private State state;
    private float stateTimer;
    private  Unit targetUnit;
   
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };

    }

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingKingAttackCameraStart:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingKingBeforeCamera:

                break;
            case State.SwingingKingAttackCameraEnd:

                break;
            case State.SwingingKingBeforeHit:
                
                break;
            case State.SwingingKingAfterCamera:

                break;
            case State.SwingingKingAfterHit:
                
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
            case State.SwingingKingAttackCameraStart:
                AttackCameraStart();
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                state = State.SwingingKingBeforeCamera;

                break;
            case State.SwingingKingBeforeCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_0 = 0.5f;
                stateTimer = afterHitStateTime_0;
                state = State.SwingingKingAttackCameraEnd;

                break;
            case State.SwingingKingAttackCameraEnd:
                if (unit.IsEnemy())
                {
                    AttackActionSystem.Instance.OnAtLocationMove(targetUnit, unit);
                }
                else
                {
                    AttackActionSystem.Instance.OnAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);
                }
                ActionCameraStart();
                AttackCameraComplete();
                float afterHitStateTime_1 = 2.0f;
                stateTimer = afterHitStateTime_1;
                state = State.SwingingKingBeforeHit;

                break;
            case State.SwingingKingBeforeHit:
                float afterHitStateTime_2 = 1.5f;
                stateTimer = afterHitStateTime_2;
                OnKingActionStarted?.Invoke(this, EventArgs.Empty);
                state = State.SwingingKingAfterCamera;
                targetUnit.Damage(100);

                break;
            case State.SwingingKingAfterCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingKingAfterHit;

                break;
            case State.SwingingKingAfterHit:
                ActionCameraComplete();
                if (unit.IsEnemy())
                {
                    AttackActionSystem.Instance.OffAtLocationMove(targetUnit, unit);
                }
                else
                {
                    AttackActionSystem.Instance.OffAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);
                }
                OnKingActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();

                break;
        }

    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxKingDistance; x <= maxKingDistance; x++)
        {
            for (int z = -maxKingDistance; z <= maxKingDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the character is already at
                    continue;
                }


                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
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


                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingKingAttackCameraStart;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "킹";
    }

    public int GetMaxKingDistance()
    {
        return maxKingDistance;
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

    public override string GetSingleActionPoint()
    {
        return "1";
    }
}
