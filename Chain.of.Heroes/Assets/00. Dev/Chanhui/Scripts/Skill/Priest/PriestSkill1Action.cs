using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestSkill1Action : BaseAction
{
    public event EventHandler OnPsSkill_1_Hill;

    [SerializeField] private Transform skill1_effect;


    private enum State
    {
        SwingingPsSkill_1_BeforeSkill,
        SwingingPsSkill_1_Hill,
        SwingingPsSkill_1_AfterHit,
    }

    [SerializeField] private int maxPsSkill_1_Distance = 2;

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

        switch (state)
        {
            case State.SwingingPsSkill_1_BeforeSkill:

                break;
            case State.SwingingPsSkill_1_Hill:

                break;
            case State.SwingingPsSkill_1_AfterHit:

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
            case State.SwingingPsSkill_1_BeforeSkill:
                TimeAttack(0.1f);
                state = State.SwingingPsSkill_1_Hill;

                break;
            case State.SwingingPsSkill_1_Hill:
                OnPsSkill_1_Hill?.Invoke(this, EventArgs.Empty);
                TargetCharacterHill();
                Transform skill1EffectTransform = Instantiate(skill1_effect, targetUnit.transform.position, Quaternion.identity);
                Destroy(skill1EffectTransform.gameObject, 1.5f);
                SoundManager.instance.Priest_1();

                TimeAttack(1.5f);
                state = State.SwingingPsSkill_1_AfterHit;

                break;
            case State.SwingingPsSkill_1_AfterHit:
                UnitActionSystem.Instance.SetCharacterHill(false);
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
        UnitActionSystem.Instance.SetCharacterHill(true);
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxPsSkill_1_Distance; x <= maxPsSkill_1_Distance; x++)
        {
            for (int z = -maxPsSkill_1_Distance; z <= maxPsSkill_1_Distance; z++)
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

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (targetUnit.IsEnemy())
                    {
                        continue;
                    }
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

    private void TargetCharacterHill()
    {
        CharacterDataManager _cmd = targetUnit.GetCharacterDataManager();
        if (_cmd.m_hp == _cmd.m_maxhp)
        {
            return;
        }
        else if ((_cmd.m_maxhp - _cmd.m_hp) < (_cmd.m_maxhp * 0.4f))
        {
            _cmd.m_hp = _cmd.m_maxhp;
        }
        else
        {
            _cmd.m_hp += (_cmd.m_maxhp * 0.4f) * 1.2f;
        }
    }


    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isSkill = true;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        if (isSkillCount <= 0)
        {
            isSkillCount = 2;
        }

        state = State.SwingingPsSkill_1_BeforeSkill;
        TimeAttack(0.7f);

        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxPsSkill_1_Distance()
    {
        return maxPsSkill_1_Distance;
    }

    public override string GetActionName()
    {
        return "Àç»ýÀÇºû";
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
