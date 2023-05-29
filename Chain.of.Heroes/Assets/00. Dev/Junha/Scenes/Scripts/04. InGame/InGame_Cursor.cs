using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_Cursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;

    private const float INIT_X = -920f; private const float INIT_Y = 100f;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_Info");
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
                    InGame_UIManager.instance.OnPartyInfo();
                    break;

                case "_TurnEnd":
                    InGame_UIManager.instance.OnTurnfo();
                    break;

                case "_Surrender":
                    InGame_UIManager.instance.Onfallfo();

                    Reset();
                    break;
            }
        }
    }

    private void Reset()
    {
        UnitManager.Instance.OnDestroys();
        GridSystemVisual.Instance.DestroyGridPositionList();
    }
}