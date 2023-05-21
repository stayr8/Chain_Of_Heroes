using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkill2Action : BaseAction
{
    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private enum State
    {
        SwingingArSkill_2_CameraStart,
        SwingingArSkill_2_BeforeCamera,
        SwingingArSkill_2_AttackCameraEnd,
        SwingingArSkill_2_AttackStand,
        SwingingArSkill_2_Aiming,
        SwingingArSkill_2_Shooting,
        SwingingArSkill_2_Cooloff,
    }

    [SerializeField] private LayerMask obstaclesLayerMask;
    [SerializeField] private int maxArSkill_2_Distance = 2;

    private State state;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

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

        switch (state)
        {
            case State.SwingingArSkill_2_CameraStart:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                float rotateSpeed = 20f;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateSpeed);

                break;
            case State.SwingingArSkill_2_BeforeCamera:

                break;
            case State.SwingingArSkill_2_AttackCameraEnd:

                break;
            case State.SwingingArSkill_2_AttackStand:

                break;
            case State.SwingingArSkill_2_Aiming:


                break;
            case State.SwingingArSkill_2_Shooting:

                break;
            case State.SwingingArSkill_2_Cooloff:

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
            case State.SwingingArSkill_2_CameraStart:
                AttackCameraStart();

                TimeAttack(0.5f);
                state = State.SwingingArSkill_2_BeforeCamera;

                break;
            case State.SwingingArSkill_2_BeforeCamera:
                ScreenManager._instance._LoadScreenTextuer();

                TimeAttack(0.1f);
                state = State.SwingingArSkill_2_AttackCameraEnd;

                break;
            case State.SwingingArSkill_2_AttackCameraEnd:
                AttackActionSystem.Instance.OnAtLocationMove(unit, targetUnit);
                ActionCameraStart();

                TimeAttack(1.0f);
                state = State.SwingingArSkill_2_AttackStand;

                break;
            case State.SwingingArSkill_2_AttackStand:
                AttackCameraComplete();

                TimeAttack(0.3f);
                state = State.SwingingArSkill_2_Aiming;

                break;
            case State.SwingingArSkill_2_Aiming:

                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }

                TimeAttack(2.0f);
                state = State.SwingingArSkill_2_Shooting;

                break;
            case State.SwingingArSkill_2_Shooting:
                ScreenManager._instance._LoadScreenTextuer();
                TimeAttack(0.1f);
                state = State.SwingingArSkill_2_Cooloff;

                break;
            case State.SwingingArSkill_2_Cooloff:
                ActionCameraComplete();
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

        for (int x = -maxArSkill_2_Distance; x <= maxArSkill_2_Distance; x++)
        {
            for (int z = -maxArSkill_2_Distance; z <= maxArSkill_2_Distance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if (testX == 0 || testZ == 0 || testX == testZ)
                {
                    continue;
                }

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid Position is empty, no Unit
                    continue;
                }

                if (!Pathfinding.Instance.IsWalkableGridPosition(testGridPosition))
                {
                    continue;
                }

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;

                float unitShoulderHeight = 1.7f;
                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir,
                    Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                    obstaclesLayerMask))
                {
                    // Blocked by an Obstacle
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
            isSkillCount = 4;
        }

        TimeAttack(0.7f);
        state = State.SwingingArSkill_2_CameraStart;

        canShootBullet = true;

        AttackActionSystem.Instance.SetUnitChainFind(targetUnit, unit);
        AttackActionSystem.Instance.SetCharacterDataManager(unit.GetCharacterDataManager());

        ActionStart(onActionComplete);
    }

    public int GetMaxArSkill_2_Distance()
    {
        return maxArSkill_2_Distance;
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
        return "스나이핑";
    }

    public override string GetSingleActionPoint()
    {
        return "4";
    }
    public override int GetActionPointsCost()
    {
        return 4;
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
