using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff : MonoBehaviour
{

    protected Unit unit;
    protected CharacterDataManager _cdm;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        _cdm = GetComponent<CharacterDataManager>();
    }

    public abstract void TakeAction(GridPosition gridPosition);

    protected void ActionStart(Action onActionComplete)
    {

    }

    protected void ActionComplete()
    {
          
    }

    public Unit GetUnit()
    {
        return unit;
    }
}
