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
        if(!isInitStart)
        {
            Init(rt, INIT_X, INIT_Y, ref currentSelected, "_1");
            isInitStart = true;
        }
        else
        {
            return;
        }
    }

    public static bool isOnMenuSelect = false; public bool isOnSkill = false;
    private const float MOVE_DISTANCE_X = 313.5f; private const float MOVE_DISTANCE_Y = 125f;
    private const float MAX_POSITION_X = -476.5f; private const float MAX_POSITION_Y = 375f;
    private const float MIN_POSITION_X = -790f; private const float MIN_POSITION_Y = -125f;
    private void Update()
    {
        if (!isOnMenuSelect)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE_X, MOVE_DISTANCE_Y, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }
        else if (isOnMenuSelect)
        {
            if(!isOnSkill)
            {
                Movement(ref currentSelected);
            }
            else
            {
                return;
            }
        }

        MenuFunction();
    }

    private GameObject temp;
    private void MenuFunction()
    {
        // ���� �� â���� ������ �������� ���
        if (currentSelected.name != "_10" && Input.GetKeyDown(KeyCode.Return))
        {
            isOnMenuSelect = true;

            switch (currentSelected.name)
            {
                case "_1":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_2":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_3":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_4":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_5":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_6":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_7":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_8":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                case "_9":
                    temp = currentSelected.gameObject;
                    NextButton(temp);
                    break;

                //case "_10":
                //    break;

                case "_Formation":
                    Formation();
                    break;

                case "_Skill":
                    //isOnSkill = true;
                    Skill();
                    break;
            }
        }

        // ��, ��ų Ȯ�� â ���¿��� ESC ��ư�� ���� ���
        if (isOnMenuSelect && Input.GetKeyDown(KeyCode.Escape))
        {
            isOnMenuSelect = false;
            NextButton();
        }

        //// ��ų Ȯ�� â ���¿��� ESC ��ư�� ���� ���
        //if(isOnSkill && Input.GetKeyDown(KeyCode.Escape))
        //{
        //    isOnSkill = false;
        //    NextButton();
        //}
    }

    private void NextButton()
    {
        if (isOnMenuSelect)
        {
            //if(isOnSkill)
            //{
            //    BattleReady_UIManager.instance.OffMenuSelected();
            //}
            //else if(!isOnSkill)
            //{
            //    BattleReady_UIManager.instance.OffSkillCursor();

            //    BattleReady_UIManager.instance.OnMenuSelected();

            //    currentSelected = GameObject.Find("_Formation");
            //    currentSelected.GetComponent<Selectable>().Select();
            //}

            BattleReady_UIManager.instance.OnMenuSelected();

            currentSelected = GameObject.Find("_Formation");
            currentSelected.GetComponent<Selectable>().Select();
        }
        else if (!isOnMenuSelect && !isOnSkill)
        {
            BattleReady_UIManager.instance.OffMenuSelected();

            currentSelected = temp;
            currentSelected.GetComponent<Selectable>().Select();
        }
    }

    #region �� ���� �Լ�
    BattleReady_FormationState formationState;
    private bool isState;
    private void NextButton(GameObject obj)
    {
        formationState = obj.GetComponent<BattleReady_FormationState>();
        isState = formationState.isFormationState;
        if(!isState)
        {
            tmp.text = "��";
        }
        else if(isState)
        {
            tmp.text = "�� ����";
        }

        if (isOnMenuSelect)
        {
            BattleReady_UIManager.instance.OnMenuSelected();

            currentSelected = GameObject.Find("_Formation");
            currentSelected.GetComponent<Selectable>().Select();
        }
        else if (!isOnMenuSelect)
        {
            BattleReady_UIManager.instance.OffMenuSelected();

            currentSelected = temp;
            currentSelected.GetComponent<Selectable>().Select();
        }
    }
    [SerializeField, Header("[�� / �� ����] �ؽ�Ʈ")] private TextMeshProUGUI tmp;
    private void Formation()
    { 
        if (!isState)
        {
            formationState.isFormationState = true;

            isOnMenuSelect = false;
            NextButton();
        }
        else if (isState)
        {
            formationState.isFormationState = false;

            isOnMenuSelect = false;
            NextButton();
        }

        BattleReady_UIManager.instance.OffMenuSelected();
    }
    #endregion

    #region ��ų Ȯ�� ���� �Լ�
    private void Skill()
    {
        //if(isOnSkill)
        //{
        //    BattleReady_UIManager.instance.OnSkillCursor();
        //}
        //else if(!isOnSkill)
        //{
        //    BattleReady_UIManager.instance.OffSkillCursor();
        //}
        Debug.Log("��ų Ȯ��");
    }
    #endregion
}