using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_UIManager : MonoBehaviour
{
    [SerializeField, Header("�޴�")] private GameObject UI_Menu;
    [SerializeField, Header("���� ��")] private GameObject UI_UnitFormation;
    [SerializeField, Header("����")] private GameObject UI_Maintenance;
    [SerializeField, Header("��ġ ����")] private GameObject UI_ChangeFormation;
    [SerializeField, Header("����")] private GameObject UI_Save;

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
