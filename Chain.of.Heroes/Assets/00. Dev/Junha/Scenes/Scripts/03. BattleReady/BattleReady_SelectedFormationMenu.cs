using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.EventSystems;

// 예외적으로 SelectMenuBase 상속 안 받음

public class BattleReady_SelectedFormationMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
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
        if(gameObject.name == BattleReady_UnitFormationCursor.currentSelected.name)
        {
            Select();
        }
    }

    // private void Update() {}

    private void OnDisable()
    {
        DeSelect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        DeSelect();
    }

    private void Select()
    {
        image.sprite = Selected;

        image.SetNativeSize();
    }
    private void DeSelect()
    {
        image.sprite = notSelected;

        image.SetNativeSize();
    }
}
