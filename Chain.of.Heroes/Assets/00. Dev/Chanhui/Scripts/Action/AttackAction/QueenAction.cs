using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenAction : BaseAction
{
    public event EventHandler OnQueenStartMoving;
    public event EventHandler OnQueenStopMoving;
    public event EventHandler OnQueenSwordSlash;
    //public event EventHandler OnQueenDash;


    private List<Vector3> positionList;
    private int currentPositionIndex;

    private enum State
    {
        SwingingQueenBeforeMoving,
        SwingingQueenMoving,
        SwingingQueenBeforeCamera,
        SwingingQueenAttackStand,
        SwingingQueenAfterMoving,
        SwingingQueenAttackMoving,
        SwingingQueenBeforeHit,
        SwingingQueenAfterCamera,
        SwingingQueenAfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxQueenDistance = 2;

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


        if (state == State.SwingingQueenBeforeMoving)
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
                    OnQueenStopMoving?.Invoke(this, EventArgs.Empty);
                    currentPositionIndex++;
                    state = State.SwingingQueenMoving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }


        switch (state)
        {
            case State.SwingingQueenBeforeMoving:
  
                break;
            case State.SwingingQueenMoving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingQueenBeforeCamera:

                break;
            case State.SwingingQueenAttackStand:

                break;
            case State.SwingingQueenAfterMoving:

                break;
            case State.SwingingQueenAttackMoving:
                Vector3 targetDirection2 = targetUnit.transform.position;
                Vector3 aimDir2 = (targetDirection2 - transform.position).normalized;
                float rotateSpeed2 = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir2, Time.deltaTime * rotateSpeed2);

                float stoppingDistance1 = 1.5f;
                if (Vector3.Distance(transform.position, targetDirection2) > stoppingDistance1)
                {
                    float moveSpeed = 15f;
                    transform.position += aimDir2 * moveSpeed * Time.deltaTime;
                }
                else
                {
                    OnQueenStopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingQueenBeforeHit;
                }

                break;
            case State.SwingingQueenBeforeHit:

                break;
            case State.SwingingQueenAfterCamera:

                break;
            case State.SwingingQueenAfterHit:
                
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
            case State.SwingingQueenBeforeMoving:

                break;
            case State.SwingingQueenMoving:
                AttackCameraStart();
                float afterHitStateTime = 0.5f;
                stateTimer = afterHitStateTime;
                state = State.SwingingQueenBeforeCamera;

                break;
            case State.SwingingQueenBeforeCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_0 = 0.5f;
                stateTimer = afterHitStateTime_0;
                state = State.SwingingQueenAttackStand;

                break;
            case State.SwingingQueenAttackStand:
                AttackActionSystem.Instance.OnAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);
                ActionCameraStart();
                AttackCameraComplete();
                stateTimer = 0.8f;
                state = State.SwingingQueenAfterMoving;

                break;
            case State.SwingingQueenAfterMoving:
                //OnQueenDash?.Invoke(this, EventArgs.Empty);
                OnQueenStartMoving?.Invoke(this, EventArgs.Empty);

                float afterHitStateTime_1 = 1.0f;
                stateTimer = afterHitStateTime_1;
                state = State.SwingingQueenAttackMoving;

                break;
            case State.SwingingQueenAttackMoving:

                break;
            case State.SwingingQueenBeforeHit:
                float afterHitStateTime_2 = 1.5f;
                stateTimer = afterHitStateTime_2;
                OnQueenSwordSlash?.Invoke(this, EventArgs.Empty);
                state = State.SwingingQueenAfterCamera;

                break;
            case State.SwingingQueenAfterCamera:
                StageUI.Instance.Fade();
                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingQueenAfterHit;

                break;
            case State.SwingingQueenAfterHit:
                ActionCameraComplete();
                AttackActionSystem.Instance.OffAtLocationMove(UnitActionSystem.Instance.GetSelecterdUnit(), targetUnit);
                ActionComplete();
                AttackActionSystem.Instance.SetIsAtk(false);
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

        for (int x = -maxQueenDistance; x <= maxQueenDistance; x++)
        {
            for (int z = -maxQueenDistance; z <= maxQueenDistance; z++)
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
                if ((testX != 0) && (testZ != 0) && (testX != testZ))
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

        state = State.SwingingQueenBeforeMoving;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnQueenStartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetIsAtk(true);

        ActionStart(onActionComplete);
    }

    public int GetMaxQueenDistance()
    {
        return maxQueenDistance;
    }

    public override string GetActionName()
    {
        return "Äý";
    }

    public override string GetSingleActionPoint()
    {
        return "1";
    }
}
