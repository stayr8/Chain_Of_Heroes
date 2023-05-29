using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class BattleReady_SelectedMenu : SelectMenuBase, ISelectHandler, IDeselectHandler
{
    private void OnEnable()
    {
        if (gameObject.name == "_BattleStart")
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
            "_BattleStart", -45f,
            "_UnitFormation", -55f,
            "_ChangeFormation", -65f,
            "_Back", -75f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select(gameObject,
            "_BattleStart", 55f,
            "_UnitFormation", 45f,
            "_ChangeFormation", 35f,
            "_Back", 25f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Select(gameObject,
            "_BattleStart", -45f,
            "_UnitFormation", -55f,
            "_ChangeFormation", -65f,
            "_Back", -75f);
    }
}