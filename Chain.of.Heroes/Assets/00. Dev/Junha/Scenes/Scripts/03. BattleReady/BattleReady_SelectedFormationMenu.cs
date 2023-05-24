using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine.EventSystems;

// 예외적으로 SelectMenuBase 상속 안 받음

public class BattleReady_SelectedFormationMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Image image;
    private Sprite Selected;
    private Sprite DeSelected;
    private void Awake()
    {
        image = GetComponent<Image>();

        Selected = Resources.Load<Sprite>("J_B_next_SelectMenu");
        DeSelected = Resources.Load<Sprite>("J_B_next_DeSelectMenu");
    }

    private void OnEnable()
    {
        
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
        image.sprite = DeSelected;

        image.SetNativeSize();
    }
}
