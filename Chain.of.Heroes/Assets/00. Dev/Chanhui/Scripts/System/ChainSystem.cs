using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSystem : MonoBehaviour
{
    public static ChainSystem Instance { get; private set; }

    private bool IsChain;
    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ChainSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        IsChain = false;
    }

    private void Update()
    {
        if (AttackActionSystem.Instance.GetIsAtk() && !IsChain)
        {
            Debug.Log("체인 시스템 온");
            IsChain = true;
            TryTakeEnemyAIAction(Chain);
        }
    }

    public void SetChain(bool IsChain)
    {
        this.IsChain = IsChain;
    }

    public bool GetChain()
    {
        return IsChain;
    }

    private void Chain()
    {

    }


    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (AttackActionSystem.Instance.GetenemyChainFind() == enemyUnit) 
            {
                if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        BaseAction bestBaseAction = enemyUnit.GetAction<ChainAction>();
        EnemyAIAction bestEnemyAIAction = bestBaseAction.GetBestEnemyAIAction();


        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }
}
