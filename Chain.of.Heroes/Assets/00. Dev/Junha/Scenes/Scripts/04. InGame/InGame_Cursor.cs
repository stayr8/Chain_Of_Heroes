using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_Y = 100f;
    private const float MIN_POSITION_Y = -100f;
    private void Update()
    {
        MenuFunction();

        Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);
    }

    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0f, 0f);
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

    public static GameObject GetCurrentSelected()
    {
        return currentSelected;
    }

    private void Reset()
    {
        UnitManager.Instance.OnDestroys();
        GridSystemVisual.Instance.DestroyGridPositionList();
    }
}