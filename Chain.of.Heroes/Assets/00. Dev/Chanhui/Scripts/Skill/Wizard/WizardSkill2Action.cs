using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardSkill2Action : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;


    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        SwingingWzSkill_2_LookAt,
        SwingingWzSkill_2_Attack,
        SwingingWzSkill_2_AfterHit,
    }

    [SerializeField] private int maxWzSkill_2_Distance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private List<Binding> Binds = new List<Binding>();

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }


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
                }

            }
        });
        Binds.Add(Bind);

        isSkillCount = 0;
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
            case State.SwingingWzSkill_2_LookAt:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingWzSkill_2_Attack:

                break;
            case State.SwingingWzSkill_2_AfterHit:

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
            case State.SwingingWzSkill_2_LookAt:
                TimeAttack(0.1f);
                state = State.SwingingWzSkill_2_Attack;

                break;
            case State.SwingingWzSkill_2_Attack:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }

                TimeAttack(3.0f);
                state = State.SwingingWzSkill_2_AfterHit;

                break;
            case State.SwingingWzSkill_2_AfterHit:

                ActionComplete();
                unit.GetCharacterDataManager().m_skilldamagecoefficient = 0f;
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

        for (int x = -maxWzSkill_2_Distance; x <= maxWzSkill_2_Distance; x++)
        {
            for (int z = -maxWzSkill_2_Distance; z <= maxWzSkill_2_Distance; z++)
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

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if (testX != testZ)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
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
        if (isSkillCount <= 0)
        {
            isSkillCount = 4;
        }
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        unit.GetCharacterDataManager().m_skilldamagecoefficient = 3.0f;

        state = State.SwingingWzSkill_2_LookAt;
        TimeAttack(0.7f);

        canShootBullet = true;

        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);
        AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());

        ActionStart(onActionComplete);
    }

    public int GetMaxWzSkill_2_Distance()
    {
        return maxWzSkill_2_Distance;
    }

    public override string GetActionName()
    {
        return "╦чев©ю";
    }

    public override string GetSingleActionPoint()
    {
        return "3";
    }

    public override int GetActionPointsCost()
    {
        return 3;
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
