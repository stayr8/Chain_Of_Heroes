using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteSkeletonArcherPassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _mdm.m_criticalRate += 0.2f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
