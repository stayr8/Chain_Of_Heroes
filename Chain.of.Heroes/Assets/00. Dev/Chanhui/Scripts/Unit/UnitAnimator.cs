using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;



    private void Awake()
    {

        if(TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<EnemyMoveAction>(out EnemyMoveAction enemymoveAction))
        {
            enemymoveAction.OnStartMoving += MoveAction_OnStartMoving;
            enemymoveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ReadyAction>(out ReadyAction shootAction))
        {
            shootAction.OnShoot += shootAction_OnShoot;
        }

        if (TryGetComponent<LongKnightAction>(out LongKnightAction longKnightAction))
        {
            longKnightAction.OnShoot += LongKnightAction_OnShoot;
        }

        if (TryGetComponent<LongBishopAction>(out LongBishopAction longBishopAction))
        {
            longBishopAction.OnShoot += LongBishopAction_OnShoot;
        }

        if (TryGetComponent<KingAction>(out KingAction kingAction))
        {
            kingAction.OnKingSwordSlash += Action_OnSwordSlash;
            kingAction.OnKingStartMoving += MoveAction_OnStartMoving;
            kingAction.OnKingStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<RookAction>(out RookAction rookAction))
        {
            rookAction.OnRookStartMoving += MoveAction_OnStartMoving;
            rookAction.OnRookStopMoving += MoveAction_OnStopMoving;
            rookAction.OnRookSwordSlash += Action_OnSwordSlash;
        }

        if (TryGetComponent<BishopAction>(out BishopAction bishopAction))
        {
            bishopAction.OnBishopStartMoving += MoveAction_OnStartMoving;
            bishopAction.OnBishopStopMoving += MoveAction_OnStopMoving;
            bishopAction.OnBishopSwordSlash += Action_OnSwordSlash;
        }

        if (TryGetComponent<QueenAction>(out QueenAction queenAction))
        {
            queenAction.OnQueenStartMoving += MoveAction_OnStartMoving;
            queenAction.OnQueenStopMoving += MoveAction_OnStopMoving;
            queenAction.OnQueenSwordSlash += Action_OnSwordSlash;
            //queenAction.OnQueenDash += queenAction_OnQueenDash;
        }

        if (TryGetComponent<KnightAction>(out KnightAction knightAction))
        {
            knightAction.OnKnightStartMoving += MoveAction_OnStartMoving;
            knightAction.OnKnightStopMoving += MoveAction_OnStopMoving;
            knightAction.OnKnightSwordSlash += Action_OnSwordSlash;
        }

        if (TryGetComponent<ChainAttackAction>(out ChainAttackAction chainAttackAction))
        {
            chainAttackAction.OnChainAttackStartMoving += MoveAction_OnStartMoving;
            chainAttackAction.OnChainAttackStopMoving += MoveAction_OnStopMoving;
            chainAttackAction.OnChainAttackSwordSlash += Action_OnSwordSlash;
        }

        if (TryGetComponent<ChainLongAttackAction>(out ChainLongAttackAction chainLongAttackAction))
        {
            chainLongAttackAction.OnChainShoot += shootAction_OnChainShoot;
        }

        if (TryGetComponent<CharacterDataManager>(out CharacterDataManager characterDataManager))
        {
            characterDataManager.OnPlayerDamage += Unit_OnUnitDamage;
            characterDataManager.OnPlayerDie += Unit_OnUnitDie;
        }

        if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterDataManager))
        {
            monsterDataManager.OnEnemyDamage += Unit_OnUnitDamage;
            monsterDataManager.OnEnemyDie += Unit_OnUnitDie;
        }

        if (TryGetComponent<SwordWomanSkill1Action>(out SwordWomanSkill1Action swordWomanSkill1Action))
        {
            swordWomanSkill1Action.OnSWSkill_1_StartMoving += MoveAction_OnStartMoving;
            swordWomanSkill1Action.OnSWSkill_1_StopMoving += MoveAction_OnStopMoving;
            swordWomanSkill1Action.OnSWSkill_1_Slash += Action_OnSkill_1_Slash;
            swordWomanSkill1Action.OnSWSkill_1_Dash += Unit_OnUnitDash;
        }

        if (TryGetComponent<SwordWomanSkill2Action>(out SwordWomanSkill2Action swordWomanSkill2Action))
        {
            swordWomanSkill2Action.OnShoot += SwordWomanSkill2Action_OnShoot;
            
        }

    }

    #region Attack Action
    private void Action_OnSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void Action_OnSkill_1_Slash(object sender, EventArgs e)
    {
        animator.SetTrigger("IsSkill_1");
    }

    private void Action_OnSkill_2_Slash(object sender, EventArgs e)
    {
        animator.SetTrigger("IsSkill_2");
    }

    #endregion

    #region Move Action

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    #endregion


    #region 원거리 공격 Action
    private void shootAction_OnShoot(object sender, ReadyAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = 
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        
        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);

    }

    private void shootAction_OnChainShoot(object sender, ChainLongAttackAction.OnChainShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);

    }

    private void LongKnightAction_OnShoot(object sender, LongKnightAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);

    }

    private void LongBishopAction_OnShoot(object sender, LongBishopAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform =
            Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);

    }

    private void SwordWomanSkill2Action_OnShoot(object sender, SwordWomanSkill2Action.OnShootEventArgs e)
    {
        animator.SetTrigger("IsSkill_2");

        StartCoroutine(WaitShoot(e.targetUnit.GetWorldPosition()));
    }

    IEnumerator WaitShoot(Vector3 e)
    {
        yield return new WaitForSeconds(0.3f);
        Transform bulletProjectileTransform =
                Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        RangeBulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<RangeBulletProjectile>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform2 =
                Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        RangeBulletProjectile bulletProjectile2 = bulletProjectileTransform2.GetComponent<RangeBulletProjectile>();

        Vector3 targetUnitShootAtPosition2 = e;

        targetUnitShootAtPosition2.y = shootPointTransform.position.y;

        bulletProjectile2.Setup(targetUnitShootAtPosition2);

        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform3 =
                Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        RangeBulletProjectile bulletProjectile3 = bulletProjectileTransform3.GetComponent<RangeBulletProjectile>();

        Vector3 targetUnitShootAtPosition3 = e;

        targetUnitShootAtPosition3.y = shootPointTransform.position.y;

        bulletProjectile3.Setup(targetUnitShootAtPosition3);

        yield return new WaitForSeconds(0.6f);
        Transform bulletProjectileTransform4 =
                Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        RangeBulletProjectile bulletProjectile4 = bulletProjectileTransform4.GetComponent<RangeBulletProjectile>();

        Vector3 targetUnitShootAtPosition4 = e;

        targetUnitShootAtPosition4.y = shootPointTransform.position.y;

        bulletProjectile4.Setup(targetUnitShootAtPosition4);
         
    }

    #endregion

    private void Unit_OnUnitDie(object sender, EventArgs e)
    {
        animator.SetBool("IsDie", true);
    }

    private void Unit_OnUnitDamage(object sender, EventArgs e)
    {
        animator.SetTrigger("IsDamage");
    }
    
    private void Unit_OnUnitDash(object sender, EventArgs e)
    {
        animator.SetTrigger("IsDash");
    }


    private void OnDisable()
    {
        /*
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving -= MoveAction_OnStartMoving;
            moveAction.OnStopMoving -= MoveAction_OnStopMoving;
        }

        if (TryGetComponent<EnemyMoveAction>(out EnemyMoveAction enemymoveAction))
        {
            enemymoveAction.OnStartMoving -= MoveAction_OnStartMoving;
            enemymoveAction.OnStopMoving -= MoveAction_OnStopMoving;
        }

        if (TryGetComponent<ReadyAction>(out ReadyAction shootAction))
        {
            shootAction.OnShoot -= shootAction_OnShoot;
        }

        if (TryGetComponent<KingAction>(out KingAction kingAction))
        {
            kingAction.OnKingSwordSlash -= Action_OnSwordSlash;
            kingAction.OnKingStartMoving -= MoveAction_OnStartMoving;
            kingAction.OnKingStopMoving -= MoveAction_OnStopMoving;
        }

        if (TryGetComponent<RookAction>(out RookAction rookAction))
        {
            rookAction.OnRookStartMoving -= MoveAction_OnStartMoving;
            rookAction.OnRookStopMoving -= MoveAction_OnStopMoving;
            rookAction.OnRookSwordSlash -= Action_OnSwordSlash;
        }

        if (TryGetComponent<BishopAction>(out BishopAction bishopAction))
        {
            bishopAction.OnBishopStartMoving -= MoveAction_OnStartMoving;
            bishopAction.OnBishopStopMoving -= MoveAction_OnStopMoving;
            bishopAction.OnBishopSwordSlash -= Action_OnSwordSlash;
        }

        if (TryGetComponent<QueenAction>(out QueenAction queenAction))
        {
            queenAction.OnQueenStartMoving -= MoveAction_OnStartMoving;
            queenAction.OnQueenStopMoving -= MoveAction_OnStopMoving;
            queenAction.OnQueenSwordSlash -= Action_OnSwordSlash;

        }

        if (TryGetComponent<KnightAction>(out KnightAction knightAction))
        {
            knightAction.OnKnightStartMoving -= MoveAction_OnStartMoving;
            knightAction.OnKnightStopMoving -= MoveAction_OnStopMoving;
            knightAction.OnKnightSwordSlash -= Action_OnSwordSlash;
        }

        if (TryGetComponent<ChainAttackAction>(out ChainAttackAction chainAttackAction))
        {
            chainAttackAction.OnChainAttackStartMoving -= MoveAction_OnStartMoving;
            chainAttackAction.OnChainAttackStopMoving -= MoveAction_OnStopMoving;
            chainAttackAction.OnChainAttackSwordSlash -= Action_OnSwordSlash;
        }

        if (TryGetComponent<ChainLongAttackAction>(out ChainLongAttackAction chainLongAttackAction))
        {
            chainLongAttackAction.OnChainShoot -= shootAction_OnChainShoot;
        }

        if (TryGetComponent<CharacterDataManager>(out CharacterDataManager characterDataManager))
        {
            characterDataManager.OnPlayerDamage -= Unit_OnUnitDamage;
            characterDataManager.OnPlayerDie -= Unit_OnUnitDie;
        }

        if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterDataManager))
        {
            monsterDataManager.OnEnemyDamage -= Unit_OnUnitDamage;
            monsterDataManager.OnEnemyDie -= Unit_OnUnitDie;
        }*/
    }
}
