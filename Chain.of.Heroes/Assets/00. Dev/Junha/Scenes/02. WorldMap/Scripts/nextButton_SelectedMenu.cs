using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class nextButton_SelectedMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField, Header("nextButton 스프라이트 On")] private Sprite nextButton_Selected;
    [SerializeField, Header("nextButton 스프라이트 Off")] private Sprite nextButton_notSelected;

    private RectTransform rt;
    private Image image;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        notSelect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        notSelect();
    }

    private const float Selected_X = 587f;
    private const float Selected_Y = 79f;
    private void Select()
    {
        image.sprite = nextButton_Selected;

        rt.sizeDelta = new Vector2(Selected_X, Selected_Y);
    }
    private const float notSelected_X = 571f;
    private const float notSelected_Y = 60f;
    private void notSelect()
    {
        image.sprite = nextButton_notSelected;

        rt.sizeDelta = new Vector2(notSelected_X, notSelected_Y);
    }
}