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
            kingAction.OnKingActionStarted += KingAction_OnKingActionStarted;
            kingAction.OnKingActionCompleted += KingAction_OnKingActionCompleted;
        }

        if (TryGetComponent<RookAction>(out RookAction rookAction))
        {
            rookAction.OnRookStartMoving += rookAction_OnRookStartMoving;
            rookAction.OnRookStopMoving += rookAction_OnRookStopMoving;
            rookAction.OnRookActionStarted += rookAction_OnRookActionStarted;
            rookAction.OnRookActionCompleted += rookAction_OnRookActionCompleted;
        }

        if (TryGetComponent<BishopAction>(out BishopAction bishopAction))
        {
            bishopAction.OnBishopStartMoving += bishopAction_OnBishopStartMoving;
            bishopAction.OnBishopStopMoving += bishopAction_OnBishopStopMoving;
            bishopAction.OnBishopActionStarted += bishopAction_OnBishopActionStarted;
            bishopAction.OnBishopActionCompleted += bishopAction_OnBishopActionCompleted;
        }

        if (TryGetComponent<QueenAction>(out QueenAction queenAction))
        {
            queenAction.OnQueenStartMoving += queenAction_OnQueenStartMoving;
            queenAction.OnQueenStopMoving += queenAction_OnQueenStopMoving;
            queenAction.OnQueenActionStarted += queenAction_OnQueenActionStarted;
            queenAction.OnQueenActionCompleted += queenAction_OnQueenActionCompleted;
        }

        if (TryGetComponent<KnightAction>(out KnightAction knightAction))
        {
            knightAction.OnKnightStartMoving += knightAction_OnKnightStartMoving;
            knightAction.OnKnightStopMoving += knightAction_OnKnightStopMoving;
            knightAction.OnKnightActionStarted += knightAction_OnKnightActionStarted;
            knightAction.OnKnightActionCompleted += knightAction_OnKnightActionCompleted;
        }

        if(TryGetComponent<CharacterDataManager>(out CharacterDataManager characterdatamanager))
        {
            characterdatamanager.OnDead += Unit_OnDead;
            characterdatamanager.OnDamage += Unit_OnDamage;
        }

        if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterdatamanager))
        {
            monsterdatamanager.OnDead += Unit_OnDead;
            monsterdatamanager.OnDamage += Unit_OnDamage;
        }

    }

    #region Attack Action
    private void KingAction_OnKingActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void KingAction_OnKingActionCompleted(object sender, EventArgs e)
    {

    }
    private void rookAction_OnRookActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void rookAction_OnRookActionCompleted(object sender, EventArgs e)
    {
        
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

    private void bishopAction_OnBishopActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void bishopAction_OnBishopActionCompleted(object sender, EventArgs e)
    {
        
    }

    private void queenAction_OnQueenStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void queenAction_OnQueenStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void queenAction_OnQueenActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void queenAction_OnQueenActionCompleted(object sender, EventArgs e)
    {

    }
    private void knightAction_OnKnightStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void knightAction_OnKnightStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void knightAction_OnKnightActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void knightAction_OnKnightActionCompleted(object sender, EventArgs e)
    {

    }
    #endregion





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

    
    private void Unit_OnDead(object sender, EventArgs e)
    {
        animator.SetBool("IsDie", true);
    }

    private void Unit_OnDamage(object sender, EventArgs e)
    {
        animator.SetTrigger("IsDamage");
    }

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
            kingAction.OnKingActionStarted -= KingAction_OnKingActionStarted;
            kingAction.OnKingActionCompleted -= KingAction_OnKingActionCompleted;
        }

        if (TryGetComponent<RookAction>(out RookAction rookAction))
        {
            rookAction.OnRookStartMoving -= rookAction_OnRookStartMoving;
            rookAction.OnRookStopMoving -= rookAction_OnRookStopMoving;
            rookAction.OnRookActionStarted -= rookAction_OnRookActionStarted;
            rookAction.OnRookActionCompleted -= rookAction_OnRookActionCompleted;
        }

        if (TryGetComponent<BishopAction>(out BishopAction bishopAction))
        {
            bishopAction.OnBishopStartMoving -= bishopAction_OnBishopStartMoving;
            bishopAction.OnBishopStopMoving -= bishopAction_OnBishopStopMoving;
            bishopAction.OnBishopActionStarted -= bishopAction_OnBishopActionStarted;
            bishopAction.OnBishopActionCompleted -= bishopAction_OnBishopActionCompleted;
        }

        if (TryGetComponent<QueenAction>(out QueenAction queenAction))
        {
            queenAction.OnQueenStartMoving -= queenAction_OnQueenStartMoving;
            queenAction.OnQueenStopMoving -= queenAction_OnQueenStopMoving;
            queenAction.OnQueenActionStarted -= queenAction_OnQueenActionStarted;
            queenAction.OnQueenActionCompleted -= queenAction_OnQueenActionCompleted;
        }

        if (TryGetComponent<KnightAction>(out KnightAction knightAction))
        {
            knightAction.OnKnightStartMoving -= knightAction_OnKnightStartMoving;
            knightAction.OnKnightStopMoving -= knightAction_OnKnightStopMoving;
            knightAction.OnKnightActionStarted -= knightAction_OnKnightActionStarted;
            knightAction.OnKnightActionCompleted -= knightAction_OnKnightActionCompleted;
        }

        if (TryGetComponent<CharacterDataManager>(out CharacterDataManager characterdatamanager))
        {
            characterdatamanager.OnDead -= Unit_OnDead;
            characterdatamanager.OnDamage -= Unit_OnDamage;
        }

        if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterdatamanager))
        {
            monsterdatamanager.OnDead -= Unit_OnDead;
            monsterdatamanager.OnDamage -= Unit_OnDamage;
        }
    }
}
