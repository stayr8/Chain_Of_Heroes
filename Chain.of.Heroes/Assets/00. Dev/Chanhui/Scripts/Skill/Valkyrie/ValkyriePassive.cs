using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _cdm.m_attackPower *= 1.1f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
