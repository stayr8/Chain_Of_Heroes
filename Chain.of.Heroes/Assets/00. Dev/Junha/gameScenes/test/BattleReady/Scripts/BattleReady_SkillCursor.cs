using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_SkillCursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private const float INIT_X = -145f;
    private const float INIT_Y = 107.5f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "Skill_1");
    }

    private const float MOVE_DISTANCE = 92.5f;
    private const float MIN_POSITION_Y = -170f;
    private const float MAX_POSITION_Y = 107.5f;
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
                case "Skill_1":
                    Debug.Log("스킬1 확인");
                    break;

                case "Skill_2":
                    Debug.Log("스킬2 확인");
                    break;

                case "Skill_3":
                    Debug.Log("스킬3 확인");
                    break;

                case "Skill_4":
                    Debug.Log("스킬4 확인");
                    break;
            }
        }
    }
}