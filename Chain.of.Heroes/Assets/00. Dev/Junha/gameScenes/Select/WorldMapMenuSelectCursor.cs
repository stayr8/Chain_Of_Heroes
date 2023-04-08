using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMapMenuSelectCursor : CursorBase
{
    private RectTransform rt;
    public GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        currentSelected = GameObject.Find("_ChapterStart");
    }

    private const float INIT_X = -160f;
    private const float INIT_Y = -260f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_ChapterStart");
    }

    private const float MOVE_DISTANCE = 101f;
    private const float MAX_POSITION_Y = -260f;
    private const float MIN_POSITION_Y = -664f;
    private void Update()
    {
        if(!isChapterStart || !isGoToBaseCamp)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);
        }

        MenuFunction();
    }

    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0f, 0f);
    }



    [SerializeField, Header("�����ϱ�, ��� ��ư")] private GameObject checkButton;
    [SerializeField] private bool isChapterStart = false;
    [SerializeField] private bool isGoToBaseCamp = false;
    [SerializeField, Header("�����ϱ� �ؽ�Ʈ")] private TextMeshProUGUI checkText;
    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentSelected.name)
            {
                #region é�� ����
                case "_ChapterStart":
                    isChapterStart = true;
                    CheckButton(true, "_Continue");
                    checkText.text = "é�͸� �����Ͻðڽ��ϱ�?";

                    break;
                #endregion

                #region ����ǰ
                case "_Inventory":
                    Debug.Log("����ǰ");

                    // �ý��� �ʿ�

                    break;
                #endregion

                #region ����
                case "_Party":
                    Debug.Log("����");

                    // �ý��� �ʿ�

                    break;
                #endregion

                #region ����
                case "_Save":
                    Debug.Log("����");

                    // �ý��� �ʿ�
                    // �׷��� �⺻ Ʋ ���� ����.

                    break;
                #endregion

                #region ���̽� ķ����
                case "_GoToBaseCamp":
                    isGoToBaseCamp = true;
                    CheckButton(true, "_Continue");
                    checkText.text = "���̽� ķ���� �����Ͻðڽ��ϱ�?";

                    // ���̵� �� ��� �ʿ��Ѱ�?
                    //UIManager.ChangeScene();
                    break;
                #endregion

                #region �����ϱ�
                case "_Continue":
                    Debug.Log("�����ϱ�");
                    break;
                #endregion

                #region ���
                case "_Cancel":
                    if(isChapterStart)
                    {
                        isChapterStart = false;
                        CheckButton(false, "_ChapterStart");
                    }
                    else if(isGoToBaseCamp)
                    {
                        isGoToBaseCamp = false;
                        CheckButton(false, "_GoToBaseCamp");
                    }
                    ResetCursor();

                    break;
                    #endregion
            }
        }
    }



    #region Selectable�� ������ ������Ʈ ����
    private void FindAndSelectObject(string name)
    {
        currentSelected = GameObject.Find(name).GetComponent<Selectable>().gameObject;
        currentSelected.GetComponent<Selectable>().Select();
    }
    #endregion

    #region "�����ϱ� / ���" �� �� Ŀ�� �� ���̱�
    private void CheckButton(bool isCheck, string name)
    {
        checkButton.SetActive(isCheck);

        if(isCheck)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
        else if(!isCheck)
        {
            gameObject.GetComponent<Image>().enabled = true;
        }

        FindAndSelectObject(name);
    }
    #endregion

    #region "�����ϱ� / ���" ���� �޴� ����� �� �� Ŀ�� ������
    private void ResetCursor()
    {
        if(currentSelected.name == "_ChapterStart")
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -260f);
        }
        else if(currentSelected.name == "_GoToBaseCamp")
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -664f);
        }
    }
    #endregion
}