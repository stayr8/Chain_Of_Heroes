using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class nextButtonSelectedMenu : SelectMenuBase, ISelectHandler, IDeselectHandler
{
    private void OnDisable()
    {
        SetSize();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetSize();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        SetSize();
    }
}