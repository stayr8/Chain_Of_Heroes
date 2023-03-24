using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField, Header("�̼��� ��������Ʈ")] private Sprite notSelected;
    [SerializeField, Header("���� ��������Ʈ")] private Sprite Selected;

    [SerializeField, Header("�����ϱ� / ��� ��������Ʈ ON")] private Sprite ContiuneCancel_On;

    private RectTransform rt;
    private Image image;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    #region ���� / ���
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

    #region �̺�Ʈ Ʈ����
    public void OnSelect(BaseEventData eventData)
    {
        if (gameObject.name == "_Continue" || gameObject.name == "_Cancel")
        {
            //�� ���ҽ��� ����
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

    #region ���õ��� �� / �� ���õ��� �� ���
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