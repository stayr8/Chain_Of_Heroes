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
        SwingingRookBeforeCamera,
        SwingingRookAttackStand,
        SwingingRookAfterMoving,
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
                    UnitActionSystem.Instance.SetCameraPointchange(true);
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
            case State.SwingingRookAttackStand:

                break;
            case State.SwingingRookAfterMoving:

                break;
            case State.SwingingRookAttackMoving:
                Vector3 targetDirection2 = targetUnit.transform.position;
                Vector3 aimDir2 = (targetDirection2 - transform.position).normalized;
                float rotateSpeed2 = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir2, Time.deltaTime * rotateSpeed2);

                float stoppingDistance1 = 0.1f;
                if (Vector3.Distance(transform.position, targetDirection2) > stoppingDistance1)
                {
                    float moveSpeed = 15f;
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

                TimeAttack(0.5f);
                state = State.SwingingRookBeforeCamera;

                break;
            case State.SwingingRookBeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingRookAttackStand;

                break;
            case State.SwingingRookAttackStand:
                AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingRookAfterMoving;

                break;
            case State.SwingingRookAfterMoving:
                AttackCameraComplete();
                OnRookStartMoving?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingRookAttackMoving;

                break;
            case State.SwingingRookAttackMoving:

                break;
            case State.SwingingRookBeforeHit:
                OnRookSwordSlash?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingRookAfterCamera;

                break;
            case State.SwingingRookAfterCamera:
                if (!AttackActionSystem.Instance.GetChainStart())
                {
                    ScreenManager._instance._LoadScreenTextuer();
                    TimeAttack(0.1f);
                    state = State.SwingingRookAfterHit;
                }
                else
                {
                    AttackActionSystem.Instance.SetIsChainAtk_1(true);
                    TimeAttack(0.5f);
                    state = State.SwingingRookAfterHit;
                }
                AttackActionSystem.Instance.SetIsAtk(false);

                break;
            case State.SwingingRookAfterHit:
                if (!AttackActionSystem.Instance.GetChainStart())
                {
                    ActionCameraComplete();
                }
                AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);

                ActionComplete();

                break;
        }
    }

    void TimeAttack(float StateTime)
    {
        float afterHitStateTime = StateTime;
        stateTimer = afterHitStateTime;
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
                /*
                if (!Pathfinding.Instance.HasAtPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }*/

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
        state = State.SwingingRookBeforeMoving;

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for(int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnRookStartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetIsAtk(true);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

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
