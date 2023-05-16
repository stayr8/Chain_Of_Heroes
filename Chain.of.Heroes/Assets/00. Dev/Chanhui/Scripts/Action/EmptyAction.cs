using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAction : BaseAction
{
    /* ���߿� ���Ͱ� ������ �ְ� ����� �� ���� �ִ�.*/


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

    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
