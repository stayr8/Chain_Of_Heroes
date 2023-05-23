using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class InGame_Cursor : CursorBase
{
    private RectTransform rt;
    public static GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    public static bool isInitStart = false;
    private const float INIT_X = -920f; private const float INIT_Y = 100f;
    private void OnEnable()
    {
        if (!isInitStart)
        {
            Init(rt, INIT_X, INIT_Y, ref currentSelected, "_Info");
            isInitStart = true;
        }
        else
        {
            return;
        }
    }

    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_Y = 100f;
    private const float MIN_POSITION_Y = -100f;
    private void Update()
    {
        MenuFunction();

        Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);
    }

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentSelected.name)
            {
                case "_Info":
                    InGame_UIManager.instance.OnInfo();
                    break;

                case "_TurnEnd":
                    Debug.Log("턴 종료");
                    break;

                case "_Surrender":
                    Debug.Log("포기");
                    break;
            }
        }
    }

    public static GameObject GetCurrentSelected()
    {
        return currentSelected;
    }
}