using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackStoneGolemPassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _mdm.m_hp += _mdm.m_hp * 0.1f;
        _mdm.m_defensePower += _mdm.m_defensePower * 0.1f;
        _mdm.m_damagereductionRate = 0.1f;

    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
