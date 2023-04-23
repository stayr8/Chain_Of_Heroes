using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class TurnSystemUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI turnNumberText;

    private List<Binding> Binds = new List<Binding>();

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            turnNumberText.text = "TURN " + TurnSystem.Property.TurnNumber;
        });
        Binds.Add(Bind);


    }




    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }

}
