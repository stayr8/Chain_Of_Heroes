using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGolemPassive : BaseBuff
{
    private void Start()
    {
        Passive();
    }

    private void Passive()
    {
        _mdm.m_hp += _mdm.m_hp * 0.1f;
        //_mdm.m_ += 0.2f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }
}
