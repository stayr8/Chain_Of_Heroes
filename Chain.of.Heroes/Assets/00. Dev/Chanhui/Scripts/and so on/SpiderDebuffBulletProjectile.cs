using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDebuffBulletProjectile : MonoBehaviour
{
    private Vector3 targetPosition;

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {

        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 30f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            GetCharacterBuffOn();
            Destroy(gameObject);
        }
    }

    private int DebuffDistance = 1;
    public void GetCharacterBuffOn()
    {
        GridPosition unitGridPosition = AttackActionSystem.Instance.GetCharacterChainFind().GetGridPosition();
        Debug.Log(AttackActionSystem.Instance.GetCharacterChainFind());
        for (int x = -DebuffDistance; x <= DebuffDistance; x++)
        {
            for (int z = -DebuffDistance; z <= DebuffDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetplayer = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (!targetplayer.IsEnemy())
                    {
                        foreach (BaseBuff baseBuff in targetplayer.GetBaseBuffArray())
                        {
                            if (targetplayer.GetBuff<SpiderSkillDebuff>() == baseBuff)
                            {
                                baseBuff.TakeAction(testGridPosition);
                                targetplayer.GetCharacterDataManager().SkillDamage();
                            }
                        }
                    }
                }
            }
        }
    }
}
