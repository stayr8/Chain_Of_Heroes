using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianSkill2Action : BaseAction
{
    public event EventHandler OnGdSkill_2_provoke;

    private List<Vector3> positionList;

    [SerializeField] private Transform skill2_effect;
    [SerializeField] private Transform skill2_effect_transform;

    private enum State
    {
        SwingingGdSkill_2_BeforeSkill,
        SwingingGdSkill_2_Provoke,
        SwingingGdSkill_2_AfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxGdSkill_2_Distance = 1;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private float lastTime;

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
                    lastTime -= 1;
                }

                if (isSkillCount <= 0)
                {
                    isSkill = false;
                }

                if(lastTime <= 0)
                {
                    //Debug.Log("도발 종료");
                    isProvoke = false;
                }
            }
        }, false);
        Binds.Add(Bind);

        isSkillCount = 0;
        lastTime = 0;
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
            case State.SwingingGdSkill_2_BeforeSkill:

                break;
            case State.SwingingGdSkill_2_Provoke:

                break;
            case State.SwingingGdSkill_2_AfterHit:

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
            case State.SwingingGdSkill_2_BeforeSkill:
                TimeAttack(0.1f);
                state = State.SwingingGdSkill_2_Provoke;

                break;
            case State.SwingingGdSkill_2_Provoke:
                OnGdSkill_2_provoke?.Invoke(this, EventArgs.Empty);
                isProvoke = true;
                Invoke("Effect", 0.3f);

                TimeAttack(2.0f);
                state = State.SwingingGdSkill_2_AfterHit;

                break;
            case State.SwingingGdSkill_2_AfterHit:

                ActionComplete();

                break;
        }
    }
    void TimeAttack(float StateTime)
    {
        float afterHitStateTime = StateTime;
        stateTimer = afterHitStateTime;
    }

    void Effect()
    {
        Debug.Log("도발 시작");
        Transform skill1EffectTransform = Instantiate(skill2_effect, skill2_effect_transform.position, Quaternion.identity);
        Destroy(skill1EffectTransform.gameObject, 1.5f);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxGdSkill_2_Distance; x <= maxGdSkill_2_Distance; x++)
        {
            for (int z = -maxGdSkill_2_Distance; z <= maxGdSkill_2_Distance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
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
        lastTime = 2;
        
        state = State.SwingingGdSkill_2_BeforeSkill;
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

    public int GetMaxGdSkill_2_Distance()
    {
        return maxGdSkill_2_Distance;
    }

    public override string GetActionName()
    {
        return "도발";
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
