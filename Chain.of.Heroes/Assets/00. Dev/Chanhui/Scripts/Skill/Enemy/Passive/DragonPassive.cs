using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonPassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _mdm.m_attackPower -= _mdm.m_attackPower * 0.2f;
        _mdm.m_defensePower -= _mdm.m_defensePower * 0.2f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
