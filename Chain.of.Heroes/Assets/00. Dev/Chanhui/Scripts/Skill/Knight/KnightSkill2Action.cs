using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSkill2Action : BaseAction
{
    public event EventHandler OnKnSkill_2_Buff;


    private List<Vector3> positionList;

    private enum State
    {
        SwingingKnSkill_2_BeforeSkill,
        SwingingKnSkill_2_Buff,
        SwingingKnSkill_2_AfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxKnSkill_2_Distance = 1;

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

        isSkillCount = 0;
    }

    private void Update()
    {

        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingKnSkill_2_BeforeSkill:

                break;
            case State.SwingingKnSkill_2_Buff:

                break;
            case State.SwingingKnSkill_2_AfterHit:

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
            case State.SwingingKnSkill_2_BeforeSkill:
                TimeAttack(0.1f);
                state = State.SwingingKnSkill_2_Buff;

                break;
            case State.SwingingKnSkill_2_Buff:
                OnKnSkill_2_Buff?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingKnSkill_2_AfterHit;

                break;
            case State.SwingingKnSkill_2_AfterHit:

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

        for (int x = -maxKnSkill_2_Distance; x <= maxKnSkill_2_Distance; x++)
        {
            for (int z = -maxKnSkill_2_Distance; z <= maxKnSkill_2_Distance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if(!targetUnit.IsEnemy())
                    {
                        foreach (BaseBuff baseBuff in targetUnit.GetBaseBuffArray())
                        {
                            if(targetUnit.GetBuff<KnightSkillBuff>() == baseBuff)
                            {
                                baseBuff.TakeAction(testGridPosition);
                            }
                        }
                    }
                }

                if (unitGridPosition != testGridPosition)
                {
                    // Same Grid Position where the character is already at
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

        state = State.SwingingKnSkill_2_BeforeSkill;
        TimeAttack(0.7f);

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxSWSkill_2_Distance()
    {
        return maxKnSkill_2_Distance;
    }

    public override string GetActionName()
    {
        return "��ų2";
    }

    public override string GetSingleActionPoint()
    {
        return "4";
    }

    public override int GetActionPointsCost()
    {
        return 4;
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
