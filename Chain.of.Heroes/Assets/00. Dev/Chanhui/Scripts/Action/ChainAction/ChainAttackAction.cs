using System;
using System.Collections;
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
                TimeAttack(0.6f);
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
                TimeAttack(0.5f);
                state = State.SwingingChainAttackWait;

                break;
            case State.SwingingChainAttackWait:
                if (unit.GetChainfirst())
                {
                    if (AttackActionSystem.Instance.GetIsChainAtk_1())
                    {
                        if(targetUnit.GetHealth() <= 0)
                        {
                            state = State.SwingingChainAttackFade;
                        }
                        else
                        {
                            AttackActionSystem.Instance.CharacterChange(unit);
                            ActionCameraStart_1();
                            TimeAttack(1.8f);
                            state = State.SwingingChainAttackMoveOn;
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
                        if (targetUnit.GetHealth() <= 0)
                        {
                            state = State.SwingingChainAttackFade;
                        }
                        else
                        {
                            AttackActionSystem.Instance.CharacterChange(unit);
                            ActionCameraStart_1();

                            TimeAttack(1.8f);
                            state = State.SwingingChainAttackMoveOn;
                        }
                    }
                    else
                    {
                        TimeAttack(0.2f);
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
                    if(unit.GetUnitName() == "크리스")
                    {
                        StartCoroutine(AttackDamages());
                    }
                    else if(unit.GetUnitName() == "아카메")
                    {
                        StartCoroutine(AttackDamages3());
                    }
                    if (AttackActionSystem.Instance.GetTripleChain())
                    {
                        AttackActionSystem.Instance.SetTripleChainPosition();
                    }
                }
                else if (unit.GetChaintwo())
                {
                    OnChainAttackSwordSlash?.Invoke(this, EventArgs.Empty);
                    AttackActionSystem.Instance.SetIsChainAtk_2(false);
                    if (unit.GetUnitName() == "크리스")
                    {
                        StartCoroutine(AttackDamages2());
                    }
                    else if (unit.GetUnitName() == "아카메")
                    {
                        StartCoroutine(AttackDamages4());
                    }
                }

                TimeAttack(1.0f);
                state = State.SwingingChainAttackFade;

                break;
            case State.SwingingChainAttackFade:
                if (unit.GetChainfirst())
                {
                    AttackActionSystem.Instance.SetIsChainAtk_1(false);

                    if (!AttackActionSystem.Instance.GetTripleChain())
                    {
                        ScreenManager._instance._LoadScreenTextuer();

                        TimeAttack(0.1f);
                        state = State.SwingingChainAttackOffLocationMove;
                    }
                    else
                    {
                        AttackActionSystem.Instance.SetIsChainAtk_2(true);

                        TimeAttack(0.5f);
                        state = State.SwingingChainAttackOffLocationMove;
                    }
                }
                else if (unit.GetChaintwo())
                {
                    ScreenManager._instance._LoadScreenTextuer();

                    TimeAttack(0.1f);
                    state = State.SwingingChainAttackOffLocationMove;
                }

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

                TimeAttack(0.2f);
                state = State.SwingingChainAttackComplete;

                break;
            case State.SwingingChainAttackComplete:

                ActionComplete();

                break;
        }

    }
    void TimeAttack(float StateTime)
    {
        float afterHitStateTime = StateTime;
        stateTimer = afterHitStateTime;
    }

    IEnumerator AttackDamages()
    {
        yield return new WaitForSeconds(0.3f);
        targetUnit.GetMonsterDataManager().Damage();
        yield return new WaitForSeconds(0.3f);
        targetUnit.GetMonsterDataManager().Damage();
    }
    IEnumerator AttackDamages2()
    {
        yield return new WaitForSeconds(0.4f);
        targetUnit.GetMonsterDataManager().Damage();
        yield return new WaitForSeconds(0.3f);
        targetUnit.GetMonsterDataManager().Damage();
    }

    IEnumerator AttackDamages3()
    {
        yield return new WaitForSeconds(0.3f);
        targetUnit.GetMonsterDataManager().Damage();
    }
    IEnumerator AttackDamages4()
    {
        yield return new WaitForSeconds(0.4f);
        targetUnit.GetMonsterDataManager().Damage();
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
        TimeAttack(0.7f);

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

    public override int GetMaxSkillCount()
    {
        return 0;
    }

}
