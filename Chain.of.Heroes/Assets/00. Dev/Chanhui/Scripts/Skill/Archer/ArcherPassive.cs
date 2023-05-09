using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherPassive : MonoBehaviour
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
        _cdm.m_criticalRate += 30;
        _cdm.m_criticalDamage += 0.4f;
    }
}
