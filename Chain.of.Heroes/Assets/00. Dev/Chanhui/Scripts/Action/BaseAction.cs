using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;
    public static event EventHandler OnAnyAttackStarted;
    public static event EventHandler OnAnyAttackCompleted;


    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;


    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract string GetSingleActionPoint();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointsCost()
    {
        return 1;
    }

    protected void ActionStart(Action onActionComplete)
    {
        isActive = true;
        this.onActionComplete = onActionComplete;

        if (!unit.IsEnemy())
        {
            UnitActionSystem.Instance.OutSelectedUnit(unit);
        }
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();

        if (!TurnSystem.Property.IsTurnEnd && (TurnSystem.Property.IsPlayerTurn && (TurnSystem.Property.ActionPoints < 1)))
        {
            Debug.Log("ÅÏ ±³Ã¼");
            TurnSystem.Property.IsPlayerTurn = false;

        }
        if (!unit.IsEnemy())
        {
            UnitActionSystem.Instance.SetDoubleSelUnit(true);
            UnitActionSystem.Instance.SetCameraSelUnit(true);
        }

        if (UnitManager.Instance.VictoryPlayer() || UnitManager.Instance.VictoryEnemy())
        {
            TurnSystem.Property.IsTurnEnd = true;
        }

        if (TurnSystem.Property.IsPlayerTurn)
        {
            ChainSystem.Instance.SetChain(false);
        }
    }

    protected void ActionCameraStart()
    {
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        StageUI.Instance.TurnSystemHide();
        StageUI.Instance.AttackShow();
    }

    protected void ActionCameraComplete()
    {
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        StageUI.Instance.AttackHide();
        StageUI.Instance.TurnSystemShow();
    }

    protected void AttackCameraStart()
    {
        OnAnyAttackStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void AttackCameraComplete()
    {
        OnAnyAttackCompleted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit()
    {
        return unit;
    }


    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

        foreach(GridPosition gridPosition in validActionGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            enemyAIActionList.Add(enemyAIAction);
        }

        if (enemyAIActionList.Count > 0)
        {
            enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return enemyAIActionList[0];
        }
        else
        {
            // No possible Enemy AI Action
            return null;
        }

    }

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);
}
