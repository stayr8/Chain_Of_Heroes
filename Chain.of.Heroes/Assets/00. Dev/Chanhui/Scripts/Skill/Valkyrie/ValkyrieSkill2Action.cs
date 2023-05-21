using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieSkill2Action : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        SwingingVkSkill_2_LookAt,
        SwingingVkSkill_2_Attack,
        SwingingVkSkill_2_AfterHit,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxVkSkill_2_Distance = 2;

    private GridPosition targetposition;

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
            case State.SwingingVkSkill_2_LookAt:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingVkSkill_2_Attack:

                break;
            case State.SwingingVkSkill_2_AfterHit:

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
            case State.SwingingVkSkill_2_LookAt:
                TimeAttack(0.1f);
                state = State.SwingingVkSkill_2_Attack;

                break;
            case State.SwingingVkSkill_2_Attack:
                if (canShootBullet)
                {
                    Shoot();
                    TargetSurroundStun();
                    canShootBullet = false;
                }

                TimeAttack(3.0f);
                state = State.SwingingVkSkill_2_AfterHit;

                break;
            case State.SwingingVkSkill_2_AfterHit:

                ActionComplete();

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

    int StunDistance = 1;
    private void TargetSurroundStun()
    {
        for (int x = -StunDistance; x <= StunDistance; x++)
        {
            for (int z = -StunDistance; z <= StunDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = targetposition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > StunDistance)
                {
                    continue;
                }

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (targetUnit.IsEnemy())
                    {
                        BaseAction StartAction = targetUnit.GetAction<StunAction>();
                        StartAction.TakeAction(targetUnit.GetGridPosition(), onActionComplete);
                    }
                }
            }
        }
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxVkSkill_2_Distance; x <= maxVkSkill_2_Distance; x++)
        {
            for (int z = -maxVkSkill_2_Distance; z <= maxVkSkill_2_Distance; z++)
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
                if ((testX != 0) && (testZ != 0) && (testX != testZ))
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

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                {
                    // Blocked by an Obstacle
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetposition = gridPosition;
        isSkill = true;
        if (isSkillCount <= 0)
        {
            isSkillCount = 3;
        }
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingVkSkill_2_LookAt;
        TimeAttack(0.7f);

        canShootBullet = true;

        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);
        AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());

        ActionStart(onActionComplete);
    }

    public int GetMaxVkSkill_2_Distance()
    {
        return maxVkSkill_2_Distance;
    }

    public override string GetActionName()
    {
        return "½ÉÆÇ";
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
