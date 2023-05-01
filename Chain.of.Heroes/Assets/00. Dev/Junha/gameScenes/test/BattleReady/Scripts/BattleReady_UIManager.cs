using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class BattleReady_UIManager : MonoBehaviour
{
    [SerializeField, Header("[�޴�] ������Ʈ")] private GameObject UI_Menu;
    [SerializeField, Header("[���� ��] ������Ʈ")] private GameObject UI_UnitFormation;
    [SerializeField, Header("[��ġ ����] ������Ʈ")] private GameObject UI_ChangeFormation;
    [SerializeField, Header("[����] ������Ʈ")] private GameObject UI_Save;

    #region instanceȭ :: Awake()�Լ� ����
    public static BattleReady_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

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

    #region �� / ��ų Ȯ��
    [SerializeField, Header("[�� / ��ų Ȯ��] ������Ʈ")] private GameObject menuSelected;
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
    #endregion

    #region ���� / �� �Ϸ�
    [SerializeField, Header("[����] ��������Ʈ")] private Sprite Img_OffFormation;
    public void OffFormation(GameObject obj)
    {
        // ��������Ʈ ����
        GameObject child = obj.transform.GetChild(0).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OffFormation;

        // ��������Ʈ ���濡 ���� ������ ����
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-41.5f, 18.5f);
        childRT.sizeDelta = new Vector2(83f, 37f);
    }
    [SerializeField, Header("[�� �Ϸ�] ��������Ʈ")] private Sprite Img_OnFormation;
    public void OnFormation(GameObject obj)
    {
        // ��������Ʈ ����
        GameObject child = obj.transform.GetChild(0).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OnFormation;

        // ��������Ʈ ���濡 ���� ������ ����
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-59.5f, 19f);
        childRT.sizeDelta = new Vector2(119f, 38f);
    }
    #endregion
}