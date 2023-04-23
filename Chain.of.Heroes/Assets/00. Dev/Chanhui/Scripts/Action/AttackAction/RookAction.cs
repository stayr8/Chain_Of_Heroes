using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookAction : BaseAction
{
    public event EventHandler OnRookStartMoving;
    public event EventHandler OnRookStopMoving;
    public event EventHandler OnRookSwordSlash;


    private List<Vector3> positionList;
    private int currentPositionIndex;

    private enum State
    {
        SwingingRookBeforeMoving,
        SwingingRookMoving,
        SwingingRookAfterMoving,
        SwingingRookBeforeCamera,
        SwingingRookAttackMoving,
        SwingingRookBeforeHit,
        SwingingRookAfterCamera,
        SwingingRookAfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxRookDistance = 3;

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

        

        if (state == State.SwingingRookBeforeMoving)
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
                    OnRookStopMoving?.Invoke(this, EventArgs.Empty);
                    currentPositionIndex++;
                    state = State.SwingingRookMoving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }
        

        switch (state)
        {
            case State.SwingingRookBeforeMoving:
                
                break;
            case State.SwingingRookMoving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingRookBeforeCamera:

                break;
            case State.SwingingRookAfterMoving:

                break;
            case State.SwingingRookAttackMoving:
                Vector3 targetDirection2 = targetUnit.transform.position;
                Vector3 aimDir2 = (targetDirection2 - transform.position).normalized;
                float rotateSpeed2 = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir2, Time.deltaTime * rotateSpeed2);

                float stoppingDistance1 = 1.5f;
                if (Vector3.Distance(transform.position, targetDirection2) > stoppingDistance1)
                {
                    float moveSpeed = 6f;
                    transform.position += aimDir2 * moveSpeed * Time.deltaTime;
                }
                else
                {
                    OnRookStopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingRookBeforeHit;
                }

                break;
            case State.SwingingRookBeforeHit:

                break;
            case State.SwingingRookAfterCamera:

                break;
            case State.SwingingRookAfterHit:

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
            case State.SwingingRookBeforeMoving:

                break;
            case State.SwingingRookMoving:
                AttackCameraStart();
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                state = State.SwingingRookBeforeCamera;

                break;
            case State.SwingingRookBeforeCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_0 = 0.5f;
                stateTimer = afterHitStateTime_0;
                state = State.SwingingRookAfterMoving;

                break;
            case State.SwingingRookAfterMoving:
                AttackActionSystem.Instance.OnAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);
                ActionCameraStart();
                AttackCameraComplete();
                OnRookStartMoving?.Invoke(this, EventArgs.Empty);

                float afterHitStateTime_1 = 1.0f;
                stateTimer = afterHitStateTime_1;
                state = State.SwingingRookAttackMoving;

                break;
            case State.SwingingRookAttackMoving:

                break;
            case State.SwingingRookBeforeHit:
                float afterHitStateTime_2 = 1.5f;
                stateTimer = afterHitStateTime_2;
                OnRookSwordSlash?.Invoke(this, EventArgs.Empty);
                state = State.SwingingRookAfterCamera;

                break;
            case State.SwingingRookAfterCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingRookAfterHit;

                break;
            case State.SwingingRookAfterHit:
                ActionCameraComplete();
                AttackActionSystem.Instance.OffAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);

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

        for (int x = -maxRookDistance; x <= maxRookDistance; x++)
        {
            for (int z = -maxRookDistance; z <= maxRookDistance; z++)
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
                if ((testX != 0) && (testZ != 0))
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

        state = State.SwingingRookBeforeMoving;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for(int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnRookStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public int GetMaxRookDistance()
    {
        return maxRookDistance;
    }

    public override string GetActionName()
    {
        return "·Ï";
    }
    public override string GetSingleActionPoint()
    {
        return "1";
    }
}
