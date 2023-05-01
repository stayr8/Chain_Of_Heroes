using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_FormationState : MonoBehaviour
{
    public bool isFormationState = false;

    private void Update()
    {
        formationState();
    }

    private void formationState()
    {
        if (!isFormationState)
        {
            BattleReady_UIManager.instance.OffFormation(gameObject);
        }
        else if (isFormationState)
        {
            BattleReady_UIManager.instance.OnFormation(gameObject);
        }
    }
}