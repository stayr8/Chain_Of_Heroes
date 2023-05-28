using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class WorldMap_SelectedMenu : SelectMenuBase, ISelectHandler, IDeselectHandler
{
    private void OnEnable()
    {
        if (gameObject.name == "_ChapterStart")
        {
            Select(55f);
        }
    }
    private void OnDisable()
    {
        DeSelect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select(gameObject,
            "_ChapterStart", 55f,
            "_Party", 45f,
            "_Save", 35f,
            "_BaseCamp", 25f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        DeSelect();
    }
    private void DeSelect()
    {
        Select(gameObject,
            "_ChapterStart", -45f,
            "_Party", -55f,
            "_Save", -65f,
            "_BaseCamp", -75f);
    }
}