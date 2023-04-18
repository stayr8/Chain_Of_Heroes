using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleReady_MenuSelectCursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();

        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_BattleStart");
    }

    private const float INIT_X = -860f;
    private const float INIT_Y = 200f;
    private void OnEnable()
    {
        
    }

    public static bool isBattleStart = false;
    public static bool isBack = false;
    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_X = -860f; private const float MAX_POSITION_Y = 200f;
    private const float MIN_POSITION_X = -910f; private const float MIN_POSITION_Y = -300f;
    private void Update()
    {
        if (!isBattleStart || !isBack)
        {
            Movement_forWorld(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }

        MenuFunction();
    }
    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0f, 0f);
    }

    [SerializeField, Header("nextButton")] private GameObject nextButton;
    [SerializeField, Header("nextButton Text")] private TextMeshProUGUI text_nextButton;

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.instance.Sound_SelectMenu();
            switch (currentSelected.name)
            {
                case "_BattleStart":
                    isBattleStart = true;
                    NextButton(true, "_Yes");
                    text_nextButton.text = "������ �����Ͻðڽ��ϱ�?";

                    break;

                case "_UnitFormation":
                    BattleReady_UIManager.instance.OnUnitFormation();

                    break;

                #region �̱���
                case "_Maintenance":
                    Debug.Log("����");
                    BattleReady_UIManager.instance.OnMaintenance();

                    break;

                case "_ChangeFormation":
                    Debug.Log("��ġ ����");
                    BattleReady_UIManager.instance.OnChangeFormation();

                    break;

                case "_Save":
                    Debug.Log("����");
                    BattleReady_UIManager.instance.OnSave();

                    break;
                #endregion
                                  
                case "_Back":
                    isBack = true;
                    NextButton(true, "_Yes");
                    text_nextButton.text = "���� ������ ���ư��ðڽ��ϱ�?";

                    break;
                case "_Yes":
                    MapManager.Instance.stageNum = 0;
                    SceneManager.LoadScene("Ch_01");

                    break;
                case "_No":
                    if (isBattleStart)
                    {
                        isBattleStart = false;
                        NextButton(false, "_BattleStart");
                    }
                    else if (isBack)
                    {
                        isBack = false;
                        NextButton(false, "_Back");
                    }
                    ResetCursorPosition();

                    break;
            }
        }
    }



    private void NextButton(bool isCheck, string name)
    {
        nextButton.SetActive(isCheck);

        if (isCheck)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
        else if (!isCheck)
        {
            gameObject.GetComponent<Image>().enabled = true;
        }

        currentSelected = GameObject.Find(name);
        currentSelected.GetComponent<Selectable>().Select();
    }

    private void ResetCursorPosition()
    {
        if (currentSelected.name == "_BattleStart")
        {
            rt.anchoredPosition = new Vector2(-800f, MAX_POSITION_Y);
        }
        else if (currentSelected.name == "_Back")
        {
            rt.anchoredPosition = new Vector2(-910f, MIN_POSITION_Y);
        }
    }
}