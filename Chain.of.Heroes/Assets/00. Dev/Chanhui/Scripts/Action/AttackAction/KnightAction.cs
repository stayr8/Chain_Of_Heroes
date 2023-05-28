using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAction : BaseAction
{
    public event EventHandler OnKnightStartMoving;
    public event EventHandler OnKnightStopMoving;
    public event EventHandler OnKnightSwordSlash;


    private List<Vector3> positionList;
    private int currentPositionIndex;


    private enum State
    {
        SwingingKnightBeforeMoving,
        SwingingKnightMoving,
        SwingingKnightBeforeCamera,
        SwingingKnightAttackStand,
        SwingingKnightAfterMoving,
        SwingingKnightAttackMoving,
        SwingingKnightBeforeHit,
        SwingingKnightAfterCamera,
        SwingingKnightAfterHit,
    }

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
                    UnitActionSystem.Instance.SetCameraPointchange(true);
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
            case State.SwingingKnightAttackStand:

                break;
            case State.SwingingKnightAfterMoving:

                break;
            case State.SwingingKnightAttackMoving:
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
                    OnKnightStopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingKnightBeforeHit;
                }

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

                TimeAttack(0.5f);
                state = State.SwingingKnightBeforeCamera;

                break;
            case State.SwingingKnightBeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingKnightAttackStand;

                break;
            case State.SwingingKnightAttackStand:
                AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingKnightAfterMoving;

                break;
            case State.SwingingKnightAfterMoving:
                AttackCameraComplete();
                OnKnightStartMoving?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingKnightAttackMoving;

                break;
            case State.SwingingKnightAttackMoving:

                break;
            case State.SwingingKnightBeforeHit:
                OnKnightSwordSlash?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingKnightAfterCamera;


                break;
            case State.SwingingKnightAfterCamera:
                if (!AttackActionSystem.Instance.GetChainStart())
                {
                    ScreenManager._instance._LoadScreenTextuer();

                    TimeAttack(0.1f);
                    state = State.SwingingKnightAfterHit;
                }
                else
                {
                    AttackActionSystem.Instance.SetIsChainAtk_1(true);

                    TimeAttack(0.5f);
                    state = State.SwingingKnightAfterHit;
                }
                AttackActionSystem.Instance.SetIsAtk(false);

                break;
            case State.SwingingKnightAfterHit:
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

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).IsEnemy())
                {
                    LevelGrid.Instance.GetChainStateGridPosition(testGridPosition);
                }
                else
                {
                    continue;
                }

                if (!Pathfinding.Instance.HasAtPath(unitGridPosition, testGridPosition))
                {
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

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        TimeAttack(0.7f);
        state = State.SwingingKnightBeforeMoving;

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnKnightStartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetIsAtk(true);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxKnightDistance()
    {
        return maxKnightDistance;
    }

    public override string GetActionName()
    {
        return "АјАн";
    }

    public override string GetSingleActionPoint()
    {
        return "2";
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
