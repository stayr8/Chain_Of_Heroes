using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLongAttackAction : BaseAction
{
    public event EventHandler<OnChainShootEventArgs> OnChainShoot;
    //public event EventHandler OnChainLongAttackStartMoving;
    //public event EventHandler OnChainLongAttackStopMoving;

    public class OnChainShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        SwingingChainLongAttackStart,
        SwingingChainLongAttackOnLocationMove,
        SwingingChainLongAttackWait,
        SwingingChainLongAttackAiming,
        SwingingChainLongAttackShooting,
        SwingingChainLongAttackFade,
        SwingingChainLongAttackOffLocationMove,
        SwingingChainLongAttackComplete,
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
            case State.SwingingChainLongAttackStart:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingChainLongAttackOnLocationMove:

                break;
            case State.SwingingChainLongAttackWait:

                break;
            case State.SwingingChainLongAttackAiming:

                break;
            case State.SwingingChainLongAttackShooting:

                break;
            case State.SwingingChainLongAttackFade:

                break;
            case State.SwingingChainLongAttackOffLocationMove:

                break;
            case State.SwingingChainLongAttackComplete:

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
            case State.SwingingChainLongAttackStart:

                TimeAttack(0.6f);
                state = State.SwingingChainLongAttackOnLocationMove;

                break;
            case State.SwingingChainLongAttackOnLocationMove:

                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.OnAtChainLocationMove_1(unit);
                }
                else if (unit.GetChaintwo())
                {
                    AttackActionSystem.Instance.OnAtChainLocationMove_2(unit);
                }

                TimeAttack(0.5f);
                state = State.SwingingChainLongAttackWait;

                break;
            case State.SwingingChainLongAttackWait:
                if (unit.GetChainfirst())
                {
                    if (AttackActionSystem.Instance.GetIsChainAtk_1())
                    {
                        AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());
                        ActionCameraComplete_1();

                        if (targetUnit.GetHealth() <= 0)
                        {
                            state = State.SwingingChainLongAttackFade;
                        }
                        else
                        {
                            ActionCameraStart_1();
                            TimeAttack(1.8f);
                            state = State.SwingingChainLongAttackAiming;
                        }

                    }
                    else
                    {
                        TimeAttack(0.2f);
                    }
                }
                else if (unit.GetChaintwo())
                {
                    if (AttackActionSystem.Instance.GetIsChainAtk_2())
                    {
                        AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());
                        ActionCameraComplete_1();

                        if (targetUnit.GetHealth() <= 0)
                        {
                            state = State.SwingingChainLongAttackFade;
                        }
                        else
                        {
                            ActionCameraStart_1();
                            TimeAttack(1.8f);
                            state = State.SwingingChainLongAttackAiming;
                        }
                    }
                    else
                    {
                        TimeAttack(0.2f);
                    }
                }

                break;
            case State.SwingingChainLongAttackAiming:
                ActionCameraComplete_1();

                TimeAttack(0.3f);
                state = State.SwingingChainLongAttackShooting;

                break;
            case State.SwingingChainLongAttackShooting:
                
                if (unit.GetChainfirst())
                {
                    if (canShootBullet)
                    {
                        Shoot();
                        canShootBullet = false;
                    }

                    if (AttackActionSystem.Instance.GetTripleChain())
                    {
                        AttackActionSystem.Instance.SetTripleChainPosition();
                    }
                }
                else if (unit.GetChaintwo())
                {
                    if (canShootBullet)
                    {
                        Shoot();
                        canShootBullet = false;
                    }

                    AttackActionSystem.Instance.SetIsChainAtk_2(false);
                }

                TimeAttack(0.7f);
                state = State.SwingingChainLongAttackFade;

                break;
            case State.SwingingChainLongAttackFade:
                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.SetIsChainAtk_1(false);

                    if (!AttackActionSystem.Instance.GetTripleChain())
                    {
                        ScreenManager._instance._LoadScreenTextuer();

                        TimeAttack(0.1f);
                        state = State.SwingingChainLongAttackOffLocationMove;
                    }
                    else
                    {
                        AttackActionSystem.Instance.Camera2();
                        AttackActionSystem.Instance.SetIsChainAtk_2(true);

                        TimeAttack(0.5f);
                        state = State.SwingingChainLongAttackOffLocationMove;
                    }
                }
                else if (unit.GetChaintwo())
                {
                    ScreenManager._instance._LoadScreenTextuer();

                    TimeAttack(0.1f);
                    state = State.SwingingChainLongAttackOffLocationMove;
                }

                break;
            case State.SwingingChainLongAttackOffLocationMove:
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

                TimeAttack(0.2f);
                state = State.SwingingChainLongAttackComplete;

                break;
            case State.SwingingChainLongAttackComplete:

                ActionComplete();

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

        TimeAttack(0.7f);
        state = State.SwingingChainLongAttackStart;

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
    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
