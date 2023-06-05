using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProvokeAction : BaseAction
{
    [SerializeField] private Transform skill1_effect;
    [SerializeField] private Transform skill1_effect_transform;
    private Transform skill1EffectTransform;

    private bool oneffect;

    private List<Binding> Binds = new List<Binding>();

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (isActive)
            {
                if (TurnSystem.Property.IsPlayerTurn)
                {
                    if (!isProvoke)
                    {
                        Debug.Log(isProvoke);
                        Destroy(skill1EffectTransform.gameObject);
                        oneffect = false;
                    }
                }
                
            }
        });
        Binds.Add(Bind);

        oneffect = false;
    }

    private void Update()
    {
        if(isProvoke && !oneffect)
        {
            skill1EffectTransform = Instantiate(skill1_effect, skill1_effect_transform.position, Quaternion.identity);
            skill1EffectTransform.transform.parent = skill1_effect_transform;
            skill1EffectTransform.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            oneffect = true;
        }
        else if(unit.GetIsStun())
        {
            Destroy(skill1EffectTransform.gameObject);
            oneffect = false;
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

    }

    public override string GetActionName()
    {
        return "Provoke";
    }

    public override string GetSingleActionPoint()
    {
        return "";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
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
