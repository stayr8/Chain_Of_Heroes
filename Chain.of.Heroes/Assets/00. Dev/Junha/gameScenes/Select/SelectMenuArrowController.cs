using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectMenuArrowController : MonoBehaviour
{
    [SerializeField, Header("현재 선택된 버튼")] private GameObject currentSelected;

    #region 실행 시
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

    #region Update 함수
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

    #region 이동
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

    #region 메뉴 선택
    [SerializeField, Header("진행하기, 취소 버튼")] private GameObject checkButton;
    [SerializeField] private bool isChapterStart = false;
    [SerializeField] private bool isGoToBaseCamp = false;
    [SerializeField, Header("진행하기 텍스트")] private TextMeshProUGUI checkText;
    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentSelected.name)
            {
                #region 챕터 시작
                case "_ChapterStart":
                    isChapterStart = true;
                    CheckButton(true, "_Continue");
                    checkText.text = "챕터를 진행하시겠습니까?";

                    break;
                #endregion

                #region 소지품
                case "_Inventory":
                    Debug.Log("소지품");
                    // 시스템 필요
                    break;
                #endregion

                #region 동료
                case "_Party":
                    Debug.Log("동료");
                    // 시스템 필요
                    break;
                #endregion

                #region 저장
                case "_Save":
                    Debug.Log("저장");
                    // 시스템 필요
                    // 그러나 기본 틀 구현 가능.
                    break;
                #endregion

                #region 베이스 캠프로
                case "_GoToBaseCamp":
                    isGoToBaseCamp = true;
                    CheckButton(true, "_Continue");
                    checkText.text = "베이스 캠프를 진행하시겠습니까?";

                    // 페이드 인 기능 필요한가?
                    //UIManager.ChangeScene();
                    break;
                #endregion

                #region 진행하기
                case "_Continue":
                    Debug.Log("진행하기");
                    break;
                #endregion

                #region 취소
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

    #region Selectable 선택
    private void Select()
    {
        currentSelected.GetComponent<Selectable>().Select();
    }
    #endregion

    #region Selectable로 지정할 오브젝트 선택
    private void FindAndSelectObject(string name)
    {
        currentSelected = GameObject.Find(name).GetComponent<Selectable>().gameObject;
        Select();
    }
    #endregion

    #region "진행하기 / 취소" 들어갈 때 커서 안 보이기
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

    #region "진행하기 / 취소" 에서 메뉴 고르기로 갈 때 커서 재정렬
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