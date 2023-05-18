using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPassive : BaseBuff
{

    private void Start()
    {
        Passive();
    }


    private void Passive()
    {
        _cdm.m_criticalRate += 30;
        _cdm.m_criticalDamage += 0.4f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
