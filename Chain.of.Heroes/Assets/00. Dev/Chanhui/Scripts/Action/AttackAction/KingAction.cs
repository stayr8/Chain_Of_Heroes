using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingAction : BaseAction
{

    public event EventHandler OnKingSwordSlash;
    public event EventHandler OnKingStartMoving;
    public event EventHandler OnKingStopMoving;

    private enum State
    {
        SwingingKingAttackCameraStart,
        SwingingKingBeforeCamera,
        SwingingKingAttackCameraEnd,
        SwingingKingAttackStand,
        SwingingKingAttackMoving,
        SwingingKingBeforeHit,
        SwingingKingAfterCamera,
        SwingingKingAfterHit,
    }

    [SerializeField] private int maxKingDistance = 1;

    private State state;
    private float stateTimer;
    private  Unit targetUnit;
   
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 200,
        };

    }

    private void Update()
    {
        if(!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingKingAttackCameraStart:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingKingBeforeCamera:

                break;
            case State.SwingingKingAttackStand:

                break;
            case State.SwingingKingAttackCameraEnd:

                break;
            case State.SwingingKingAttackMoving:
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
                    OnKingStopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingKingBeforeHit;
                }

                break;
            case State.SwingingKingBeforeHit:
                
                break;
            case State.SwingingKingAfterCamera:

                break;
            case State.SwingingKingAfterHit:
                
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
            case State.SwingingKingAttackCameraStart:
                AttackCameraStart();
                TimeAttack(0.5f);
                UnitActionSystem.Instance.SetCameraPointchange(true);
                state = State.SwingingKingBeforeCamera;

                break;
            case State.SwingingKingBeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();
                TimeAttack(0.1f);
                state = State.SwingingKingAttackStand;

                break;
            case State.SwingingKingAttackStand:
                if (unit.IsEnemy())
                {
                    AttackActionSystem.Instance.OnAtLocationMove(targetUnit, unit);
                }
                else
                {
                    AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                }
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingKingAttackCameraEnd;

                break;
            case State.SwingingKingAttackCameraEnd:
                AttackCameraComplete();
                OnKingStartMoving?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingKingAttackMoving;

                break;
            case State.SwingingKingAttackMoving:

                break;
            case State.SwingingKingBeforeHit:
                OnKingSwordSlash?.Invoke(this, EventArgs.Empty);

                TimeAttack(1.0f);
                state = State.SwingingKingAfterCamera;

                break;
            case State.SwingingKingAfterCamera:
                if (unit.IsEnemy())
                {
                    ScreenManager._instance._LoadScreenTextuer();
                    TimeAttack(0.1f);
                    state = State.SwingingKingAfterHit;
                }
                else
                {
                    if (!AttackActionSystem.Instance.GetChainStart())
                    {
                        ScreenManager._instance._LoadScreenTextuer();
                        TimeAttack(0.1f);
                        state = State.SwingingKingAfterHit;
                    }
                    else
                    {
                        AttackActionSystem.Instance.SetIsChainAtk_1(true);
                        TimeAttack(0.5f);
                        state = State.SwingingKingAfterHit;
                    }
                    AttackActionSystem.Instance.SetIsAtk(false);
                }

                break;
            case State.SwingingKingAfterHit:

                if (unit.IsEnemy())
                {
                    AttackActionSystem.Instance.OffAtLocationMove(targetUnit, unit);
                    ActionCameraComplete();
                }
                else
                {
                    if (!AttackActionSystem.Instance.GetChainStart())
                    {
                        ActionCameraComplete();
                    }
                    AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);
                }

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

        for (int x = -maxKingDistance; x <= maxKingDistance; x++)
        {
            for (int z = -maxKingDistance; z <= maxKingDistance; z++)
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

                if (unit.IsEnemy() && isProvoke)
                {
                    if(LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition))
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

                if (!unit.IsEnemy())
                {
                    if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).IsEnemy())
                    {
                        LevelGrid.Instance.GetChainStateGridPosition(testGridPosition);
                    }
                    else
                    {
                        continue;
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
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingKingAttackCameraStart;
        TimeAttack(0.7f);

        if (!unit.IsEnemy())
        {
            AttackActionSystem.Instance.SetIsAtk(true);
            AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);
        }
        else
        {
            AttackActionSystem.Instance.SetUnitChainFind(unit, targetUnit);
        }

        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "공격";
    }

    public int GetMaxKingDistance()
    {
        return maxKingDistance;
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }

    public override string GetSingleActionPoint()
    {
        return "2";
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
