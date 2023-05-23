using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;

public class BattleReady_SelectedMenu : SelectMenuBase, ISelectHandler, IDeselectHandler
{
    // private void Awake() {}

    private void OnEnable()
    {
        if (gameObject == BattleReady_Cursor.GetCurrentSelected())
        {
            Select(true, gameObject,
                "_BattleStart", 55f,
                "_UnitFormation", 45f,
                "_ChangeFormation", 35f,
                "_Back", 25f);
        }
    }

    // private void Update() {}

    private void OnDisable()
    {
        Select(false, gameObject,
            "_BattleStart", -45f,
            "_UnitFormation", -55f,
            "_ChangeFormation", -65f,
            "_Back", -75f);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select(true, gameObject,
            "_BattleStart", 55f,
            "_UnitFormation", 45f,
            "_ChangeFormation", 35f,
            "_Back", 25f);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        Select(false, gameObject,
            "_BattleStart", -45f,
            "_UnitFormation", -55f,
            "_ChangeFormation", -65f,
            "_Back", -75f);
    }
}