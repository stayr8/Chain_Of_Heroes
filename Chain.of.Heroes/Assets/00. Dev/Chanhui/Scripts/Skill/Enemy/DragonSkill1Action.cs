using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonSkill1Action : BaseAction
{
    public event EventHandler OnDGSkill_1_Bress;


    [SerializeField] private Transform skill1_effect;
    [SerializeField] private Transform skill1_effect_transform;

    private List<Vector3> positionList;
    private int actionCoolTime;
    private enum State
    {
        SwingingRSGSkill_1_BeforeMoving,
        SwingingRSGSkill_1_Moving,
        SwingingRSGSkill_1_Attacking,
        SwingingRSGSkill_1_AfterHit,
    }

    [SerializeField] private int maxDGSkill_1_Distance = 2;

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
                float rotateSpeed = 50f;
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

                TimeAttack(1f);
                state = State.SwingingRSGSkill_1_Attacking;

                break;
            case State.SwingingRSGSkill_1_Attacking:

                OnDGSkill_1_Bress?.Invoke(this, EventArgs.Empty);
                Debug.Log(targetUnit);
                StartCoroutine(Effect());

                TimeAttack(4.0f);
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

    IEnumerator Effect()
    {
        yield return new WaitForSeconds(1f);
        Transform skill1EffectTransform = Instantiate(skill1_effect, skill1_effect_transform.position, Quaternion.identity);
        skill1EffectTransform.transform.parent = skill1_effect_transform;
        skill1EffectTransform.transform.rotation = Quaternion.Euler(160f, 0f, 0f);
        Destroy(skill1EffectTransform.gameObject, 1f);
        yield return new WaitForSeconds(0.5f);
        GetPlayerStunGridPositionList(); 
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

        for (int x = -maxDGSkill_1_Distance; x <= maxDGSkill_1_Distance; x++)
        {
            for (int z = -maxDGSkill_1_Distance; z <= maxDGSkill_1_Distance; z++)
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
        unit.GetMonsterDataManager().m_skilldamagecoefficient = 1.0f;

        if (isSkillCount <= 0)
        {
            isSkillCount = 2;
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

    public int GetMaxDGSkill_1_Distance()
    {
        return maxDGSkill_1_Distance;
    }

    public override string GetActionName()
    {
        return "브레스";
    }

    public override string GetSingleActionPoint()
    {
        return "2";
    }

    public override int GetActionPointsCost()
    {
        return 2;
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
