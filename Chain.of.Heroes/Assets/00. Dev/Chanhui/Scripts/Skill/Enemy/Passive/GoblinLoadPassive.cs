using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinLoadPassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _mdm.m_attackPower *= 1.1f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
