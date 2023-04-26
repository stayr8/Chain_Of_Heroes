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
        SwingingChainAttackWait,
        SwingingChainAttackMoveOn,
        SwingingChainAttackMoving,
        SwingingChainAttackSlash,
        SwingingChainAttackFade,
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
            case State.SwingingChainAttackWait:

                break;
            case State.SwingingChainAttackMoveOn:
                
                break;
            case State.SwingingChainAttackMoving:
                Vector3 targetDirection2 = targetUnit.transform.position;
                Vector3 aimDir2 = (targetDirection2 - transform.position).normalized;
                float rotateSpeed2 = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir2, Time.deltaTime * rotateSpeed2);

                float stoppingDistance1 = 2.0f;
                if (Vector3.Distance(transform.position, targetDirection2) > stoppingDistance1)
                {
                    float moveSpeed = 15f;
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
            case State.SwingingChainAttackFade:

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
                float afterHitStateTime = 1.0f;
                stateTimer = afterHitStateTime;
                state = State.SwingingChainAttackOnLocationMove;

                break;
            case State.SwingingChainAttackOnLocationMove:

                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.OnAtChainLocationMove_1(unit);
                }
                else if(unit.GetChaintwo())
                {
                    AttackActionSystem.Instance.OnAtChainLocationMove_2(unit);
                }
                float afterHitStateTime_0 = 0.5f;
                stateTimer = afterHitStateTime_0;
                state = State.SwingingChainAttackWait;

                break;
            case State.SwingingChainAttackWait:
                if (unit.GetChainfirst())
                {
                    if (AttackActionSystem.Instance.GetIsChainAtk_1())
                    {
                        ActionCameraStart_1();
                        float afterHitStateTime_1 = 0.8f;
                        stateTimer = afterHitStateTime_1;
                        state = State.SwingingChainAttackMoveOn;
                    }
                    else
                    {
                        float afterHitStateTime_1 = 0.2f;
                        stateTimer = afterHitStateTime_1;
                    }
                }
                else if (unit.GetChaintwo())
                {
                    if (AttackActionSystem.Instance.GetIsChainAtk_2())
                    {
                        ActionCameraStart_1();
                        float afterHitStateTime_1 = 0.8f;
                        stateTimer = afterHitStateTime_1;
                        state = State.SwingingChainAttackMoveOn;
                    }
                    else
                    {
                        float afterHitStateTime_1 = 0.2f;
                        stateTimer = afterHitStateTime_1;
                    }
                }

                break;
            case State.SwingingChainAttackMoveOn:
                ActionCameraComplete_1();
                if (!AttackActionSystem.Instance.GetIsAtk())
                {
                    OnChainAttackStartMoving?.Invoke(this, EventArgs.Empty);
                    state = State.SwingingChainAttackMoving;
                }

                break;
            case State.SwingingChainAttackMoving:
                AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());

                break;
            case State.SwingingChainAttackSlash:
                if (unit.GetChainfirst())
                {
                    OnChainAttackSwordSlash?.Invoke(this, EventArgs.Empty);
                }
                else if (unit.GetChaintwo())
                {
                    OnChainAttackSwordSlash?.Invoke(this, EventArgs.Empty);
                    AttackActionSystem.Instance.SetIsChainAtk_2(false);
                }

                float afterHitStateTime_2 = 1.0f;
                stateTimer = afterHitStateTime_2;
                state = State.SwingingChainAttackFade;

                break;
            case State.SwingingChainAttackFade:
                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.SetIsChainAtk_1(false);

                    if (!AttackActionSystem.Instance.GetTripleChain())
                    {
                        StageUI.Instance.Fade();
                    }
                    if (AttackActionSystem.Instance.GetTripleChain())
                    {
                        AttackActionSystem.Instance.SetIsChainAtk_2(true);
                    }
                }
                else if (unit.GetChaintwo())
                {
                    StageUI.Instance.Fade();
                }

                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingChainAttackOffLocationMove;
                break;
            case State.SwingingChainAttackOffLocationMove:
                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.OffAtChainLocationMove_1(unit, targetUnit);
                    if (!AttackActionSystem.Instance.GetTripleChain())
                    {
                        AttackActionSystem.Instance.SetChainStart(false);
                        ActionCameraComplete();
                    }
                }
                else if (unit.GetChaintwo())
                {
                    AttackActionSystem.Instance.OffAtChainLocationMove_2(unit, targetUnit);
                    AttackActionSystem.Instance.SetTripleChain(false);
                    AttackActionSystem.Instance.SetChainStart(false);
                    ActionCameraComplete();
                }
                
                float afterHitStateTime_4 = 0.2f;
                stateTimer = afterHitStateTime_4;
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
