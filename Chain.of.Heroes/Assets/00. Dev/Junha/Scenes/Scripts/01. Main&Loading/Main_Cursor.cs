using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Cursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;

    private const float INIT_X = 0f;
    private const float INIT_Y = 120f;

    private const float MOVE_DISTANCE = 100f;
    private const float MIN_POSITION_Y = -180f;
    private const float MAX_POSITION_Y = 120f;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "Start");
    }

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
                    Main_UIManager.instance.GameStart();

                    break;
                case "Continue":
                    Main_UIManager.instance.OnContiune();

                    break;
                case "Credit":
                    Main_UIManager.instance.OnCredit();

                    break;
                case "Exit":
                    Main_UIManager.instance.GameExit();

                    break;
            }
        }
    }
}