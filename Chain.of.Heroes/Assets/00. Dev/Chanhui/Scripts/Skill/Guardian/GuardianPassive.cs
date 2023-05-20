using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianPassive : BaseBuff
{

    private void Start()
    {
        Passive();
    }


    private void Passive()
    {
        _cdm.m_damagereductionRate = 0.15f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
