using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisualGameObject;

    private void Start()
    {
        BindingManager.Bind(TurnSystem.Property, "TurnNumber", (object value) =>
        {
            turnNumberText.text = "TURN " + TurnSystem.Property.TurnNumber;
        });

        BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            UpdateEnemyTurnvisual();
        });

    }


   
    private void UpdateEnemyTurnvisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Property.IsPlayerTurn);
    }


}
