using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

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

    public static bool isOnDetail = false;
    private const float MOVE_DISTANCE = 92.5f;
    private const float MIN_POSITION_Y = -170f;
    private const float MAX_POSITION_Y = 107.5f;
    private void Update()
    {
        MenuFunction();

        if (!isOnDetail)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);
        }
        else
        {
            return;
        }
    }

    private void MenuFunction()
    {
        if (!isOnDetail && Input.GetKeyDown(KeyCode.Return))
        {
            gameObject.GetComponent<Image>().enabled = false;
            isOnDetail = true;

            switch (currentSelected.name)
            {
                case "Skill_1":
                    BattleReady_UIManager.instance.OnSkillDetail();
                    break;

                case "Skill_2":
                    BattleReady_UIManager.instance.OnSkillDetail();
                    break;

                case "Skill_3":
                    BattleReady_UIManager.instance.OnSkillDetail();
                    break;

                case "Skill_4":
                    BattleReady_UIManager.instance.OnSkillDetail();
                    break;
            }
        }

        if (isOnDetail && Input.GetKeyDown(KeyCode.Escape))
        {
            isOnDetail = false;
            BattleReady_UIManager.instance.OffSkillDetail();

            gameObject.GetComponent<Image>().enabled = true;
        }
    }
}