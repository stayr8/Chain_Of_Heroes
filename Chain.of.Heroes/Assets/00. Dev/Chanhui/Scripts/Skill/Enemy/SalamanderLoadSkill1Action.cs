using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalamanderLoadSkill1Action : BaseAction
{
    public event EventHandler OnSLSkill_1_StartMoving;
    public event EventHandler OnSLSkill_1_StopMoving;
    public event EventHandler OnSLSkill_1_Slash;


    [SerializeField] private Transform skill1_effect;
    [SerializeField] private Transform skill1_effect_transform;

    private List<Vector3> positionList;
    private int currentPositionIndex;
    private int actionCoolTime;
    private enum State
    {
        SwingingSLSkill_1_BeforeMoving,
        SwingingSLSkill_1_Moving,
        SwingingSLSkill_1_BeforeCamera,
        SwingingSLSkill_1_AttackStand,
        SwingingSLSkill_1_AfterMoving,
        SwingingSLSkill_1_AttackMoving,
        SwingingSLSkill_1_BeforeHit,
        SwingingSLSkill_1_AfterCamera,
        SwingingSLSkill_1_AfterHit,
    }

    [SerializeField] private int maxSLSkill_1_Distance = 2;

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

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        if (state == State.SwingingSLSkill_1_BeforeMoving)
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
                    OnSLSkill_1_StopMoving?.Invoke(this, EventArgs.Empty);
                    UnitActionSystem.Instance.SetCameraPointchange(true);
                    currentPositionIndex++;
                    state = State.SwingingSLSkill_1_Moving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }


        switch (state)
        {
            case State.SwingingSLSkill_1_BeforeMoving:

                break;
            case State.SwingingSLSkill_1_Moving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingSLSkill_1_BeforeCamera:

                break;
            case State.SwingingSLSkill_1_AttackStand:

                break;
            case State.SwingingSLSkill_1_AfterMoving:

                break;
            case State.SwingingSLSkill_1_AttackMoving:
                Vector3 targetDirection2 = targetUnit.transform.position;
                Vector3 aimDir2 = (targetDirection2 - transform.position).normalized;
                float rotateSpeed2 = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir2, Time.deltaTime * rotateSpeed2);

                float stoppingDistance1 = 1.5f;
                if (Vector3.Distance(transform.position, targetDirection2) > stoppingDistance1)
                {
                    float moveSpeed = 15f;
                    transform.position += aimDir2 * moveSpeed * Time.deltaTime;
                }
                else
                {
                    OnSLSkill_1_StopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingSLSkill_1_BeforeHit;
                }

                break;
            case State.SwingingSLSkill_1_BeforeHit:

                break;
            case State.SwingingSLSkill_1_AfterCamera:

                break;
            case State.SwingingSLSkill_1_AfterHit:

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
            case State.SwingingSLSkill_1_BeforeMoving:

                break;
            case State.SwingingSLSkill_1_Moving:
                AttackCameraStart();
                UnitActionSystem.Instance.SetCameraPointchange(true);
                TimeAttack(0.5f);
                state = State.SwingingSLSkill_1_BeforeCamera;

                break;
            case State.SwingingSLSkill_1_BeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();
                TimeAttack(0.1f);
                state = State.SwingingSLSkill_1_AttackStand;

                break;
            case State.SwingingSLSkill_1_AttackStand:
                AttackActionSystem.Instance.OnAtLocationMove(targetUnit, unit);
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingSLSkill_1_AfterMoving;

                break;
            case State.SwingingSLSkill_1_AfterMoving:
                AttackCameraComplete();
                OnSLSkill_1_StartMoving?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingSLSkill_1_AttackMoving;

                break;
            case State.SwingingSLSkill_1_AttackMoving:

                break;
            case State.SwingingSLSkill_1_BeforeHit:
                OnSLSkill_1_Slash?.Invoke(this, EventArgs.Empty);
                Invoke("Effect", 0.5f);

                TimeAttack(1.0f);
                state = State.SwingingSLSkill_1_AfterCamera;

                break;
            case State.SwingingSLSkill_1_AfterCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingSLSkill_1_AfterHit;

                break;
            case State.SwingingSLSkill_1_AfterHit:

                ActionCameraComplete();
                AttackActionSystem.Instance.OffAtLocationMove(targetUnit, unit);

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
        BaseAction StartAction = targetUnit.GetAction<StunAction>();
        StartAction.TakeAction(targetUnit.GetGridPosition(), onActionComplete);

        Transform skill1EffectTransform = Instantiate(skill1_effect, skill1_effect_transform.position, Quaternion.identity);
        Destroy(skill1EffectTransform.gameObject, 0.4f);
        targetUnit.GetCharacterDataManager().SkillDamage();
        targetUnit.SetIsStun(true);
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxSLSkill_1_Distance; x <= maxSLSkill_1_Distance; x++)
        {
            for (int z = -maxSLSkill_1_Distance; z <= maxSLSkill_1_Distance; z++)
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
                        if (Prunit.GetUnitName() == "�ö�ƾ")
                        {
                            Debug.Log("�����");
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
        unit.GetMonsterDataManager().m_skilldamagecoefficient = 1.5f;

        if (isSkillCount <= 0)
        {
            isSkillCount = 3;
        }

        state = State.SwingingSLSkill_1_BeforeMoving;
        TimeAttack(0.7f);

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnSLSkill_1_StartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

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
        return maxSLSkill_1_Distance;
    }

    public override string GetActionName()
    {
        return "��ö����";
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
