using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuff : MonoBehaviour
{

    protected Unit unit;


    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
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
