using System;
using System.Collections.Generic;
using UnityEngine;

public class ChainAttackAction : BaseAction
{

    public event EventHandler OnChainAttackSwordSlash;
    public event EventHandler OnChainAttackStartMoving;
    public event EventHandler OnChainAttackStopMoving;

    private enum State
    {
        SwingingChainAttackStart,
        SwingingChainAttackOnLocationMove,
        SwingingChainAttackMoveOn,
        SwingingChainAttackMoving,
        SwingingChainAttackSlash,
        SwingingChainAttackOffLocationMove,
        SwingingChainAttackComplete,
    }


    private State state;
    private float stateTimer;
    private Unit targetUnit;

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };

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
            case State.SwingingChainAttackStart:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingChainAttackOnLocationMove:

                break;
            case State.SwingingChainAttackMoveOn:
                /*
                if(!AttackActionSystem.Instance.GetIsAtk())
                {
                    state = State.SwingingChainAttackMoving;
                }*/
                
                break;
            case State.SwingingChainAttackMoving:
                Vector3 targetDirection2 = targetUnit.transform.position;
                Vector3 aimDir2 = (targetDirection2 - transform.position).normalized;
                float rotateSpeed2 = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir2, Time.deltaTime * rotateSpeed2);

                float stoppingDistance1 = 1.5f;
                if (Vector3.Distance(transform.position, targetDirection2) > stoppingDistance1)
                {
                    float moveSpeed = 6f;
                    transform.position += aimDir2 * moveSpeed * Time.deltaTime;
                }
                else
                {
                    OnChainAttackStopMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingChainAttackSlash;
                }

                break;
            case State.SwingingChainAttackSlash:

                break;
            case State.SwingingChainAttackOffLocationMove:

                break;
            case State.SwingingChainAttackComplete:

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
            case State.SwingingChainAttackStart:
                AttackActionSystem.Instance.SetIsChainAtk(true);
                float afterHitStateTime = 1.0f;
                stateTimer = afterHitStateTime;
                state = State.SwingingChainAttackOnLocationMove;

                break;
            case State.SwingingChainAttackOnLocationMove:
                AttackActionSystem.Instance.OnAtChainLocationMove(unit);
                float afterHitStateTime_0 = 0.5f;
                stateTimer = afterHitStateTime_0;
                state = State.SwingingChainAttackMoveOn;

                break;
            case State.SwingingChainAttackMoveOn:
                if(!AttackActionSystem.Instance.GetIsAtk())
                {
                    OnChainAttackStartMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingChainAttackMoving;
                }

                break;
            case State.SwingingChainAttackMoving:
                AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());
                break;
            case State.SwingingChainAttackSlash:
                OnChainAttackSwordSlash?.Invoke(this, EventArgs.Empty);
                AttackActionSystem.Instance.SetIsChainAtk(false);

                float afterHitStateTime_2 = 2.0f;
                stateTimer = afterHitStateTime_2;
                state = State.SwingingChainAttackOffLocationMove;

                break;
            case State.SwingingChainAttackOffLocationMove:

                AttackActionSystem.Instance.OffAtChainLocationMove(unit);
                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingChainAttackComplete;

                break;
            case State.SwingingChainAttackComplete:
               
                ActionComplete();

                break;
        }

    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingingChainAttackStart;
        float beforeHitStateTime = 0.7f;
        stateTimer = beforeHitStateTime;

        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "체인 근거리 공격";
    }

    public override int GetActionPointsCost()
    {
        return 0;
    }

    public override string GetSingleActionPoint()
    {
        return "0";
    }


}
