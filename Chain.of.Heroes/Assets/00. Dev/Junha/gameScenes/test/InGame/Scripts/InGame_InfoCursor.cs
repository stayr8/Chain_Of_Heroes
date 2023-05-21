using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_InfoCursor : CursorBase
{
    private RectTransform rt;
    public static GameObject currentSelected;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private const float INIT_X = 3.5f; private const float INIT_Y = 310f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_1");
    }

    private const float MOVE_DISTANCE = 110f;
    private const float MAX_POSITION_Y = 310f;
    private const float MIN_POSITION_Y = -460f;

    private void Update()
    {
        MenuFunction();

        Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);
    }

    private void MenuFunction()
    {
        Debug.Log(UnitManager.Instance.GetFriendlyUnitList().Count);
    }
}
