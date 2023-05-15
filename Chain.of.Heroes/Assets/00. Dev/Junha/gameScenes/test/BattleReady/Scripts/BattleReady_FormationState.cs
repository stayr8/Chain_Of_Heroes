using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BattleReady_FormationState : MonoBehaviour
{
    public bool isUnlock = false; // 동료를 조우했나, 안 했나
    public bool isFormationState = false; // 편성이 되었나, 안 되었나

    #region instance화 :: Awake()함수 포함
    public static BattleReady_FormationState instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Update()
    {
        formationState();
    }

    private void formationState()
    {
        if(isUnlock)
        {
            if (!isFormationState)
            {
                BattleReady_UIManager.instance.OffFormation(gameObject);
            }
            else // isFormationState
            {
                BattleReady_UIManager.instance.OnFormation(gameObject);
            }
        }
        else // !isUnlock
        {
            BattleReady_UIManager.instance.UnlockCharacter(gameObject);
        }

        //switch (gameObject.name)
        //{
        //    case "_1":
        //        CharacterTypeManager.Instance.SetIsCharacter(0, isFormationState);
        //        break;

        //    case "_2":
        //        CharacterTypeManager.Instance.SetIsCharacter(1, isFormationState);
        //        break;

        //    case "_3":
        //        CharacterTypeManager.Instance.SetIsCharacter(2, isFormationState);
        //        break;

        //    case "_4":
        //        CharacterTypeManager.Instance.SetIsCharacter(3, isFormationState);
        //        break;

        //    case "_5":
        //        CharacterTypeManager.Instance.SetIsCharacter(4, isFormationState);
        //        break;

        //    case "_6":
        //        CharacterTypeManager.Instance.SetIsCharacter(5, isFormationState);
        //        break;

        //    case "_7":
        //        CharacterTypeManager.Instance.SetIsCharacter(6, isFormationState);
        //        break;

        //    case "_8":
        //        CharacterTypeManager.Instance.SetIsCharacter(7, isFormationState);
        //        break;

        //    case "_9":
        //        CharacterTypeManager.Instance.SetIsCharacter(8, isFormationState);
        //        break;
        //}
    }
}