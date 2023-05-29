using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongBishopAction : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        SwingingWizardAttackCameraStart,
        SwingingWizardBeforeCamera,
        SwingingWizardAttackCameraEnd,
        SwingingWizardAttackStand,
        SwingingWizardAiming,
        SwingingWizardShooting,
        SwingingWizardCooloff,
    }

    [SerializeField] private int maxWizardDistance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingingWizardAttackCameraStart:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingWizardBeforeCamera:

                break;
            case State.SwingingWizardAttackCameraEnd:

                break;
            case State.SwingingWizardAttackStand:

                break;
            case State.SwingingWizardAiming:


                break;
            case State.SwingingWizardShooting:

                break;
            case State.SwingingWizardCooloff:

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
            case State.SwingingWizardAttackCameraStart:
                AttackCameraStart();
                UnitActionSystem.Instance.SetCameraPointchange(true);
                TimeAttack(0.5f);
                state = State.SwingingWizardBeforeCamera;

                break;
            case State.SwingingWizardBeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingWizardAttackCameraEnd;

                break;
            case State.SwingingWizardAttackCameraEnd:
                AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingWizardAttackStand;

                break;
            case State.SwingingWizardAttackStand:
                AttackCameraComplete();

                TimeAttack(0.3f);
                state = State.SwingingWizardAiming;

                break;
            case State.SwingingWizardAiming:

                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }

                TimeAttack(1.0f);
                state = State.SwingingWizardShooting;

                break;
            case State.SwingingWizardShooting:
                if (!AttackActionSystem.Instance.GetChainStart())
                {
                    ScreenManager._instance._LoadScreenTextuer();

                    TimeAttack(0.1f);
                    state = State.SwingingWizardCooloff;
                }
                else
                {
                    AttackActionSystem.Instance.SetIsChainAtk_1(true);
                    AttackActionSystem.Instance.Camera();
                    AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);

                    TimeAttack(0.4f);
                    state = State.SwingingWizardCooloff;
                }
                AttackActionSystem.Instance.SetIsAtk(false);

                break;
            case State.SwingingWizardCooloff:

                if (!AttackActionSystem.Instance.GetChainStart())
                {
                    ActionCameraComplete();
                }
                AttackActionSystem.Instance.OffAtLocationMove(unit, targetUnit);

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
        OnShoot?.Invoke(this, new OnShootEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
    }


    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(unitGridPosition);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxWizardDistance; x <= maxWizardDistance; x++)
        {
            for (int z = -maxWizardDistance; z <= maxWizardDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if (testX != testZ)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                if (LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition).IsEnemy())
                {
                    LevelGrid.Instance.GetChainStateGridPosition(testGridPosition);
                }
                else
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

        TimeAttack(0.7f);
        state = State.SwingingWizardAttackCameraStart;

        canShootBullet = true;

        AttackActionSystem.Instance.SetIsAtk(true);
        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);

        ActionStart(onActionComplete);
    }

    public int GetMaxWizardDistance()
    {
        return maxWizardDistance;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {

        return new EnemyAIAction
        {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

    public override string GetActionName()
    {
        return "����";
    }

    public override string GetSingleActionPoint()
    {
        return "2";
    }

    public override int GetActionPointsCost()
    {
        return 2;
    }

    public override int GetMaxSkillCount()
    {
        return 0;
    }
}
