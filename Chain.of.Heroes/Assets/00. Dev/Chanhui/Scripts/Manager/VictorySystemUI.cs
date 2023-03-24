using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VictorySystemUI : MonoBehaviour
{
   

    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject playerVictoryVisualGameObject;
    [SerializeField] private GameObject enemyVictoryVisualGameObject;



    private void Start()
    {
        BindingManager.Bind(TurnSystem.Property, "TurnNumber", (object value) =>
        {
            if (UnitManager.Instance.VictoryPlayer())
            {
                turnNumberText.text = "TURN : " + TurnSystem.Property.TurnNumber;
            }
            else if (UnitManager.Instance.VictoryEnemy())
            {
                turnNumberText.text = "TURN : " + TurnSystem.Property.TurnNumber;
            }
    
        });

    }

}
