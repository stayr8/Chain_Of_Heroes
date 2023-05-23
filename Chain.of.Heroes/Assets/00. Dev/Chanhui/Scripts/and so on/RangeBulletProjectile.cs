using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBulletProjectile : MonoBehaviour
{
    //[SerializeField] private TrailRenderer trailRenderer;
    //[SerializeField] private Transform bulletHitVfxPrefab;
    public ParticleSystem particle;
    private Vector3 targetPosition;

    int maxMoveDistance = 1;

    private void Start()
    {
        if (particle != null)
        {
            particle.Play();
            StartCoroutine(Pause());
        }
    }

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(0.1f);
        particle.Pause();
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    public void Setup(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }

    private void Update()
    {
        Debug.Log("여기 들어오니?");
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPosition);

        float moveSpeed = 30f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPosition);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPosition;
            Debug.Log("여기도?");
            //trailRenderer.transform.parent = null;
            GetValidActionGridPositionList();
            Destroy(gameObject);
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
                    if (targetUnit.IsEnemy())
                    {
                        targetUnit.GetMonsterDataManager().Damage();
                    }
                }
            }
        }
    }
}
