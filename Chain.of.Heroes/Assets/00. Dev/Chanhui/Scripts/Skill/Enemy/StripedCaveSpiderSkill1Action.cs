using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripedCaveSpiderSkill1Action : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private List<Vector3> positionList;
    private int actionCoolTime;

    private enum State
    {
        SwingingSpider_LookAt,
        SwingingSpider_SkillAttack,
        SwingingSpider_AfterHit,
    }

    [SerializeField] private int maxSpiderDistance = 1;

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
                    actionCoolTime = 300;
                }
            }
        });
        Binds.Add(Bind);

        isSkillCount = 0;
        actionCoolTime = 300;
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
            case State.SwingingSpider_LookAt:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 40f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingSpider_SkillAttack:


                break;
            case State.SwingingSpider_AfterHit:

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
            case State.SwingingSpider_LookAt:
                TimeAttack(0.5f);
                state = State.SwingingSpider_SkillAttack;

                break;

            case State.SwingingSpider_SkillAttack:

                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                
                TimeAttack(4.0f);
                state = State.SwingingSpider_AfterHit;

                break;

            case State.SwingingSpider_AfterHit:

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

        for (int x = -maxSpiderDistance; x <= maxSpiderDistance; x++)
        {
            for (int z = -maxSpiderDistance; z <= maxSpiderDistance; z++)
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

                if (isProvoke)
                {
                    if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                    {
                        Unit Prunit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                        if (Prunit.GetUnitName() == "플라틴")
                        {
                            Debug.Log("가디언");
                        }
                        else
                        {
                            Debug.Log(Prunit);
                            continue;
                        }
                    }
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
        unit.GetMonsterDataManager().m_skilldamagecoefficient = 1.2f;

        if (isSkillCount <= 0)
        {
            isSkillCount = 3;
        }

        state = State.SwingingSpider_LookAt;
        TimeAttack(0.7f);
        canShootBullet = true;

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }


        AttackActionSystem.Instance.SetUnitChainFind(unit, targetUnit);
        AttackActionSystem.Instance.SetMonsterDataManager(unit.GetMonsterDataManager());

        ActionStart(onActionComplete);
    }

    public int GetMaxSpiderDistance()
    {
        return maxSpiderDistance;
    }



    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = actionCoolTime,
        };
    }


    public override int GetActionPointsCost()
    {
        return 3;
    }
    public override string GetActionName()
    {
        return "독극물";
    }

    public override string GetSingleActionPoint()
    {
        return "3";
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
