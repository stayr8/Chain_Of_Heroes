using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestSkill2Action : BaseAction
{
    public event EventHandler OnPsSkill_2_Hill;

    [SerializeField] private Transform skill2_effect;
    [SerializeField] private Transform skill2_effect_transform;

    private enum State
    {
        SwingingPsSkill_2_BeforeSkill,
        SwingingPsSkill_2_Hill,
        SwingingPsSkill_2_AfterHit,
    }

    [SerializeField] private int maxPsSkill_2_Distance = 2;

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
            case State.SwingingPsSkill_2_BeforeSkill:

                break;
            case State.SwingingPsSkill_2_Hill:

                break;
            case State.SwingingPsSkill_2_AfterHit:

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
            case State.SwingingPsSkill_2_BeforeSkill:
                TimeAttack(0.1f);
                state = State.SwingingPsSkill_2_Hill;

                break;
            case State.SwingingPsSkill_2_Hill:
                OnPsSkill_2_Hill?.Invoke(this, EventArgs.Empty);
                GetCharacterHillOn();
                Transform skill1EffectTransform = Instantiate(skill2_effect, skill2_effect_transform.position, Quaternion.identity);
                Destroy(skill1EffectTransform.gameObject, 1.5f);
                SoundManager.instance.Priest_2();

                TimeAttack(1.0f);
                state = State.SwingingPsSkill_2_AfterHit;

                break;
            case State.SwingingPsSkill_2_AfterHit:

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

        for (int x = -maxPsSkill_2_Distance; x <= maxPsSkill_2_Distance; x++)
        {
            for (int z = -maxPsSkill_2_Distance; z <= maxPsSkill_2_Distance; z++)
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

    int HillDistance = 1;
    public void GetCharacterHillOn()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        for (int x = -HillDistance; x <= HillDistance; x++)
        {
            for (int z = -HillDistance; z <= HillDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (!targetUnit.IsEnemy())
                    {
                        CharacterDataManager _cmd = targetUnit.GetCharacterDataManager();
                        if (_cmd.m_hp == _cmd.m_maxhp)
                        {
                            continue;
                        }
                        else if((_cmd.m_maxhp - _cmd.m_hp) < (_cmd.m_maxhp * 0.2f))
                        {
                            _cmd.m_hp = _cmd.m_maxhp;
                        }
                        else
                        {
                            _cmd.m_hp += (_cmd.m_maxhp * 0.2f) * 1.2f;
                        }
                    }
                }
            }
        }

    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isSkill = true;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        if (isSkillCount <= 0)
        {
            isSkillCount = 4;
        }

        state = State.SwingingPsSkill_2_BeforeSkill;
        TimeAttack(0.7f);

        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxPsSkill_2_Distance()
    {
        return maxPsSkill_2_Distance;
    }

    public override string GetActionName()
    {
        return "대천사의축복";
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
