using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianPassive : CharacterBase
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
        
    }
}
