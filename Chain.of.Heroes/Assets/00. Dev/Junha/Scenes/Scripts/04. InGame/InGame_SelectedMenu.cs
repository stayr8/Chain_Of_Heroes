using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class InGame_SelectedMenu : SelectMenuBase, ISelectHandler, IDeselectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        Select(110f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        DeSelect(25f);
    }
}