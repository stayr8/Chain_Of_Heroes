using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class InGame_SelectedMenu : SelectMenuBase, ISelectHandler, IDeselectHandler
{
    // private void Awake() {}

    private void OnEnable()
    {
        if (gameObject == InGame_Cursor.GetCurrentSelected())
        {
            Select(110f);
        }
    }

    // private void Update() {}

    private void OnDisable()
    {
        DeSelect(25f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select(110f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        DeSelect(25f);
    }
}