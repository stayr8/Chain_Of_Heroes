using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnUI : MonoBehaviour
{
    [SerializeField] private GameObject PlayerTurn;
    [SerializeField] private GameObject EnemyTurn;

    private void Start()
    {
        BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (TurnSystem.Property.IsPlayerTurn)
            {
                PlayerShow();
                EnemyHide();
            }
            else
            {
                EnemyShow();
                PlayerHide();
            }

        });

        PlayerHide();
        EnemyHide();
    }

    private void PlayerShow()
    {
        PlayerTurn.SetActive(true);
    }

    private void PlayerHide()
    {
        PlayerTurn.SetActive(false);
    }

    private void EnemyShow()
    {
        EnemyTurn.SetActive(true);
    }

    private void EnemyHide()
    {
        EnemyTurn.SetActive(false);
    }
}
