using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class BattleReady_UIManager : MonoBehaviour
{
     public event EventHandler OnCharacterChangeFormation;

    [SerializeField, Header("[메뉴] 오브젝트")] private GameObject UI_Menu;
    [SerializeField, Header("[유닛 편성] 오브젝트")] private GameObject UI_UnitFormation;
    [SerializeField, Header("[배치 변경] 오브젝트")] private GameObject UI_ChangeFormation;

    #region instance화 :: Awake()함수 포함
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

    private enum STATE { MENU, UNIT_FORMATION, CHANGE_FORMATION }
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
                UpdateData();
                break;

            case STATE.CHANGE_FORMATION:
                if (!BattleReady_UnitFormationCursor.isOnMenuSelect && Input.GetKeyDown(KeyCode.Escape))
                {
                    UI_ChangeFormation.SetActive(false);
                    UI_Menu.SetActive(true);
                    //ChangeFormationSystem.Instance.DestroyCharacterUI();

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
        OnCharacterChangeFormation?.Invoke(this, EventArgs.Empty);
    }

    #region 편성 / 스킬 확인
    [SerializeField, Header("[편성 / 스킬 확인] 오브젝트")] private GameObject menuSelected;
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

    #region 미편성 / 편성 완료
    [SerializeField, Header("[미편성] 스프라이트")] private Sprite Img_OffFormation;
    public void OffFormation(GameObject obj)
    {
        // 스프라이트 변경
        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OffFormation;

        // 스프라이트 변경에 따른 포지션 변경
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-41.5f, 18.5f);
        childRT.sizeDelta = new Vector2(83f, 37f);
    }
    [SerializeField, Header("[편성 완료] 스프라이트")] private Sprite Img_OnFormation;
    public void OnFormation(GameObject obj)
    {
        // 스프라이트 변경
        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OnFormation;

        // 스프라이트 변경에 따른 포지션 변경
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-59.5f, 19f);
        childRT.sizeDelta = new Vector2(119f, 38f);
    }
    #endregion

    [Header("[캐릭터 정보] 텍스트")]
    [SerializeField] private TextMeshProUGUI character_Name;
    [SerializeField] private TextMeshProUGUI character_Class;
    [SerializeField] private TextMeshProUGUI character_Level;
    [SerializeField] private TextMeshProUGUI character_HP;
    [SerializeField] private TextMeshProUGUI character_AttackPower;
    [SerializeField] private TextMeshProUGUI character_ChainAttackPower;
    [SerializeField] private TextMeshProUGUI character_DefensePower;
    private CharacterDataManager data;
    private void UpdateData()
    {
        GameObject obj = BattleReady_UnitFormationCursor.currentSelected;
        if(obj.name == "_10")
        {
            return;
        }
        data = obj.GetComponentInChildren<CharacterDataManager>();

        character_Name.text = data.m_name;
        //character_Class.text = data.m_class.ToString();
        character_Level.text = "Lv. " + data.m_level.ToString();
        character_HP.text = data.m_hp.ToString();
        character_AttackPower.text = data.m_attackPower.ToString();
        character_ChainAttackPower.text = data.m_chainAttackPower.ToString();
        character_DefensePower.text = data.m_defensePower.ToString();
    }
}