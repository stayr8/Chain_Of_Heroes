using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSalamanderWarriorPassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _mdm.m_damagereductionRate = 0.15f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
