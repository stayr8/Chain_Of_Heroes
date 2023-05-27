using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainCheck : MonoBehaviour
{

    private Unit enemy;

    private int chainDistance;

    private List<Binding> Binds = new List<Binding>();
    private List<GridPosition> validGridPositionList;

    int count;

    void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPointUse", (object value) =>
        {
            Debug.Log(validGridPositionList.Count);
            if (count > 1)
            {
                Debug.Log("체인 캐릭터 발견");
                Unit targetUnits = LevelGrid.Instance.GetUnitAtGridPosition(validGridPositionList[0]);
                targetUnits.SetIsChainPossibleState(true);
                Unit targetUnits2 = LevelGrid.Instance.GetUnitAtGridPosition(validGridPositionList[1]);
                targetUnits2.SetIsChainPossibleState(true);
            }
            Debug.Log("체인 체크");
        }, false);
        Binds.Add(Bind);

        enemy = GetComponent<Unit>();
        chainDistance = 1;
        count = 0;
    }


    public List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = enemy.GetGridPosition();
        validGridPositionList = new List<GridPosition>();

        for (int x = -chainDistance; x <= chainDistance; x++)
        {
            for (int z = -chainDistance; z <= chainDistance; z++)
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
                if (testDistance > chainDistance)
                {
                    continue;
                }

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (targetUnit.GetIsStun())
                    {
                        continue;
                    }
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position already occupied with another Character
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitAtEnemyGridPosition(testGridPosition))
                {
                    // Is the unit on the grid a monster
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                Debug.Log("주변에 플레이어가 있어요");
                validGridPositionList.Add(testGridPosition);
                count++;
            }
        }
        return validGridPositionList;
    }



    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }
}
