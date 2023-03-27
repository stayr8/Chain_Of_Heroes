using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurnUI : MonoBehaviour
{
    private void Start()
    {
        BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (TurnSystem.Property._isPlayerTurn)
            {
                Show();
            }

        });

        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
