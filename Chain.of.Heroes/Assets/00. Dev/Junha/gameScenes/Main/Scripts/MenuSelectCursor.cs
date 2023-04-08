using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSelectCursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        currentSelected = GameObject.Find("Start");
    }

    private const float INIT_X = 0f;
    private const float INIT_Y = 120f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "Start");
    }

    private const float MOVE_DISTANCE = 100f;
    private const float MIN_POSITION_Y = -180f;
    private const float MAX_POSITION_Y = 120f;
    private void Update()
    {
        Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);

        MenuFunction();
    }

    private void MenuFunction()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            switch(currentSelected.name)
            {
                case "Start":
                    UIManager_Main.instance.GameStart();

                    break;
                case "Continue":
                    UIManager_Main.instance.GameContinue();

                    break;
                case "Credit":
                    UIManager_Main.instance.GameCredit();

                    break;
                case "Exit":
                    UIManager_Main.instance.GameExit();

                    break;
            }
        }
    }
}