using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWomanPassive : CharacterBase
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
        _cdm.m_criticalRate += 15;
        _cdm.m_criticalDamage += 0.2f;
    }


}