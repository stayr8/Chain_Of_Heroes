using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStoneGolemSkill1Action : BaseAction
{
    public event EventHandler OnRSGSkill_1_StartMoving;
    public event EventHandler OnRSGSkill_1_StopMoving;
    public event EventHandler OnRSGSkill_1_Slash;


    [SerializeField] private Transform skill1_effect;

    private List<Vector3> positionList;
    private int actionCoolTime;
    private enum State
    {
        SwingingRSGSkill_1_BeforeMoving,
        SwingingRSGSkill_1_Moving,
        SwingingRSGSkill_1_Attacking,
        SwingingRSGSkill_1_AfterHit,
    }

    [SerializeField] private int maxRSGSkill_1_Distance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;

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
            case State.SwingingRSGSkill_1_BeforeMoving:

                break;
            case State.SwingingRSGSkill_1_Moving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 40f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingRSGSkill_1_Attacking:

                break;        
            case State.SwingingRSGSkill_1_AfterHit:

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
            case State.SwingingRSGSkill_1_BeforeMoving:
                state = State.SwingingRSGSkill_1_Moving;

                break;
            case State.SwingingRSGSkill_1_Moving:
                
                TimeAttack(0.5f);
                state = State.SwingingRSGSkill_1_Attacking;

                break;
            case State.SwingingRSGSkill_1_Attacking:
                OnRSGSkill_1_Slash?.Invoke(this, EventArgs.Empty);
                
                Invoke("Effect", 0.4f);

                TimeAttack(2.0f);
                state = State.SwingingRSGSkill_1_AfterHit;

                break;
            case State.SwingingRSGSkill_1_AfterHit:

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

    void Effect()
    {
        GetPlayerStunGridPositionList();
        Transform skill1EffectTransform = Instantiate(skill1_effect, targetUnit.transform.position, Quaternion.identity);
        Destroy(skill1EffectTransform.gameObject, 0.4f);
    }

    int stunDistance = 1;
    public void GetPlayerStunGridPositionList()
    {
        GridPosition unitGridPosition = targetUnit.GetGridPosition();

        for (int x = -stunDistance; x <= stunDistance; x++)
        {
            for (int z = -stunDistance; z <= stunDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    Unit targetPlayer = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (!targetPlayer.IsEnemy())
                    {
                        Debug.Log(targetPlayer);
                        BaseAction StartAction = targetPlayer.GetAction<StunAction>();
                        StartAction.TakeAction(targetPlayer.GetGridPosition(), onActionComplete);
                        StartAction.SetIsSkillCount(3);
                        targetPlayer.SetIsStun(true);
                        targetPlayer.GetCharacterDataManager().SkillDamage();
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

        for (int x = -maxRSGSkill_1_Distance; x <= maxRSGSkill_1_Distance; x++)
        {
            for (int z = -maxRSGSkill_1_Distance; z <= maxRSGSkill_1_Distance; z++)
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
                        if (Prunit.GetUnitName() == "ÇÃ¶óÆ¾")
                        {
                            Debug.Log("°¡µð¾ð");
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

                if (!Pathfinding.Instance.HasAtPath(unitGridPosition, testGridPosition))
                {
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
        unit.GetMonsterDataManager().m_skilldamagecoefficient = 2.0f;

        if (isSkillCount <= 0)
        {
            isSkillCount = 3;
        }

        state = State.SwingingRSGSkill_1_BeforeMoving;
        TimeAttack(0.7f);

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        //OnRSGSkill_1_StartMoving?.Invoke(this, EventArgs.Empty);
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

    public int GetMaxGLSkill_1_Distance()
    {
        return maxRSGSkill_1_Distance;
    }

    public override string GetActionName()
    {
        return "´ëÁöºØ±«";
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
