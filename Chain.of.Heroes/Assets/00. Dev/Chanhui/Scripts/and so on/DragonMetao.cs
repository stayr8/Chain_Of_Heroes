using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMetao : MonoBehaviour
{
    private Vector3 targetPosition;
    int count = 0;

    int maxMoveDistance = 1;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {

        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float moveSpeed = 30f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (targetPosition.y >= transform.position.y)
        {
            if (count <= 0)
            {
                GetValidActionGridPositionList();
                Destroy(gameObject, 0.3f);
                count++;
            }
        }
    }

    public void GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = AttackActionSystem.Instance.GetenemyChainFind().GetGridPosition();

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

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (!targetUnit.IsEnemy())
                    {
                        targetUnit.GetCharacterDataManager().SkillDamage();
                    }
                }
            }
        }
    }
}
