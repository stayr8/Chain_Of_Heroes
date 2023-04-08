using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSelectCursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        currentSelected = GameObject.Find("DataSlot1");
    }

    private const float INIT_X = 850f;
    private const float INIT_Y = 325f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "DataSlot1");
    }

    private const float MOVE_DISTANCE = 325f;
    private const float MIN_POSITION_Y = -325f;
    private const float MAX_POSITION_Y = 325f;
    private void Update()
    {
        Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);

        MenuFunction();
    }

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentSelected.name)
            {
                case "DataSlot1":
                    Debug.Log("µ•¿Ã≈ÕΩΩ∑‘1");

                    break;
                case "DataSlot2":
                    Debug.Log("µ•¿Ã≈ÕΩΩ∑‘2");

                    break;
                case "DataSlot3":
                    Debug.Log("µ•¿Ã≈ÕΩΩ∑‘3");

                    break;
            }
        }
    }
}