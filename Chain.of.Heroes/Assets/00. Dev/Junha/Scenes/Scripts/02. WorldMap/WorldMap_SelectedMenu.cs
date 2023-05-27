using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class WorldMap_SelectedMenu : SelectMenuBase, ISelectHandler, IDeselectHandler
{
    // private void Awake() {}

    // private void Start() {}

    private void OnEnable()
    {
        if (gameObject.name == "_ChapterStart")
        {
            Select(true, gameObject,
                "_ChapterStart", 55f,
                "_Party", 45f,
                "_Save", 35f,
                "_BaseCamp", 25f);
        }
    }

    // private void Update() {}

    private void OnDisable()
    {
        Select(false, gameObject,
            "_ChapterStart", -45f,
            "_Party", -55f,
            "_Save", -65f,
            "_BaseCamp", -75f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select(true, gameObject,
            "_ChapterStart", 55f,
            "_Party", 45f,
            "_Save", 35f,
            "_BaseCamp", 25f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Select(false, gameObject,
            "_ChapterStart", -45f,
            "_Party", -55f,
            "_Save", -65f,
            "_BaseCamp", -75f);
    }
}