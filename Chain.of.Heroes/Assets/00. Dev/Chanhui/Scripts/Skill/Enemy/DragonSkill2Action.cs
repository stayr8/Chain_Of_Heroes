using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSkill2Action : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    [SerializeField] private Transform skill1_effect;
    [SerializeField] private Transform skill1_effect_transform;



    private int actionCoolTime;
    private enum State
    {
        SwingingRSGSkill_2_LookAt,
        SwingingRSGSkill_2_Attacking,
        SwingingRSGSkill_2_AfterHit,
    }

    [SerializeField] private int maxDGSkill_2_Distance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private List<Binding> Binds = new List<Binding>();


    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (TurnSystem.Property.IsPlayerTurn)
            {
                if (isSkill && isSkillCount > 0)
                {
                    isSkillCount -= 1;
                }

                if (isSkillCount <= 0)
                {
                    isSkill = false;
                    actionCoolTime = 250;
                }
            }
        });
        Binds.Add(Bind);

        isSkillCount = 0;
        actionCoolTime = 250;
    }

    private void Update()
    {

        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingRSGSkill_2_LookAt:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 40f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingRSGSkill_2_Attacking:

                break;
            case State.SwingingRSGSkill_2_AfterHit:

                break;
        }

        if (stateTimer <= 0f)
        {
            NextState();
        }
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingingRSGSkill_2_LookAt:
                state = State.SwingingRSGSkill_2_Attacking;
                TimeAttack(0.5f);

                break;
            case State.SwingingRSGSkill_2_Attacking:

                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                Debug.Log(targetUnit);
                Transform skill1EffectTransform = Instantiate(skill1_effect, skill1_effect_transform.position, Quaternion.identity);
                skill1EffectTransform.transform.parent = skill1_effect_transform;
                Destroy(skill1EffectTransform.gameObject, 2f);

                TimeAttack(4.0f);
                state = State.SwingingRSGSkill_2_AfterHit;

                break;
            case State.SwingingRSGSkill_2_AfterHit:

                ActionComplete();
                unit.GetMonsterDataManager().m_skilldamagecoefficient = 0f;
                break;
        }
    }
    void TimeAttack(float StateTime)
    {
        float afterHitStateTime = StateTime;
        stateTimer = afterHitStateTime;
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxDGSkill_2_Distance; x <= maxDGSkill_2_Distance; x++)
        {
            for (int z = -maxDGSkill_2_Distance; z <= maxDGSkill_2_Distance; z++)
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

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both Units on same 'team'
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        isSkill = true;
        actionCoolTime = 0;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        unit.GetMonsterDataManager().m_skilldamagecoefficient = 1.5f;

        if (isSkillCount <= 0)
        {
            isSkillCount = 3;
        }

        state = State.SwingingRSGSkill_2_LookAt;
        TimeAttack(0.7f);

        canShootBullet = true;

        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);
        AttackActionSystem.Instance.SetMonsterDataManager(unit.GetMonsterDataManager());

        ActionStart(onActionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = actionCoolTime,
        };
    }

    public int GetMaxDGSkill_2_Distance()
    {
        return maxDGSkill_2_Distance;
    }

    public override string GetActionName()
    {
        return "È­¿°±¸";
    }

    public override string GetSingleActionPoint()
    {
        return "4";
    }

    public override int GetActionPointsCost()
    {
        return 4;
    }

    public override int GetSkillCountPoint()
    {
        return isSkillCount;
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }
}
