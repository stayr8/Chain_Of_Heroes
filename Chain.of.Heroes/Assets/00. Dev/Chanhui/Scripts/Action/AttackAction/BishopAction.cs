using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopAction : BaseAction
{
    public event EventHandler OnBishopStartMoving;
    public event EventHandler OnBishopStopMoving;
    public event EventHandler OnBishopSwordSlash;


    private List<Vector3> positionList;
    private int currentPositionIndex;

    private enum State
    {
        SwingingBishopBeforeMoving,
        SwingingBishopMoving,
        SwingingBishopBeforeCamera,
        SwingingBishopAttackStand,
        SwingingBishopAfterMoving,
        SwingingBishopAttackMoving,
        SwingingBishopBeforeHit,
        SwingingBishopAfterCamera,
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


        if (state == State.SwingingBishopBeforeMoving)
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
                    OnBishopStopMoving?.Invoke(this, EventArgs.Empty);
                    UnitActionSystem.Instance.SetCameraPointchange(true);
                    currentPositionIndex++;
                    state = State.SwingingBishopMoving;
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
            case State.SwingingBishopMoving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingBishopBeforeCamera:

                break;
            case State.SwingingBishopAttackStand:

                break;
            case State.SwingingBishopAfterMoving:

                break;
            case State.SwingingBishopAttackMoving:
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
                    OnBishopStopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingBishopBeforeHit;
                }

                break;
            case State.SwingingBishopBeforeHit:

                break;
            case State.SwingingBishopAfterCamera:

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
            case State.SwingingBishopMoving:
                AttackCameraStart();

                TimeAttack(0.5f);
                state = State.SwingingBishopBeforeCamera;

                break;
            case State.SwingingBishopBeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingBishopAttackStand;

                break;

            case State.SwingingBishopAttackStand:
                AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingBishopAfterMoving;

                break;
            case State.SwingingBishopAfterMoving:
                AttackCameraComplete();
                OnBishopStartMoving?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingBishopAttackMoving;

                break;
            case State.SwingingBishopAttackMoving:

                break;
            case State.SwingingBishopBeforeHit:
                OnBishopSwordSlash?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingBishopAfterCamera;


                break;
            case State.SwingingBishopAfterCamera:
                if (!AttackActionSystem.Instance.GetChainStart())
                {
                    ScreenManager._instance._LoadScreenTextuer();
                    TimeAttack(0.1f);
                    state = State.SwingingBishopAfterHit;
                }
                else
                {
                    AttackActionSystem.Instance.SetIsChainAtk_1(true);
                    TimeAttack(0.5f);
                    state = State.SwingingBishopAfterHit;
                }
                AttackActionSystem.Instance.SetIsAtk(false);

                break;
            case State.SwingingBishopAfterHit:
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

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the character is already at
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

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        TimeAttack(0.7f);
        state = State.SwingingBishopBeforeMoving; 

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnBishopStartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetIsAtk(true);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxBishopDistance()
    {
        return maxBishopDistance;
    }

    public override string GetActionName()
    {
        return "ºñ¼ó";
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
