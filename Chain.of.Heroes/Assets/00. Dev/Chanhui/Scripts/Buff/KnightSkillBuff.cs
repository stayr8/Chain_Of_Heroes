using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSkillBuff : BaseBuff
{

    private List<Binding> Binds = new List<Binding>();

    private CharacterDataManager cdm;
    

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
                        cdm.m_attackPower -= atkPowerBuff;
                        atkPowerBuff = 0;
                        ActionComplete();
                    }
                }
            }
        });
        Binds.Add(Bind);

        buffTurnCount = 0;
    }

    private void Update()
    {
        if(!isActive)
        {
            return;
        }
    }



    public override void TakeAction(GridPosition gridPosition)
    {
        buffTurnCount = 2;

        cdm = unit.GetCharacterDataManager();
        atkPowerBuff = (int)((int)cdm.m_attackPower * 0.3f);
        cdm.m_attackPower += atkPowerBuff;

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
