using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAction : BaseAction
{
    [SerializeField] private int maxChainDistance = 1;

    private Unit targetUnit;

    private void Update()
    { 
    
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
       
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        Debug.Log(targetUnit.GetUnitName());
    }

    // 이동 범위 선정
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxChainDistance; x <= maxChainDistance; x++)
        {
            for (int z = -maxChainDistance; z <= maxChainDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the character is already at
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (AttackActionSystem.Instance.GetPlayer() == targetUnit)
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxChainDistance)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position already occupied with another Character
                    continue;
                }


                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }


                int pathfindingDistanceMultiplier = 12;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxChainDistance * pathfindingDistanceMultiplier)
                {
                    // Path length is too long
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }



    public override string GetActionName()
    {
        return "ChainAction";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
       
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue =  0,
        };
    }

    public int GetMaxChainDistance()
    {
        return maxChainDistance;
    }


    public override int GetActionPointsCost()
    {
        return 0;
    }

    public override string GetSingleActionPoint()
    {
        return "";
    }
}
