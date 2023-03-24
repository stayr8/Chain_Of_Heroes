using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopAction : BaseAction
{
    public event EventHandler OnBishopStartMoving;
    public event EventHandler OnBishopStopMoving;
    public event EventHandler OnBishopActionStarted;
    public event EventHandler OnBishopActionCompleted;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private enum State
    {
        SwingingBishopBeforeMoving,
        SwingingBishopAfterMoving,
        SwingingBishopBeforeHit,
        SwingingBishopAfterHit,
    }

    [SerializeField] private int maxBishopDistance = 3;
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

        float rotateSpeed_1 = 30f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed_1);

        if (state == State.SwingingBishopBeforeMoving)
        {
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
                    OnBishopStopMoving?.Invoke(this, EventArgs.Empty);
                    currentPositionIndex++;
                    state = State.SwingingBishopAfterMoving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }


        switch (state)
        {
            case State.SwingingBishopBeforeMoving:

                break;
            case State.SwingingBishopAfterMoving:

            case State.SwingingBishopBeforeHit:
                Vector3 targetDirection = positionList[currentPositionIndex];
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingBishopAfterHit:

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
            case State.SwingingBishopBeforeMoving:

                break;
            case State.SwingingBishopAfterMoving:
                float afterHitStateTime_1 = 0.7f;
                stateTimer = afterHitStateTime_1;
                OnBishopActionStarted?.Invoke(this, EventArgs.Empty);
                state = State.SwingingBishopBeforeHit;

                break;
            case State.SwingingBishopBeforeHit:
                state = State.SwingingBishopAfterHit;
                float afterHitStateTime_2 = 0.2f;
                stateTimer = afterHitStateTime_2;
                targetUnit.Damage(100);
                break;
            case State.SwingingBishopAfterHit:
                OnBishopActionCompleted?.Invoke(this, EventArgs.Empty);
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

        for (int x = -maxBishopDistance; x <= maxBishopDistance; x++)
        {
            for (int z = -maxBishopDistance; z <= maxBishopDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if (testX != testZ)
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

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingBishopBeforeMoving;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnBishopStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxBishopDistance()
    {
        return maxBishopDistance;
    }

    public override string GetActionName()
    {
        return "Bishop";
    }
}
