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
       
    }

    #region Attack Action
    private void Action_OnSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
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

    private void Unit_OnUnitDie(object sender, EventArgs e)
    {
        animator.SetBool("IsDie", true);
    }

    private void Unit_OnUnitDamage(object sender, EventArgs e)
    {
        animator.SetTrigger("IsDamage");
    }
    /*
    private void queenAction_OnQueenDash(object sender, EventArgs e)
    {
        animator.SetTrigger("IsDash");
    }*/


    private void OnDisable()
    {
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
        }
    }
}
