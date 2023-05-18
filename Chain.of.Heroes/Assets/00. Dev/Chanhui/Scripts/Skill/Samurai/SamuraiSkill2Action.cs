using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiSkill2Action : BaseAction
{
    public event EventHandler OnSrSkill_2_StartMoving;
    public event EventHandler OnSrSkill_2_StopMoving;
    public event EventHandler OnSrSkill_2_Slash;


    private List<Vector3> positionList;
    private int currentPositionIndex;

    private enum State
    {
        SwingingSrSkill_2_BeforeMoving,
        SwingingSrSkill_2_Moving,
        SwingingSrSkill_2_Attacking,
        SwingingSrSkill_2_AfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxSrSkill_2_Distance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;

    private List<Binding> Binds = new List<Binding>();

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (TurnSystem.Property.IsPlayerTurn)
            {
                if (isSkill && isSkillCount > 0)
                {
                    isSkillCount -= 1;
                }

                if (isSkillCount <= 0)
                {
                    isSkill = false;
                }
            }
        });
        Binds.Add(Bind);

        isSkillCount = 0;
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


        if (state == State.SwingingSrSkill_2_BeforeMoving)
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
                    OnSrSkill_2_StopMoving?.Invoke(this, EventArgs.Empty);
                    //UnitActionSystem.Instance.SetCameraPointchange(true);
                    currentPositionIndex++;
                    state = State.SwingingSrSkill_2_Moving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }


        switch (state)
        {
            case State.SwingingSrSkill_2_BeforeMoving:

                break;
            case State.SwingingSrSkill_2_Moving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingSrSkill_2_Attacking:

                break;
            case State.SwingingSrSkill_2_AfterHit:

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
            case State.SwingingSrSkill_2_BeforeMoving:

                break;
            case State.SwingingSrSkill_2_Moving:

                TimeAttack(0.1f);
                state = State.SwingingSrSkill_2_Attacking;

                break;
            case State.SwingingSrSkill_2_Attacking:
                OnSrSkill_2_Slash?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingSrSkill_2_AfterHit;

                break;
            case State.SwingingSrSkill_2_AfterHit:

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

        for (int x = -maxSrSkill_2_Distance; x <= maxSrSkill_2_Distance; x++)
        {
            for (int z = -maxSrSkill_2_Distance; z <= maxSrSkill_2_Distance; z++)
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

                if (LevelGrid.Instance.GetEnemyAtSurroundPosition(testGridPosition))
                {
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
        isSkill = true;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        if (isSkillCount <= 0)
        {
            isSkillCount = 3;
        }

        state = State.SwingingSrSkill_2_BeforeMoving;
        TimeAttack(0.7f);

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnSrSkill_2_StartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxSrSkill_2_Distance()
    {
        return maxSrSkill_2_Distance;
    }

    public override string GetActionName()
    {
        return "¹Ý¿ù¼¶";
    }

    public override string GetSingleActionPoint()
    {
        return "3";
    }

    public override int GetActionPointsCost()
    {
        return 3;
    }

    public override int GetSkillCountPoint()
    {
        return isSkillCount;
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }
}
