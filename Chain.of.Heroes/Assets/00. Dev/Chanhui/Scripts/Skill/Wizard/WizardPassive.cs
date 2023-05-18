using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPassive : BaseBuff
{

    private void Start()
    {
        Passive();
    }


    private void Passive()
    {
        _cdm.m_criticalRate -= 12;
        _cdm.m_criticalDamage += 3.0f;
    }

    public override void TakeAction(GridPosition gridPosition)
    {
    }

    // 나중에 스킬 행동력 포인터 -1 감소 시키기
}
