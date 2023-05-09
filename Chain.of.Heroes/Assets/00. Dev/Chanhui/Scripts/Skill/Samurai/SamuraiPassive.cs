using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiPassive : MonoBehaviour
{
    private CharacterDataManager _cdm;

    private void Awake()
    {
        _cdm = GetComponent<CharacterDataManager>();
    }


    private void Start()
    {
        Passive();
    }


    private void Passive()
    {
        _cdm.m_criticalRate += 20;
        _cdm.m_criticalDamage += 0.3f;
    }

    // 나중에 스킬 행동력 포인터 -1 감소 시키기
}
