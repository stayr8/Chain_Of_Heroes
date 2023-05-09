using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManPassive : MonoBehaviour
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
        _cdm.m_criticalRate += 10;
        _cdm.m_criticalDamage += 0.1f;
        _cdm.m_attackPower *= 1.1f;
    }
}
