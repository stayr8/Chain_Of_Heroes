using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStoneGolemPassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _mdm.m_hp += _mdm.m_hp * 0.2f;
        _mdm.m_defensePower += _mdm.m_defensePower * 0.2f;
        _mdm.m_damagereductionRate = 0.2f;

    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
