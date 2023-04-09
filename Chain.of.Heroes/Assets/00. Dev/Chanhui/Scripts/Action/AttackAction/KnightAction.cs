using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAction : BaseAction
{
    public event EventHandler OnKnightStartMoving;
    public event EventHandler OnKnightStopMoving;
    public event EventHandler OnKnightActionStarted;
    public event EventHandler OnKnightActionCompleted;

    private List<Vector3> positionList;
    private int currentPositionIndex;


    private enum State
    {
        SwingingKnightBeforeMoving,
        SwingingKnightMoving,
        SwingingKnightAfterMoving,
        SwingingKnightBeforeCamera,
        SwingingKnightBeforeHit,
        SwingingKnightAfterCamera,
        SwingingKnightAfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxKnightDistance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;


        if (state == State.SwingingKnightBeforeMoving)
        {
            float rotateSpeed_1 = 30f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed_1);

            float stoppingDistance = 0.1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                float moveSpeed = 4f;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;

            }
            else
            {
                float BeforepositionList = positionList.Count - 2;
                if (currentPositionIndex >= BeforepositionList)
                {
                    OnKnightStopMoving?.Invoke(this, EventArgs.Empty);
                    currentPositionIndex++;
                    state = State.SwingingKnightMoving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }


        switch (state)
        {
            case State.SwingingKnightBeforeMoving:

                break;
            case State.SwingingKnightMoving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingKnightBeforeCamera:

                break;
            case State.SwingingKnightAfterMoving:

                break;
            case State.SwingingKnightBeforeHit:

                break;
            case State.SwingingKnightAfterCamera:

                break;
            case State.SwingingKnightAfterHit:

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
            case State.SwingingKnightBeforeMoving:

                break;
            case State.SwingingKnightMoving:
                AttackCameraStart();
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                state = State.SwingingKnightBeforeCamera;

                break;
            case State.SwingingKnightBeforeCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_0 = 0.5f;
                stateTimer = afterHitStateTime_0;
                state = State.SwingingKnightAfterMoving;

                break;
            case State.SwingingKnightAfterMoving:
                AttackActionSystem.Instance.OnAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);
                ActionCameraStart();
                AttackCameraComplete();
                float afterHitStateTime_1 = 2.0f;
                stateTimer = afterHitStateTime_1;
                state = State.SwingingKnightBeforeHit;

                break;
            case State.SwingingKnightBeforeHit:
                float afterHitStateTime_2 = 1.5f;
                stateTimer = afterHitStateTime_2;
                OnKnightActionStarted?.Invoke(this, EventArgs.Empty);
                state = State.SwingingKnightAfterCamera;
                targetUnit.Damage(100);

                break;
            case State.SwingingKnightAfterCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingKnightAfterHit;

                break;
            case State.SwingingKnightAfterHit:
                ActionCameraComplete();
                AttackActionSystem.Instance.OffAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);
                OnKnightActionCompleted?.Invoke(this, EventArgs.Empty);
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

        for (int x = -maxKnightDistance; x <= maxKnightDistance; x++)
        {
            for (int z = -maxKnightDistance; z <= maxKnightDistance; z++)
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

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if (testX == 0 || testZ == 0 || testX == testZ)
                {
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

                if (!Pathfinding.Instance.HasAtPath(unitGridPosition, testGridPosition))
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

        state = State.SwingingKnightBeforeMoving;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnKnightStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxKnightDistance()
    {
        return maxKnightDistance;
    }

    public override string GetActionName()
    {
        return "³ªÀÌÆ®";
    }

    public override string GetSingleActionPoint()
    {
        return "1";
    }
}
