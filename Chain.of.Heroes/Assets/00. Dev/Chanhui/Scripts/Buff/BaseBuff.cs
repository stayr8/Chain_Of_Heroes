using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff : MonoBehaviour
{

    protected Unit unit;
    protected CharacterDataManager _cdm;
    protected MonsterDataManager _mdm;

    protected bool isActive;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        if (TryGetComponent<CharacterDataManager>(out CharacterDataManager characterDataManager))
        {
            this._cdm = characterDataManager;
        }
        if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterDataManager))
        {
            this._mdm = monsterDataManager;
        }
    }

    public abstract void TakeAction(GridPosition gridPosition);

    protected void ActionStart()
    {
        isActive = true;


    }

    protected void ActionComplete()
    {
        isActive = false;


    }

    public Unit GetUnit()
    {
        return unit;
    }
}
