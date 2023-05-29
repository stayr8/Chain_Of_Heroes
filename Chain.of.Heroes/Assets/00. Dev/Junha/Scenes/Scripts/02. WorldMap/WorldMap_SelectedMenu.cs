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

    private void Update()
    {
        SetSize();
    }

    private void OnDisable()
    {
        Select(gameObject,
            "_ChapterStart", -45f,
            "_Party", -55f,
            "_Save", -65f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select(gameObject,
            "_ChapterStart", 55f,
            "_Party", 45f,
            "_Save", 35f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Select(gameObject,
            "_ChapterStart", -45f,
            "_Party", -55f,
            "_Save", -65f);
    }
}