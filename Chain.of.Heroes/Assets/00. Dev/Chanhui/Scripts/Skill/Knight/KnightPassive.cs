using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPassive : MonoBehaviour
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
        _cdm.m_attackPower *= 1.15f;
    }
}
