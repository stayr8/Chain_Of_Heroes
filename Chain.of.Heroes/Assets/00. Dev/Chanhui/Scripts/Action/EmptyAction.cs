using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAction : BaseAction
{
    /* 나중에 몬스터가 가만히 있게 만들어 줄 수도 있다.*/


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();      

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        
    }

    public override string GetActionName()
    {
        return "Empty";
    }

    public override string GetSingleActionPoint()
    {
        return "";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }
}
