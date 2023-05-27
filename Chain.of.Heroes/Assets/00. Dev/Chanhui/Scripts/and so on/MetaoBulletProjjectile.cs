using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaoBulletProjjectile : MonoBehaviour
{

    private Vector3 targetPosition;

    int maxMoveDistance = 1;
    int count = 0;

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
            if (count <= 2)
            {
                GetValidActionGridPositionList();
            }
            else
            {
                Destroy(gameObject, 0.3f);
            }
        }
    }

    int BossAttckCount = 0;
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
                    if (targetUnit.IsEnemy())
                    {
                        if (targetUnit.GetEnemyVisualType() == Unit.EnemyType.Dragon ||
                            targetUnit.GetEnemyVisualType() == Unit.EnemyType.RedStoneGolem)
                        {
                            if (BossAttckCount <= 0)
                            {
                                targetUnit.GetMonsterDataManager().SkillDamage();
                                BossAttckCount++;
                            }
                        }
                        else
                        {
                            targetUnit.GetMonsterDataManager().SkillDamage();
                        }
                    }
                }
            }
        }
        count++;
    }
}
