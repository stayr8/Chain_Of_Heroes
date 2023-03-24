using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectMenuArrowController : MonoBehaviour
{
    [SerializeField, Header("���� ���õ� ��ư")] private GameObject currentSelected;

    #region ���� ��
    private RectTransform rt;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();

        Init();
    }
    private void OnEnable()
    {
        rt.anchoredPosition = new Vector2(-160f, -260f);

        Init();
    }
    private void Init()
    {
        currentSelected = EventSystem.current.firstSelectedGameObject;

        Select();
    }
    #endregion

    #region Update �Լ�
    private void Update()
    {
        if(!isChapterStart || !isGoToBaseCamp)
        {
            Movement();
        }

        MenuFunction();
    }
    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0, 0);
    }
    #endregion

    #region �̵�
    private const float MOVE_DISTANCE = 101f;
    private const float MAX_POSITION_Y = -260f;
    private const float MIN_POSITION_Y = -664f;
    private void Movement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rt.anchoredPosition += new Vector2(0, MOVE_DISTANCE);

            currentSelected = currentSelected.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
            Select();

            if (rt.anchoredPosition.y > MAX_POSITION_Y)
            {
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, MIN_POSITION_Y);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rt.anchoredPosition += new Vector2(0, -MOVE_DISTANCE);

            currentSelected = currentSelected.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
            Select();

            if (rt.anchoredPosition.y < MIN_POSITION_Y)
            {
                rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, MAX_POSITION_Y);
            }
        }
    }
    #endregion

    #region �޴� ����
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
    #endregion

    #region Selectable ����
    private void Select()
    {
        currentSelected.GetComponent<Selectable>().Select();
    }
    #endregion

    #region Selectable�� ������ ������Ʈ ����
    private void FindAndSelectObject(string name)
    {
        currentSelected = GameObject.Find(name).GetComponent<Selectable>().gameObject;
        Select();
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