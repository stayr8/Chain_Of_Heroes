using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_UIManager : MonoBehaviour
{
    [SerializeField, Header("메뉴")] private GameObject UI_Menu;
    [SerializeField, Header("유닛 편성")] private GameObject UI_UnitFormation;
    [SerializeField, Header("배치 변경")] private GameObject UI_ChangeFormation;
    [SerializeField, Header("저장")] private GameObject UI_Save;

    public static BattleReady_UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UI_STATE();
    }

    private enum STATE { MENU, UNIT_FORMATION, CHANGE_FORMATION, SAVE }
    private STATE state = STATE.MENU;
    private void UI_STATE()
    {
        switch (state)
        {
            case STATE.MENU:

                break;

            case STATE.UNIT_FORMATION:
                if (!BattleReady_UnitFormationCursor.isOnMenuSelect && Input.GetKeyDown(KeyCode.Escape))
                {
                    UI_UnitFormation.SetActive(false);
                    UI_Menu.SetActive(true);

                    state = STATE.MENU;
                }

                break;

            case STATE.CHANGE_FORMATION:
                if (!BattleReady_UnitFormationCursor.isOnMenuSelect && Input.GetKeyDown(KeyCode.Escape))
                {
                    UI_ChangeFormation.SetActive(false);
                    UI_Menu.SetActive(true);

                    state = STATE.MENU;
                }

                break;

            case STATE.SAVE:
                if (!BattleReady_UnitFormationCursor.isOnMenuSelect && Input.GetKeyDown(KeyCode.Escape))
                {
                    UI_Save.SetActive(false);
                    UI_Menu.SetActive(true);

                    state = STATE.MENU;
                }

                break;
        }
    }

    public void OnUnitFormation()
    {
        state = STATE.UNIT_FORMATION;

        UI_Menu.SetActive(false);
        UI_UnitFormation.SetActive(true);
    }

    public void OnChangeFormation()
    {
        state = STATE.CHANGE_FORMATION;

        UI_Menu.SetActive(false);
        UI_ChangeFormation.SetActive(true);
    }

    public void OnSave()
    {
        state = STATE.SAVE;

        UI_Menu.SetActive(false);
        UI_Save.SetActive(true);
    }

    [SerializeField] private GameObject menuSelected;
    public void OnMenuSelected()
    {
        SoundManager.instance.Sound_SelectMenu();
        menuSelected.SetActive(true);
    }

    public void OffMenuSelected()
    {
        SoundManager.instance.Sound_SelectMenu();
        menuSelected.SetActive(false);
    }
}