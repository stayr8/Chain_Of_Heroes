using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.EventSystems;

// 예외적으로 SelectMenuBase 상속 안 받음

public class WorldMap_nextButtonSelectedMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField, Header("nextButton 스프라이트 On")] private Sprite Selected;
    [SerializeField, Header("nextButton 스프라이트 Off")] private Sprite DeSelected;

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

    private void Select()
    {
        image.sprite = Selected;

        image.SetNativeSize();
    }
    private void notSelect()
    {
        image.sprite = DeSelected;

        image.SetNativeSize();
    }
}