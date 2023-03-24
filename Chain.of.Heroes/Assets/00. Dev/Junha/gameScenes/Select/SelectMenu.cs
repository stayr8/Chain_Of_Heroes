using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField, Header("미선택 스프라이트")] private Sprite notSelected;
    [SerializeField, Header("선택 스프라이트")] private Sprite Selected;

    [SerializeField, Header("진행하기 / 취소 스프라이트 ON")] private Sprite ContiuneCancel_On;

    private RectTransform rt;
    private Image image;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    #region 실행 / 취소
    private void OnEnable()
    {
        if (gameObject.name == "_ChapterStart")
        {
            select();
        }
        else if (gameObject.name == "_Continue" || gameObject.name == "_Cancel")
        {
            rt.anchoredPosition = new Vector2(0f, rt.anchoredPosition.y);
            rt.sizeDelta = new Vector2(350f, rt.sizeDelta.y);
        }
    }
    private void OnDisable()
    {
        deselect();
    }
    #endregion

    #region 이벤트 트리거
    public void OnSelect(BaseEventData eventData)
    {
        if (gameObject.name == "_Continue" || gameObject.name == "_Cancel")
        {
            //새 리소스로 변경
            image.sprite = ContiuneCancel_On;
        }
        else
        {
            select();
        }
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (gameObject.name == "_Continue" || gameObject.name == "_Cancel")
        {
            image.sprite = notSelected;
        }
        else
        {
            deselect();
        }
    }
    #endregion

    #region 선택됐을 때 / 미 선택됐을 때 기능
    private void select()
    {
        image.sprite = Selected;

        rt.anchoredPosition = new Vector2(160f, rt.anchoredPosition.y);
        rt.sizeDelta = new Vector2(572f, rt.sizeDelta.y);
    }
    private void deselect()
    {
        image.sprite = notSelected;

        rt.anchoredPosition = new Vector2(50f, rt.anchoredPosition.y);
        rt.sizeDelta = new Vector2(350f, rt.sizeDelta.y);
    }
    #endregion
}