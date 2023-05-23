using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyAttackStarted;
    public static event EventHandler OnAnyAttackCompleted;

    public static event EventHandler OnAnyActionStarted_1;
    public static event EventHandler OnAnyActionCompleted_1;

    public static bool isProvoke;

    protected Unit unit;
    protected bool isActive;
    protected bool isSkill;
    protected int isSkillCount;
    protected int isMaxSkillCount;
    protected Action onActionComplete;


    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();
    public abstract string GetSingleActionPoint();
    public virtual int GetSkillCountPoint()
    {
        return 0;
    }

    public abstract int GetMaxSkillCount();
    


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
        UnitActionSystem.Instance.SetCameraPointchange(false);
        if (!TurnSystem.Property.IsTurnEnd && (TurnSystem.Property.IsPlayerTurn && (TurnSystem.Property.ActionPoints < 1)))
        {
            if (!AttackActionSystem.Instance.GetIsChainAtk_1() && !AttackActionSystem.Instance.GetIsChainAtk_2())
            {
                Debug.Log("ÅÏ ±³Ã¼");
                TurnSystem.Property.IsPlayerTurn = false;
            }

        }
        if (!unit.IsEnemy())
        {
            UnitActionSystem.Instance.SetDoubleSelUnit(true);
        }

        if (UnitManager.Instance.VictoryPlayer() || UnitManager.Instance.VictoryEnemy())
        {
            TurnSystem.Property.IsTurnEnd = true;
        }

        if (!unit.IsEnemy())
        {
            ChainSystem.Instance.SetChain(false);
        }

    }

    protected void ActionCameraStart()
    {
        StageUI.Instance.TurnSystemHide();
        StageUI.Instance.AttackShow();
    }

    protected void ActionCameraComplete()
    {
        StageUI.Instance.AttackHide();
        StageUI.Instance.TurnSystemShow();
    }

    protected void ActionCameraStart_1()
    {
        OnAnyActionStarted_1?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionCameraComplete_1()
    {
        OnAnyActionCompleted_1?.Invoke(this, EventArgs.Empty);
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

    public bool GetIsSkill()
    {
        return isSkill;
    }

    public int GetIsSkillCount()
    {
        return isSkillCount;
    }

    public int GetMaxIsSkillCount()
    {
        return isMaxSkillCount;
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
