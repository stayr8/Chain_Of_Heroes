using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_FormationState : MonoBehaviour
{
    #region instanceȭ :: Awake()�Լ� ����
    public static BattleReady_FormationState instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public bool isUnlock = false; // ���Ḧ �����߳�, �� �߳�
    public bool isFormationState = false; // ������ �Ǿ���, �� �Ǿ���

    private void Update()
    {
        formationState();
    }

    private void formationState()
    {
        if (isUnlock)
        {
            BattleReady_UIManager.instance.Formation(gameObject);
        }
        else // !isUnlock
        {
            BattleReady_UIManager.instance.UnlockCharacter(gameObject);
        }

        switch (gameObject.name)
        {
            case "_1":
                CharacterTypeManager.Instance.SetIsCharacter(0, isFormationState);
                break;

            case "_2":
                CharacterTypeManager.Instance.SetIsCharacter(1, isFormationState);
                break;

            case "_3":
                CharacterTypeManager.Instance.SetIsCharacter(2, isFormationState);
                break;

            case "_4":
                CharacterTypeManager.Instance.SetIsCharacter(3, isFormationState);
                break;

            case "_5":
                CharacterTypeManager.Instance.SetIsCharacter(4, isFormationState);
                break;

            case "_6":
                CharacterTypeManager.Instance.SetIsCharacter(5, isFormationState);
                break;

            case "_7":
                CharacterTypeManager.Instance.SetIsCharacter(6, isFormationState);
                break;

            case "_8":
                CharacterTypeManager.Instance.SetIsCharacter(7, isFormationState);
                break;
        }
    }
}