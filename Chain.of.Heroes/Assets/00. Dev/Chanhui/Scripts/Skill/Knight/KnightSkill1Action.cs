using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightSkill1Action : BaseAction
{
    public event EventHandler OnKnSkill_1_StartMoving;
    public event EventHandler OnKnSkill_1_StopMoving;
    public event EventHandler OnKnSkill_1_Stun;

    [SerializeField] private Transform skill1_effect;
    [SerializeField] private Transform skill1_effect_transform;

    private List<Vector3> positionList;
    private int currentPositionIndex;

    private enum State
    {
        SwingingKnSkill_1_BeforeMoving,
        SwingingKnSkill_1_Moving,
        SwingingKnSkill_1_Attacking,
        SwingingKnSkill_1_Effect,
        SwingingKnSkill_1_AfterHit,
    }

    [SerializeField] private int maxKnSkill_1_Distance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;

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

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;


        if (state == State.SwingingKnSkill_1_BeforeMoving)
        {
            float rotateSpeed_1 = 30f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed_1);

            float stoppingDistance = 0.1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                float moveSpeed = 4f;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                float BeforepositionList = positionList.Count - 2;
                if (currentPositionIndex >= BeforepositionList)
                {
                    OnKnSkill_1_StopMoving?.Invoke(this, EventArgs.Empty);
                    //UnitActionSystem.Instance.SetCameraPointchange(true);
                    currentPositionIndex++;
                    state = State.SwingingKnSkill_1_Moving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }


        switch (state)
        {
            case State.SwingingKnSkill_1_BeforeMoving:

                break;
            case State.SwingingKnSkill_1_Moving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingKnSkill_1_Attacking:

                break;
            case State.SwingingKnSkill_1_Effect:

                break;
            case State.SwingingKnSkill_1_AfterHit:

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
            case State.SwingingKnSkill_1_BeforeMoving:

                break;
            case State.SwingingKnSkill_1_Moving:

                TimeAttack(0.1f);
                state = State.SwingingKnSkill_1_Attacking;

                break;
            case State.SwingingKnSkill_1_Attacking:
                OnKnSkill_1_Stun?.Invoke(this, EventArgs.Empty);
                if (targetUnit.GetEnemyVisualType() != Unit.EnemyType.Dragon) 
                {
                    BaseAction StartAction = targetUnit.GetAction<StunAction>();
                    StartAction.TakeAction(targetUnit.GetGridPosition(), onActionComplete);
                }
                TimeAttack(0.6f);
                state = State.SwingingKnSkill_1_Effect;

                break;
            case State.SwingingKnSkill_1_Effect:
                AttackActionSystem.Instance.GetMonsterDataManager().Damage();
                Transform skill1EffectTransform = Instantiate(skill1_effect, skill1_effect_transform.position, Quaternion.identity);
                Destroy(skill1EffectTransform.gameObject, 0.2f);

                TimeAttack(0.4f);
                state = State.SwingingKnSkill_1_AfterHit;

                break;
            case State.SwingingKnSkill_1_AfterHit:

                ActionComplete();

                break;
        }
    }
    void TimeAttack(float StateTime)
    {
        float afterHitStateTime = StateTime;
        stateTimer = afterHitStateTime;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxKnSkill_1_Distance; x <= maxKnSkill_1_Distance; x++)
        {
            for (int z = -maxKnSkill_1_Distance; z <= maxKnSkill_1_Distance; z++)
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

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
                {
                    if (!LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).IsEnemy())
                    {
                        continue;
                    }
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                if (LevelGrid.Instance.GetEnemyAtSurroundPosition(testGridPosition))
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
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        if (isSkillCount <= 0)
        {
            isSkillCount = 2;
        }

        state = State.SwingingKnSkill_1_BeforeMoving;
        TimeAttack(0.7f);

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnKnSkill_1_StartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);
        AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());
        AttackActionSystem.Instance.SetMonsterDataManager(targetUnit.GetMonsterDataManager());

        ActionStart(onActionComplete);
    }

    public int GetMaxSWSkill_1_Distance()
    {
        return maxKnSkill_1_Distance;
    }

    public override string GetActionName()
    {
        return "신성강타";
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
