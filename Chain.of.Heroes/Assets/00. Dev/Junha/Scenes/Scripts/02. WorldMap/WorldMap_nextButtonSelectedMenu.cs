using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.EventSystems;

// 예외적으로 SelectMenuBase 상속 안 받음

public class WorldMap_nextButtonSelectedMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Image image;
    private Sprite Selected;
    private Sprite DeSelected;
    private void Awake()
    {
        image = GetComponent<Image>();

        Selected = Resources.Load<Sprite>("J_next_SelectMenu");
        DeSelected = Resources.Load<Sprite>("J_next_DeSelectMenu");
    }

    private void OnEnable()
    {

    }

    // private void Update() {}

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