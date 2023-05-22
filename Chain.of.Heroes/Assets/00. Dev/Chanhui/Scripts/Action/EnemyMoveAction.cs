using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance;
    [SerializeField] private int minMoveDistance;

    private List<Vector3> positionList;
    private int currentPositionIndex = 0;


    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized; 

        float stoppingDistance = 0.1f;
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            float rotateSpeed = 80f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

        }
        else
        {
            int Distance = minMoveDistance > positionList.Count ? positionList.Count : minMoveDistance;
            if (currentPositionIndex >= Distance - 1)
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);

                ActionComplete();
            }
            else
            {
                currentPositionIndex++;
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 1;
        positionList = new List<Vector3>();

        foreach (GridPosition pathgridPosition in pathgridPositionList)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPosition));
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    // 이동 범위 선정
    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
         List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
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

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                int maxMovevalue = (maxMoveDistance * 2) - 1;                
                if (testDistance >= maxMovevalue)
                {
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    continue;
                }
                
                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                if (!Pathfinding.Instance.HasAtPath(unitGridPosition, testGridPosition))
                {
                    continue;
                }
                
                int pathfindingDistanceMultiplier = 12;
                if (Pathfinding.Instance.GetPathLength(unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier)
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
        return "EnemyMove";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = 0;

        if (unit.GetEnemyVisualType() == Unit.EnemyType.Archer)
        {
            targetCountAtGridPosition = unit.GetAction<ReadyAction>().GetTargetCountAtPosition(gridPosition);
        }
        else if (unit.GetEnemyVisualType() == Unit.EnemyType.Sword || unit.GetEnemyVisualType() == Unit.EnemyType.RedStoneGolem)
        {
            targetCountAtGridPosition = unit.GetAction<KingAction>().GetTargetCountAtPosition(gridPosition);
        }


        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = targetCountAtGridPosition * 10,
        };
    }

    public override int GetActionPointsCost()
    {
        if (unit.GetEnemyVisualType() == Unit.EnemyType.Archer)
        {
            return 1;
        }
        else if (unit.GetEnemyVisualType() == Unit.EnemyType.Sword)
        {
            return 1;
        }
        else if (unit.GetEnemyVisualType() == Unit.EnemyType.RedStoneGolem)
        {
            return 4;
        }
        return 0;
    }

    public override string GetSingleActionPoint()
    {
        return "";
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
