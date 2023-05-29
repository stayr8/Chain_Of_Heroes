using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSkillDebuff : BaseBuff
{
    private List<Binding> Binds = new List<Binding>();

    private float defense;

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (isActive)
            {
                if (!TurnSystem.Property.IsPlayerTurn)
                {
                    buffTurnCount--;
                    if (buffTurnCount <= 0)
                    {
                        _mdm.m_defensePower = defense;
                        ActionComplete();
                    }
                }
            }
        });
        Binds.Add(Bind);

        buffTurnCount = 0;
        defense = 0;
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
    }

    public override void TakeAction(GridPosition gridPosition)
    {
        buffTurnCount = 2;

        defense = _mdm.m_defensePower;
        _mdm.m_defensePower -= _mdm.m_defensePower * 0.1f;

        ActionStart();
    }


    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }
}
