using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_UIManager : MonoBehaviour
{
    [SerializeField, Header("메뉴")] private GameObject UI_Menu;
    [SerializeField, Header("유닛 편성")] private GameObject UI_UnitFormation;
    [SerializeField, Header("정비")] private GameObject UI_Maintenance;
    [SerializeField, Header("배치 변경")] private GameObject UI_ChangeFormation;
    [SerializeField, Header("저장")] private GameObject UI_Save;

    public static BattleReady_UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void OnUnitFormation()
    {
        UI_Menu.SetActive(false);
        UI_UnitFormation.SetActive(true);
    }

    public void OnMaintenance()
    {
        UI_Menu.SetActive(false);
        UI_UnitFormation.SetActive(true);
    }

    public void OnChangeFormation()
    {
        UI_Menu.SetActive(false);
        UI_UnitFormation.SetActive(true);
    }

    public void OnSave()
    {
        UI_Menu.SetActive(false);
        UI_UnitFormation.SetActive(true);
    }
}
