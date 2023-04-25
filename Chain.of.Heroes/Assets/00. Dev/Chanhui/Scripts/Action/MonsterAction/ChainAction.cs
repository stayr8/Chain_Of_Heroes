using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAction : BaseAction
{
    [SerializeField] private int maxChainDistance = 1;

    private Unit targetUnit;
    private Unit targetUnit2;

    private List<GridPosition> validGridPositionList;

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        // 발견 된 체인 플레이어가 Action을 시작하는 곳
        if(validGridPositionList.Count > 1)
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(validGridPositionList[0]);
            targetUnit2 = LevelGrid.Instance.GetUnitAtGridPosition(validGridPositionList[1]);

            //Debug.Log(targetUnit.GetUnitName());
            //Debug.Log(targetUnit2.GetUnitName());
            BaseAction bestBaseAction = null;
            BaseAction bestBaseAction2 = null;
            if (targetUnit.GetIsAttackDistance())
            {
                bestBaseAction = targetUnit.GetAction<ChainLongAttackAction>();
                targetUnit.SetChainfirst(true);
            }
            else
            {
                bestBaseAction = targetUnit.GetAction<ChainAttackAction>();
                targetUnit.SetChainfirst(true);
            }

            if (targetUnit2.GetIsAttackDistance())
            {
                bestBaseAction2 = targetUnit2.GetAction<ChainLongAttackAction>();
                targetUnit2.SetChaintwo(true);
            }
            else
            {
                bestBaseAction2 = targetUnit2.GetAction<ChainAttackAction>();
                targetUnit2.SetChaintwo(true);
            }

            bestBaseAction.TakeAction(unit.GetGridPosition(), onActionComplete);
            bestBaseAction2.TakeAction(unit.GetGridPosition(), onActionComplete);
        }
        else
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            //Debug.Log(targetUnit.GetUnitName());
            BaseAction bestBaseAction = null;
            if (targetUnit.GetIsAttackDistance())
            {
                bestBaseAction = targetUnit.GetAction<ChainLongAttackAction>();
                targetUnit.SetChainfirst(true);
            }
            else
            {
                bestBaseAction = targetUnit.GetAction<ChainAttackAction>();
                targetUnit.SetChainfirst(true);
            }


            bestBaseAction.TakeAction(unit.GetGridPosition(), onActionComplete);
        }
        
    }
    

    // 이동 범위 선정
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        validGridPositionList = new List<GridPosition>();

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

                Unit targetUnit = UnitActionSystem.Instance.GetSelecterdUnit();
                if (targetUnit == LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    continue;
                }
                /*
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (AttackActionSystem.Instance.GetPlayer() == targetUnit)
                {
                    continue;
                }*/
                /*
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same 'team'
                    continue;
                }*/

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
