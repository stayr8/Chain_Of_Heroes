using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGame_SelectMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField, Header("선택 스프라이트")] private Sprite Selected;
    [SerializeField, Header("미선택 스프라이트")] private Sprite notSelected;

    private RectTransform rt;
    private Image image;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (gameObject.name == "_Info")
        {
            Select();
        }
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

    private void Select()
    {
        image.sprite = Selected;

        rt.anchoredPosition = new Vector2(110f, rt.anchoredPosition.y);

        image.SetNativeSize();
    }
    private void notSelect()
    {
        image.sprite = notSelected;

        rt.anchoredPosition = new Vector2(25f, rt.anchoredPosition.y);

        image.SetNativeSize();
    }
}
