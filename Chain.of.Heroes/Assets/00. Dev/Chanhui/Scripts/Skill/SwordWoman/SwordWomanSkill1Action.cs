using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWomanSkill1Action : BaseAction
{
    public event EventHandler OnSWSkill_1_StartMoving;
    public event EventHandler OnSWSkill_1_StopMoving;
    public event EventHandler OnSWSkill_1_Slash;
    public event EventHandler OnSWSkill_1_Dash;

    [SerializeField] private Transform skill1_effect;
    [SerializeField] private Transform skill1_effect_transform;


    private List<Vector3> positionList;
    private int currentPositionIndex;

    private enum State
    {
        SwingingSWSkill_1_BeforeMoving,
        SwingingSWSkill_1_Moving,
        SwingingSWSkill_1_BeforeCamera,
        SwingingSWSkill_1_AttackStand,
        SwingingSWSkill_1_AfterMoving,
        SwingingSWSkill_1_AttackMoving,
        SwingingSWSkill_1_BeforeHit,
        SwingingSWSkill_1_AfterCamera,
        SwingingSWSkill_1_AfterHit,
    }

    [SerializeField] private int maxSWSkill_1_Distance = 2;

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

                if(isSkillCount <= 0)
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


        if (state == State.SwingingSWSkill_1_BeforeMoving)
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
                    OnSWSkill_1_StopMoving?.Invoke(this, EventArgs.Empty);
                    UnitActionSystem.Instance.SetCameraPointchange(true);
                    currentPositionIndex++;
                    state = State.SwingingSWSkill_1_Moving;
                }
                else
                {
                    currentPositionIndex++;
                }
            }
        }


        switch (state)
        {
            case State.SwingingSWSkill_1_BeforeMoving:

                break;
            case State.SwingingSWSkill_1_Moving:
                Vector3 targetDirection = targetUnit.transform.position;
                Vector3 aimDir = (targetDirection - transform.position).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingSWSkill_1_BeforeCamera:

                break;
            case State.SwingingSWSkill_1_AttackStand:

                break;
            case State.SwingingSWSkill_1_AfterMoving:

                break;
            case State.SwingingSWSkill_1_AttackMoving:
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
                    OnSWSkill_1_StopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingSWSkill_1_BeforeHit;
                }

                break;
            case State.SwingingSWSkill_1_BeforeHit:

                break;
            case State.SwingingSWSkill_1_AfterCamera:

                break;
            case State.SwingingSWSkill_1_AfterHit:

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
            case State.SwingingSWSkill_1_BeforeMoving:

                break;
            case State.SwingingSWSkill_1_Moving:
                AttackCameraStart();

                TimeAttack(0.5f);
                state = State.SwingingSWSkill_1_BeforeCamera;

                break;
            case State.SwingingSWSkill_1_BeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingSWSkill_1_AttackStand;

                break;
            case State.SwingingSWSkill_1_AttackStand:
                AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingSWSkill_1_AfterMoving;

                break;
            case State.SwingingSWSkill_1_AfterMoving:
                AttackCameraComplete();
                OnSWSkill_1_Dash?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingSWSkill_1_AttackMoving;

                break;
            case State.SwingingSWSkill_1_AttackMoving:

                break;
            case State.SwingingSWSkill_1_BeforeHit:
                OnSWSkill_1_Slash?.Invoke(this, EventArgs.Empty);
                SoundManager.instance.SwordWoman_1();
                Transform skill1EffectTransform = Instantiate(skill1_effect, skill1_effect_transform.position, Quaternion.identity);
                skill1EffectTransform.transform.rotation = Quaternion.Euler(-30f, -75f, 0);
                Destroy(skill1EffectTransform.gameObject, 0.2f);

                TimeAttack(1.0f);
                state = State.SwingingSWSkill_1_AfterCamera;

                break;
            case State.SwingingSWSkill_1_AfterCamera:
                ScreenManager._instance._LoadScreenTextuer();
                TimeAttack(0.1f);
                state = State.SwingingSWSkill_1_AfterHit;

                break;
            case State.SwingingSWSkill_1_AfterHit:
                ActionCameraComplete();
                AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);

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

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxSWSkill_1_Distance; x <= maxSWSkill_1_Distance; x++)
        {
            for (int z = -maxSWSkill_1_Distance; z <= maxSWSkill_1_Distance; z++)
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
                if ((testX != 0) && (testZ != 0))
                {
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

                if (!Pathfinding.Instance.HasAtPath(unitGridPosition, testGridPosition))
                {
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
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        unit.GetCharacterDataManager().m_skilldamagecoefficient = 1.5f;

        if (isSkillCount <= 0)
        {
            isSkillCount = 2;
        }

        state = State.SwingingSWSkill_1_BeforeMoving;
        TimeAttack(0.7f);

        List<GridPosition> pathgridPositionList = Pathfinding.Instance.AttackFindPath(unit.GetGridPosition(), gridPosition, out int pathLength);

        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        for (int i = 0; i < pathgridPositionList.Count; i++)
        {
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathgridPositionList[i]));
        }

        OnSWSkill_1_StartMoving?.Invoke(this, EventArgs.Empty);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxSWSkill_1_Distance()
    {
        return maxSWSkill_1_Distance;
    }

    public override string GetActionName()
    {
        return "ÀÏ¼¶";
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
