using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class BattleReady_UnitFormationCursor : CursorBase
{
    private RectTransform rt;
    public static GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private bool isInitStart = false;
    private const float INIT_X = -790f; private const float INIT_Y = 375f;
    private void OnEnable()
    {
        if (!isInitStart)
        {
            Init(rt, INIT_X, INIT_Y, ref currentSelected, "_1");
            isInitStart = true;
        }
        else
        {
            return;
        }
    }

    public static bool isOnMenuSelect = false; private bool isOnSkill = false;
    private const float MOVE_DISTANCE_X = 313.5f; private const float MAX_POSITION_X = -476.5f; private const float MIN_POSITION_X = -790f;
    private const float MOVE_DISTANCE_Y = 125f; private const float MAX_POSITION_Y = 375f; private const float MIN_POSITION_Y = 0f;
     
    private void Update()
    {
        MenuFunction();

        if (!isOnMenuSelect)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE_X, MOVE_DISTANCE_Y, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }
        else // isOnMenuSelect
        {
            if (!isOnSkill)
            {
                Movement(ref currentSelected);
            }
            else // isOnSkill
            {
                return;
            }
        }
    }

    private GameObject temp;
    private const int characterNum = 8;
    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            for (int i = 0; i < characterNum; ++i)
            {
                if (currentSelected.name == BattleReady_UIManager.instance.slot[i].name && 
                    !BattleReady_UIManager.instance.slot[i].GetComponent<BattleReady_FormationState>().isUnlock)
                {
                    return;
                }
            }

            isOnMenuSelect = true;

            switch (currentSelected.name)
            {
                case "_1":
                case "_2":
                case "_3":
                case "_4":
                case "_5":
                case "_6":
                case "_7":
                case "_8":
                    temp = currentSelected.gameObject;
                    NextButton_Menu(temp);
                    break;

                case "_Formation":
                    Formation();
                    break;

                case "_Skill":
                    On_SkillCheck();
                    break;
            }
        }

        #region ESC 키를 입력했을 경우
        // 편성, 스킬 확인 창 상태
        if (isOnMenuSelect && !isOnSkill && Input.GetKeyDown(KeyCode.Escape))
        {
            isOnMenuSelect = false;
            Off_FormationSkillSelect();
        }

        // 스킬 확인 창 상태
        if (isOnMenuSelect && isOnSkill && !BattleReady_SkillCursor.isOnDetail && Input.GetKeyDown(KeyCode.Escape))
        {
            Off_SkillCheck();
        }
        #endregion
    }

    #region [편성 / 스킬 확인] 창 On / Off
    private void On_FormationSkillSelect()
    {
        BattleReady_UIManager.instance.OnMenuSelected();

        currentSelected = GameObject.Find("_Formation");
        currentSelected.GetComponent<Selectable>().Select();
    }
    private void Off_FormationSkillSelect()
    {
        BattleReady_UIManager.instance.OffMenuSelected();

        currentSelected = temp;
        currentSelected.GetComponent<Selectable>().Select();
    }
    #endregion

    #region 편성 관련 함수
    private BattleReady_FormationState formationState;
    private TextMeshProUGUI tmp;
    public static int count = 0;
    private void NextButton_Menu(GameObject obj)
    {
        if (isOnMenuSelect)
        {
            On_FormationSkillSelect();
        }
        else // !isOnMenuSelect
        {
            Off_FormationSkillSelect();
        }

        tmp = GameObject.Find("_Formation").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        formationState = obj.GetComponent<BattleReady_FormationState>();

        tmp.text = formationState.isFormationState ? "편성 해제" : "편성"; // true : false
    }
    private void Formation()
    {
        formationState.isFormationState = !formationState.isFormationState;
        count = formationState.isFormationState ? ++count : --count; // true : false

        isOnMenuSelect = false;
        Off_FormationSkillSelect();
    }
    #endregion

    #region 스킬 확인 관련 함수
    private void On_SkillCheck()
    {
        BattleReady_UIManager.instance.OffMenuSelected();

        isOnSkill = true;
        BattleReady_UIManager.instance.OnSkillCursor();

    }
    private void Off_SkillCheck()
    {
        BattleReady_UIManager.instance.OffSkillCursor();
        isOnSkill = false;

        BattleReady_UIManager.instance.OnMenuSelected();

        currentSelected = GameObject.Find("_Skill");
        currentSelected.GetComponent<Selectable>().Select();
    }
    #endregion
}