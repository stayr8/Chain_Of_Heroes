using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWorldUI : MonoBehaviour
{
    [SerializeField] private Unit player;
    [SerializeField] private GameObject chainUI;

    private bool _ischainui;

    private void Start()
    {
        _ischainui = false;
    }

    private void Update()
    {
        if(player.GetIsChainPossibleState())
        {
            if(!_ischainui)
            {
                chainUI.SetActive(true);
                _ischainui = true;
            }
        }
        else
        {
            if (_ischainui)
            {
                chainUI.SetActive(false);
                _ischainui = false;
            }
        }
    }

}
