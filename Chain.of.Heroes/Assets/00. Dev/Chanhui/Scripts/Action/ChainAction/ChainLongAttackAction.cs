using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLongAttackAction : BaseAction
{
    public event EventHandler<OnChainShootEventArgs> OnChainShoot;

    public class OnChainShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        SwingingChainAttackStart,
        SwingingChainAttackOnLocationMove,
        SwingingChainAttackWait,
        SwingingChainAttackAiming,
        SwingingChainAttackShooting,
        SwingingChainAttackOffLocationMove,
        SwingingChainAttackComplete,
    }


    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

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
            case State.SwingingChainAttackShooting:

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
                AttackActionSystem.Instance.SetIsChainAtk_1(true);
                float afterHitStateTime = 1.0f;
                stateTimer = afterHitStateTime;
                state = State.SwingingChainAttackOnLocationMove;

                break;
            case State.SwingingChainAttackOnLocationMove:
                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.OnAtChainLocationMove_1(unit);
                }
                else if (unit.GetChaintwo())
                {
                    AttackActionSystem.Instance.OnAtChainLocationMove_2(unit);
                }
                float afterHitStateTime_0 = 1.0f;
                stateTimer = afterHitStateTime_0;
                state = State.SwingingChainAttackWait;

                break;
            case State.SwingingChainAttackWait:
                float afterHitStateTime_1 = 1.5f;
                stateTimer = afterHitStateTime_1;
                AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());
                state = State.SwingingChainAttackShooting;

                break;
            case State.SwingingChainAttackShooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                    AttackActionSystem.Instance.SetIsChainAtk_1(false);
                }
                

                float afterHitStateTime_2 = 1.0f;
                stateTimer = afterHitStateTime_2;
                state = State.SwingingChainAttackOffLocationMove;

                break;
            case State.SwingingChainAttackOffLocationMove:
                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.OffAtChainLocationMove_1(unit, targetUnit);
                }
                else if (unit.GetChaintwo())
                {
                    AttackActionSystem.Instance.OffAtChainLocationMove_2(unit, targetUnit);
                }
                float afterHitStateTime_3 = 0.5f;
                stateTimer = afterHitStateTime_3;
                state = State.SwingingChainAttackComplete;

                break;
            case State.SwingingChainAttackComplete:

                ActionComplete();

                break;
        }

    }

    private void Shoot()
    {
        OnChainShoot?.Invoke(this, new OnChainShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
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

        canShootBullet = true;

        ActionStart(onActionComplete);
    }

    public override string GetActionName()
    {
        return "체인 원거리 공격";
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
