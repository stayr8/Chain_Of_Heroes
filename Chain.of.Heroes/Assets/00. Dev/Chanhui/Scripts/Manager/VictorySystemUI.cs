using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
                SoundManager.instance.Sound_StageWin();

                turnNumberText.text = "" + TurnSystem.Property.TurnNumber;
            }
            else if (UnitManager.Instance.VictoryEnemy())
            {
                SoundManager.instance.Sound_StageLose();

                turnNumberText.text = "" + TurnSystem.Property.TurnNumber;
            }
    
        });

    }

    private void Update()
    {
        if(InputManager.Instance.IsMouseButtonDown())
        {
            SceneManager.LoadScene("ChoiceScene");
        }
    }

}
