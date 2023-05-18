using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWomanPassive : BaseBuff
{

    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _cdm.m_criticalRate += 15;
        _cdm.m_criticalDamage += 0.2f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}