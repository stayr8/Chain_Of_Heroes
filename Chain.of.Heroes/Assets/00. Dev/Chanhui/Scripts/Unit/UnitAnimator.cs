using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitAnimator : MonoBehaviour
{
    [Header("총알 위치")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform shootPointTransform;
    [SerializeField] private Transform VkeffectTransform;
    [SerializeField] private Transform ArSkill_1_shootPointTransform;
    [SerializeField] private Transform ArSkill_2_shootPointTransform;
    [SerializeField] private Transform VkSkill_2_shootPointTransform;

    [Header("총알들")]
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform ArSkill1_bulletProjectilePrefab;
    [SerializeField] private Transform ArSkill2_bulletProjectilePrefab;
    [SerializeField] private Transform WzSkill_2_bulletProjectilePrefab;
    [SerializeField] private Transform DragonbulletProjectilePrefab;
    [SerializeField] private Transform DGSkill_2_bulletProjectilePrefab;
    [SerializeField] private Transform SPSkill_1_bulletProjectilePrefab;

    private void Awake()
    {

        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
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

        if (TryGetComponent<KnightAttackAction>(out KnightAttackAction knightAttackAction))
        {
            knightAttackAction.OnKnightAttackSwordSlash += Action_OnSwordSlash;
            knightAttackAction.OnKnightAttackStartMoving += MoveAction_OnStartMoving;
            knightAttackAction.OnKnightAttackStopMoving += MoveAction_OnStopMoving;
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
            swordWomanSkill1Action.OnSWSkill_1_Slash += Action_OnSkill_1;
            swordWomanSkill1Action.OnSWSkill_1_Dash += Unit_OnUnitDash;
        }
        if (TryGetComponent<SwordWomanSkill2Action>(out SwordWomanSkill2Action swordWomanSkill2Action))
        {
            swordWomanSkill2Action.OnShoot += SwordWomanSkill2Action_OnShoot;
        }

        if (TryGetComponent<KnightSkill1Action>(out KnightSkill1Action knightSkill1Action))
        {
            knightSkill1Action.OnKnSkill_1_StartMoving += MoveAction_OnStartMoving;
            knightSkill1Action.OnKnSkill_1_StopMoving += MoveAction_OnStopMoving;
            knightSkill1Action.OnKnSkill_1_Stun += Action_OnSkill_1;
        }
        if (TryGetComponent<KnightSkill2Action>(out KnightSkill2Action knightSkill2Action))
        {
            knightSkill2Action.OnKnSkill_2_Buff += Action_OnSkill_2;
        }

        if (TryGetComponent<SamuraiSkill1Action>(out SamuraiSkill1Action samuraiSkill1Action))
        {
            samuraiSkill1Action.OnSrSkill_1_StartMoving += MoveAction_OnStartMoving;
            samuraiSkill1Action.OnSrSkill_1_StopMoving += MoveAction_OnStopMoving;
            samuraiSkill1Action.OnSrSkill_1_Slash += Action_OnSkill_1;
            samuraiSkill1Action.OnSrSkill_1_Dash += Unit_OnUnitDash;
        }
        if (TryGetComponent<SamuraiSkill2Action>(out SamuraiSkill2Action samuraiSkill2Action))
        {
            samuraiSkill2Action.OnSrSkill_2_StartMoving += MoveAction_OnStartMoving;
            samuraiSkill2Action.OnSrSkill_2_StopMoving += MoveAction_OnStopMoving;
            samuraiSkill2Action.OnSrSkill_2_Slash += Action_OnSkill_2;
        }

        if (TryGetComponent<ArcherSkill1Action>(out ArcherSkill1Action archerSkill1Action))
        {
            archerSkill1Action.OnShoot += ArSkill1_1_Action_OnShoot;
        }
        if (TryGetComponent<ArcherSkill2Action>(out ArcherSkill2Action archerSkill2Action))
        {
            archerSkill2Action.OnShoot += ArSkill1_2_Action_OnShoot;
        }

        if (TryGetComponent<GuardianSkill1Action>(out GuardianSkill1Action guardianSkill1Action))
        {
            guardianSkill1Action.OnGdSkill_1_StartMoving += MoveAction_OnStartMoving;
            guardianSkill1Action.OnGdSkill_1_StopMoving += MoveAction_OnStopMoving;
            guardianSkill1Action.OnGdSkill_1_Slash += Action_OnSkill_1;
            guardianSkill1Action.OnGdSkill_1_Dash += Unit_OnUnitDash;
        }
        if (TryGetComponent<GuardianSkill2Action>(out GuardianSkill2Action guardianSkill2Action))
        {
            guardianSkill2Action.OnGdSkill_2_provoke += Action_OnSkill_2;
        }

        if (TryGetComponent<PriestSkill1Action>(out PriestSkill1Action priestSkill1Action))
        {
            priestSkill1Action.OnPsSkill_1_Hill += Action_OnSkill_1;
        }
        if (TryGetComponent<PriestSkill2Action>(out PriestSkill2Action priestSkill2Action))
        {
            priestSkill2Action.OnPsSkill_2_Hill += Action_OnSkill_2;
        }

        if (TryGetComponent<WizardSkill1Action>(out WizardSkill1Action wizardSkill1Action))
        {
            wizardSkill1Action.OnWzSkill_1_StartMoving += MoveAction_OnStartMoving;
            wizardSkill1Action.OnWzSkill_1_StopMoving += MoveAction_OnStopMoving;
            wizardSkill1Action.OnWzSkill_1_Debuff += Action_OnSkill_1;
        }
        if (TryGetComponent<WizardSkill2Action>(out WizardSkill2Action wizardSkill2Action))
        {
            wizardSkill2Action.OnShoot += WzSkill1_2_Action_OnShoot;
        }

        if (TryGetComponent<ValkyrieSkill1Action>(out ValkyrieSkill1Action valkyrieSkill1Action))
        {
            valkyrieSkill1Action.OnVkSkill_1_StartMoving += MoveAction_OnStartMoving;
            valkyrieSkill1Action.OnVkSkill_1_StopMoving += MoveAction_OnStopMoving;
            valkyrieSkill1Action.OnVkSkill_1_Slash += Action_OnSkill_1;
            valkyrieSkill1Action.OnVkSkill_1_Dash += Unit_OnUnitDash;
        }
        if (TryGetComponent<ValkyrieSkill2Action>(out ValkyrieSkill2Action valkyrieSkill2Action))
        {
            valkyrieSkill2Action.OnShoot += VkSkill1_2_Action_OnShoot;
        }

        if (TryGetComponent<GoblinLoadSkill1Action>(out GoblinLoadSkill1Action goblinLoadSkill1Action))
        {
            goblinLoadSkill1Action.OnGLSkill_1_StartMoving += MoveAction_OnStartMoving;
            goblinLoadSkill1Action.OnGLSkill_1_StopMoving += MoveAction_OnStopMoving;
            goblinLoadSkill1Action.OnGLSkill_1_Slash += Action_OnSkill_1;
        }
        if (TryGetComponent<SalamanderLoadSkill1Action>(out SalamanderLoadSkill1Action salamanderLoadSkill1Action))
        {
            salamanderLoadSkill1Action.OnSLSkill_1_StartMoving += MoveAction_OnStartMoving;
            salamanderLoadSkill1Action.OnSLSkill_1_StopMoving += MoveAction_OnStopMoving;
            salamanderLoadSkill1Action.OnSLSkill_1_Slash += Action_OnSkill_1;
        }
        if (TryGetComponent<RedStoneGolemSkill1Action>(out RedStoneGolemSkill1Action redStoneGolemSkill1Action))
        {
            redStoneGolemSkill1Action.OnRSGSkill_1_StartMoving += MoveAction_OnStartMoving;
            redStoneGolemSkill1Action.OnRSGSkill_1_StopMoving += MoveAction_OnStopMoving;
            redStoneGolemSkill1Action.OnRSGSkill_1_Slash += Action_OnSkill_1;
        }
        if (TryGetComponent<DragonReadyAction>(out DragonReadyAction dragonReadyAction))
        {
            dragonReadyAction.OnShoot += dragonshootAction_OnShoot;
        }
        if (TryGetComponent<DragonSkill1Action>(out DragonSkill1Action dragonSkill1Action))
        {
            dragonSkill1Action.OnDGSkill_1_Bress += Action_OnSkill_1;
        }
        if (TryGetComponent<DragonSkill2Action>(out DragonSkill2Action dragonSkill2Action))
        {
            dragonSkill2Action.OnShoot += DGSkill1_2_Action_OnShoot;
        }
        if (TryGetComponent<StripedCaveSpiderSkill1Action>(out StripedCaveSpiderSkill1Action stripedCaveSpiderSkill1Action))
        {
            stripedCaveSpiderSkill1Action.OnShoot += SPSkill_1_Action_OnShoot;
        }

        if (TryGetComponent<StunAction>(out StunAction stunAction))
        {
            stunAction.OnUnitStun_Start += stunAction_OnUnitStun_Start;
            stunAction.OnUnitStun_Stop += stunAction_OnUnitStun_Stop;
        }

       
    }

    #region Attack Action
    private void Action_OnSwordSlash(object sender, EventArgs e)
    {
        animator.SetTrigger("SwordSlash");
    }

    private void Action_OnSkill_1(object sender, EventArgs e)
    {
        animator.SetTrigger("IsSkill_1");
    }

    private void Action_OnSkill_2(object sender, EventArgs e)
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

    private void stunAction_OnUnitStun_Start(object sender, EventArgs e)
    {
        animator.SetBool("IsStun", true);
    }

    private void stunAction_OnUnitStun_Stop(object sender, EventArgs e)
    {
        animator.SetBool("IsStun", false);
    }

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
    private void dragonshootAction_OnShoot(object sender, DragonReadyAction.OnShootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        StartCoroutine(Dragonbullet(e.targetUnit.GetWorldPosition()));
    }

    IEnumerator Dragonbullet(Vector3 e)
    {
        yield return new WaitForSeconds(0.8f);
        Transform bulletProjectileTransform =
            Instantiate(DragonbulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = e.y;

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
        float random = UnityEngine.Random.Range(-45f, 45f);
        bulletProjectile.transform.rotation = Quaternion.Euler(random, 0f, 0);

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform2 =
                Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        RangeBulletProjectile bulletProjectile2 = bulletProjectileTransform2.GetComponent<RangeBulletProjectile>();
        float random2 = UnityEngine.Random.Range(-45f, 45f);
        bulletProjectile2.transform.rotation = Quaternion.Euler(random2, 0f, 0);

        Vector3 targetUnitShootAtPosition2 = e;

        targetUnitShootAtPosition2.y = shootPointTransform.position.y;

        bulletProjectile2.Setup(targetUnitShootAtPosition2);

        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform3 =
                Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        RangeBulletProjectile bulletProjectile3 = bulletProjectileTransform3.GetComponent<RangeBulletProjectile>();
        float random3 = UnityEngine.Random.Range(-45f, 45f);
        bulletProjectile3.transform.rotation = Quaternion.Euler(random3, 0f, 0);

        Vector3 targetUnitShootAtPosition3 = e;

        targetUnitShootAtPosition3.y = shootPointTransform.position.y;

        bulletProjectile3.Setup(targetUnitShootAtPosition3);

        yield return new WaitForSeconds(0.6f);
        Transform bulletProjectileTransform4 =
                Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        RangeBulletProjectile bulletProjectile4 = bulletProjectileTransform4.GetComponent<RangeBulletProjectile>();
        float random4 = UnityEngine.Random.Range(-45f, 45f);
        bulletProjectile4.transform.rotation = Quaternion.Euler(random4, 0f, 0);
        Vector3 targetUnitShootAtPosition4 = e;

        targetUnitShootAtPosition4.y = shootPointTransform.position.y;

        bulletProjectile4.Setup(targetUnitShootAtPosition4);
         
    }
    private void ArSkill1_1_Action_OnShoot(object sender, ArcherSkill1Action.OnShootEventArgs e)
    {
        animator.SetTrigger("IsSkill_1");

        StartCoroutine(ArSkill_1_Shoot(e.targetUnit.GetWorldPosition()));
    }
    IEnumerator ArSkill_1_Shoot(Vector3 e)
    {
        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform =
                Instantiate(ArSkill1_bulletProjectilePrefab, ArSkill_1_shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = ArSkill_1_shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform2 =
                Instantiate(ArSkill1_bulletProjectilePrefab, ArSkill_1_shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile2 = bulletProjectileTransform2.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition2 = e;

        targetUnitShootAtPosition2.y = ArSkill_1_shootPointTransform.position.y;

        bulletProjectile2.Setup(targetUnitShootAtPosition2);

        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform3 =
                Instantiate(ArSkill1_bulletProjectilePrefab, ArSkill_1_shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile3 = bulletProjectileTransform3.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition3 = e;

        targetUnitShootAtPosition3.y = ArSkill_1_shootPointTransform.position.y;

        bulletProjectile3.Setup(targetUnitShootAtPosition3);

        yield return new WaitForSeconds(0.2f);
        Transform bulletProjectileTransform4 =
                Instantiate(ArSkill1_bulletProjectilePrefab, ArSkill_1_shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile4 = bulletProjectileTransform4.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition4 = e;

        targetUnitShootAtPosition4.y = ArSkill_1_shootPointTransform.position.y;

        bulletProjectile4.Setup(targetUnitShootAtPosition4);

    }
    private void ArSkill1_2_Action_OnShoot(object sender, ArcherSkill2Action.OnShootEventArgs e)
    {
        animator.SetTrigger("IsSkill_2");

        StartCoroutine(ArSkill_2_Shoot(e.targetUnit.GetWorldPosition()));

    }
    IEnumerator ArSkill_2_Shoot(Vector3 e)
    {
        yield return new WaitForSeconds(0.4f);

        Transform bulletProjectileTransform =
            Instantiate(ArSkill2_bulletProjectilePrefab, ArSkill_2_shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = ArSkill_2_shootPointTransform.position.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void WzSkill1_2_Action_OnShoot(object sender, WizardSkill2Action.OnShootEventArgs e)
    {
        animator.SetTrigger("IsSkill_2");
        StartCoroutine(WzSkill_2_Shoot(e.targetUnit.GetWorldPosition()));
        
    }

    IEnumerator WzSkill_2_Shoot(Vector3 e)
    {
        yield return new WaitForSeconds(0.2f);

        Vector3 MousePosition = e + new Vector3(0f,20f,0f);
        Transform bulletProjectileTransform =
            Instantiate(WzSkill_2_bulletProjectilePrefab, MousePosition, Quaternion.identity);
        MetaoBulletProjjectile bulletProjectile = bulletProjectileTransform.GetComponent<MetaoBulletProjjectile>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = e.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void VkSkill1_2_Action_OnShoot(object sender, ValkyrieSkill2Action.OnShootEventArgs e)
    {
        animator.SetTrigger("IsSkill_2");

        StartCoroutine(VkSkill_2_Shoot(e.targetUnit.GetWorldPosition()));
    }
    IEnumerator VkSkill_2_Shoot(Vector3 e)
    {
        yield return new WaitForSeconds(1f);

        Vector3 MousePosition = e;
        Transform bulletProjectileTransform =
            Instantiate(VkeffectTransform, MousePosition, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = MousePosition.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void DGSkill1_2_Action_OnShoot(object sender, DragonSkill2Action.OnShootEventArgs e)
    {
        animator.SetTrigger("IsSkill_2");

        StartCoroutine(DGSkill_2_Shoot(e.targetUnit.GetWorldPosition()));
    }

    IEnumerator DGSkill_2_Shoot(Vector3 e)
    {
        yield return new WaitForSeconds(0.8f);

        Vector3 MousePosition = e + new Vector3(0f, 20f, 0f);
        Transform bulletProjectileTransform =
            Instantiate(DGSkill_2_bulletProjectilePrefab, MousePosition, Quaternion.identity);
        DragonMetao bulletProjectile = bulletProjectileTransform.GetComponent<DragonMetao>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = e.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void SPSkill_1_Action_OnShoot(object sender, StripedCaveSpiderSkill1Action.OnShootEventArgs e)
    {
        animator.SetTrigger("IsSkill_1");

        StartCoroutine(SPSkill_1_Shoot(e.targetUnit.GetWorldPosition()));
    }

    IEnumerator SPSkill_1_Shoot(Vector3 e)
    {
        yield return new WaitForSeconds(0.5f);
        Transform bulletProjectileTransform =
            Instantiate(SPSkill_1_bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        SpiderDebuffBulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<SpiderDebuffBulletProjectile>();

        Vector3 targetUnitShootAtPosition = e;

        targetUnitShootAtPosition.y = e.y;

        bulletProjectile.Setup(targetUnitShootAtPosition);
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
