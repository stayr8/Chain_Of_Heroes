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
            enemymoveAction.OnStartMoving += EnemyMoveAction_OnStartMoving;
            enemymoveAction.OnStopMoving += EnemyMoveAction_OnStopMoving;
        }

        if (TryGetComponent<ReadyAction>(out ReadyAction shootAction))
        {
            shootAction.OnShoot += shootAction_OnShoot;
        }

        if (TryGetComponent<KingAction>(out KingAction kingAction))
        {
            kingAction.OnKingSwordSlash += KingAction_OnKingSwordSlash;
            kingAction.OnKingStartMoving += KingAction_OnKingStartMoving;
            kingAction.OnKingStopMoving += KingAction_OnKingStopMoving;
        }

        if (TryGetComponent<RookAction>(out RookAction rookAction))
        {
            rookAction.OnRookStartMoving += rookAction_OnRookStartMoving;
            rookAction.OnRookStopMoving += rookAction_OnRookStopMoving;
            rookAction.OnRookSwordSlash += rookAction_OnRookSwordSlash;
        }

        if (TryGetComponent<BishopAction>(out BishopAction bishopAction))
        {
            bishopAction.OnBishopStartMoving += bishopAction_OnBishopStartMoving;
            bishopAction.OnBishopStopMoving += bishopAction_OnBishopStopMoving;
            bishopAction.OnBishopSwordSlash += bishopAction_OnBishopSwordSlash;
        }

        if (TryGetComponent<QueenAction>(out QueenAction queenAction))
        {
            queenAction.OnQueenStartMoving += queenAction_OnQueenStartMoving;
            queenAction.OnQueenStopMoving += queenAction_OnQueenStopMoving;
            queenAction.OnQueenSwordSlash += queenAction_OnQueenSwordSlash;
            //queenAction.OnQueenDash += queenAction_OnQueenDash;
        }

        if (TryGetComponent<KnightAction>(out KnightAction knightAction))
        {
            knightAction.OnKnightStartMoving += knightAction_OnKnightStartMoving;
            knightAction.OnKnightStopMoving += knightAction_OnKnightStopMoving;
            knightAction.OnKnightSwordSlash += knightAction_OnKnightSwordSlash;
        }

        if (TryGetComponent<ChainAttackAction>(out ChainAttackAction chainAttackAction))
        {
            chainAttackAction.OnChainAttackStartMoving += ChainAttackAction_OnChainAttackStartMoving;
            chainAttackAction.OnChainAttackStopMoving += ChainAttackAction_OnChainAttackStopMoving;
            chainAttackAction.OnChainAttackSwordSlash += ChainAttackAction_OnChainAttackSwordSlash;
        }

        if (TryGetComponent<Unit>(out Unit unit))
        {
            unit.OnUnitDamage += Unit_OnUnitDamage;
            unit.OnUnitDie += Unit_OnUnitDie;
        }

    }

    #region Attack Action
    private void KingAction_OnKingSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void rookAction_OnRookSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void bishopAction_OnBishopSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void queenAction_OnQueenSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void knightAction_OnKnightSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void ChainAttackAction_OnChainAttackSwordSlash(object sender, EventArgs e)
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

    private void EnemyMoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void EnemyMoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }
    private void rookAction_OnRookStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void rookAction_OnRookStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void bishopAction_OnBishopStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void bishopAction_OnBishopStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void queenAction_OnQueenStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void queenAction_OnQueenStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void knightAction_OnKnightStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void knightAction_OnKnightStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void KingAction_OnKingStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void KingAction_OnKingStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void ChainAttackAction_OnChainAttackStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void ChainAttackAction_OnChainAttackStopMoving(object sender, EventArgs e)
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
            enemymoveAction.OnStartMoving -= EnemyMoveAction_OnStartMoving;
            enemymoveAction.OnStopMoving -= EnemyMoveAction_OnStopMoving;
        }

        if (TryGetComponent<ReadyAction>(out ReadyAction shootAction))
        {
            shootAction.OnShoot -= shootAction_OnShoot;
        }

        if (TryGetComponent<KingAction>(out KingAction kingAction))
        {
            kingAction.OnKingSwordSlash -= KingAction_OnKingSwordSlash;
            kingAction.OnKingStartMoving -= KingAction_OnKingStartMoving;
            kingAction.OnKingStopMoving -= KingAction_OnKingStopMoving;
        }

        if (TryGetComponent<RookAction>(out RookAction rookAction))
        {
            rookAction.OnRookStartMoving -= rookAction_OnRookStartMoving;
            rookAction.OnRookStopMoving -= rookAction_OnRookStopMoving;
            rookAction.OnRookSwordSlash -= rookAction_OnRookSwordSlash;
        }

        if (TryGetComponent<BishopAction>(out BishopAction bishopAction))
        {
            bishopAction.OnBishopStartMoving -= bishopAction_OnBishopStartMoving;
            bishopAction.OnBishopStopMoving -= bishopAction_OnBishopStopMoving;
            bishopAction.OnBishopSwordSlash -= bishopAction_OnBishopSwordSlash;
        }

        if (TryGetComponent<QueenAction>(out QueenAction queenAction))
        {
            queenAction.OnQueenStartMoving -= queenAction_OnQueenStartMoving;
            queenAction.OnQueenStopMoving -= queenAction_OnQueenStopMoving;
            queenAction.OnQueenSwordSlash -= queenAction_OnQueenSwordSlash;

        }

        if (TryGetComponent<KnightAction>(out KnightAction knightAction))
        {
            knightAction.OnKnightStartMoving -= knightAction_OnKnightStartMoving;
            knightAction.OnKnightStopMoving -= knightAction_OnKnightStopMoving;
            knightAction.OnKnightSwordSlash -= knightAction_OnKnightSwordSlash;
        }

        if (TryGetComponent<ChainAttackAction>(out ChainAttackAction chainAttackAction))
        {
            chainAttackAction.OnChainAttackStartMoving -= ChainAttackAction_OnChainAttackStartMoving;
            chainAttackAction.OnChainAttackStopMoving -= ChainAttackAction_OnChainAttackStopMoving;
            chainAttackAction.OnChainAttackSwordSlash -= ChainAttackAction_OnChainAttackSwordSlash;
        }

        if (TryGetComponent<Unit>(out Unit unit))
        {
            unit.OnUnitDamage -= Unit_OnUnitDamage;
            unit.OnUnitDie -= Unit_OnUnitDie;
        }
    }
}
