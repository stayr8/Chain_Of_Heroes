using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAction : BaseAction
{
    public event EventHandler OnUnitStun_Start;
    public event EventHandler OnUnitStun_Stop;

    [SerializeField] private Transform skill1_effect;
    [SerializeField] private Transform skill1_effect_transform;
    private Transform skill1EffectTransform;

    private List<Binding> Binds = new List<Binding>();

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    private enum State
    {
        Unit_StunStart,
        Unit_StunStop,
        Unit_Stun,
    }

    private State state;
    private float stateTimer;

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (isActive)
            {
                if (TurnSystem.Property.IsPlayerTurn)
                {
                    if (isSkill && isSkillCount > 0)
                    {
                        isSkillCount -= 1;
                    }

                    if (isSkillCount <= 0)
                    {
                        ActionComplete();
                        OnUnitStun_Stop?.Invoke(this, EventArgs.Empty);
                        isSkill = false;
                        unit.SetIsStun(false);
                        Destroy(skill1EffectTransform.gameObject);
                    }
                }
                else
                {
                    if (isSkill && isSkillCount > 0 && unit.IsEnemy())
                    {
                        unit.SetSoloEnemyActionPoints(0);
                    }
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
            case State.Unit_StunStart:

                break;
            case State.Unit_StunStop:
                
                break;
            case State.Unit_Stun:

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
            case State.Unit_StunStart:
                TimeAttack(0.5f);
                state = State.Unit_StunStop;

                break;
            case State.Unit_StunStop:
                OnUnitStun_Start?.Invoke(this, EventArgs.Empty);
                skill1EffectTransform = Instantiate(skill1_effect, skill1_effect_transform.position, Quaternion.identity);
                skill1EffectTransform.transform.parent = skill1_effect_transform.parent;
                state = State.Unit_Stun;

                break;
            case State.Unit_Stun:

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
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isSkill = true;
        if (isSkillCount <= 0)
        {
            isSkillCount = 2;
        }


        state = State.Unit_StunStart;

        ActionStart(onActionComplete);
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }

    public override string GetSingleActionPoint()
    {
        return "0";
    }

    public override string GetActionName()
    {
        return "½ºÅÏ";
    }

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }
}
