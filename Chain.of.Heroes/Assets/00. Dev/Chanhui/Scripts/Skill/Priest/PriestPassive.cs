using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriestPassive : CharacterBase
{
    private CharacterDataManager _cdm;

    private void Awake()
    {
        _cdm = GetComponent<CharacterDataManager>();
    }

}
