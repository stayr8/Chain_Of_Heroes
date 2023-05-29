using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{

    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
    [SerializeField] private GameObject actionButtionBackground;
    [SerializeField] private TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private List<Binding> Binds = new List<Binding>();

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            UpdateActionPoints();
        });
        Binds.Add(Bind);

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OffSelectedUnitChanged += UnitActionSystem_OffSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        InGame_UIManager.instance.OnCharacterInstance += InGame_UIManager_OnCharacterInstance;

        UpdateActionPoints();

        UpdateSelectedVisual();
    }


    private void CreateUnitActionButtons()
    {
        DestroyActionButton();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelecterdUnit();
        actionButtionBackground.SetActive(true);
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            if (baseAction.GetActionName() == "Empty" ||
               baseAction.GetActionName() == "체인 근거리 공격" ||
               baseAction.GetActionName() == "체인 원거리 공격" ||
               baseAction.GetActionName() == "스턴")
            {
                continue;
            }

            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
        }
    }

    private void DestroyActionButton()
    {
        actionButtionBackground.SetActive(false);
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
        Update_Data();
    }

    private void UnitActionSystem_OffSelectedUnitChanged(object sender, EventArgs e)
    {
        DestroyActionButton();
        //Update_Data();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void UpdateSelectedVisual()
    {
        foreach(ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
        SkillCoolTimeVisual();
    }

    private void SkillCoolTimeVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.SkillCoolTimeVisual();
        }
    }

    private void UpdateActionPoints()
    {
        if (TurnSystem.Property.IsPlayerTurn)
        {
            actionPointsText.text = "AP  " + TurnSystem.Property.ActionPoints + "/" + TurnSystem.Property.AllPlayerPoint;
        }
        else
        {
            actionPointsText.text = "AP  " + TurnSystem.Property.ActionPoints + "/" + TurnSystem.Property.AllEnemyPoint;
        }
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void InGame_UIManager_OnCharacterInstance(object sender, EventArgs e)
    {
        DestroyActionButton();
    }


    #region 유닛 편성: 캐릭터 정보 갱신
    [Header("[캐릭터 정보] 텍스트")]
    [SerializeField] private TextMeshProUGUI character_Name;
    [SerializeField] private TextMeshProUGUI character_Class;
    [SerializeField] private TextMeshProUGUI character_Level;
    [SerializeField] private TextMeshProUGUI character_HP;
    [SerializeField] private TextMeshProUGUI character_MaxHP;
    [SerializeField] private TextMeshProUGUI character_AttackPower;
    [SerializeField] private TextMeshProUGUI character_ChainAttackPower;
    [SerializeField] private TextMeshProUGUI character_DefensePower;
    [SerializeField] private Image character_Image;
    [SerializeField] private Image property_Image;
    private CharacterDataManager data;
    private void Update_Data()
    {
        GameObject obj = UnitActionSystem.Instance.GetSelecterdUnit().gameObject;
        if (obj.GetComponent<CharacterDataManager>() == null)
        {
            return;
        }
        data = obj.GetComponent<CharacterDataManager>();

        Set_NameAndImage();

        character_Name.text = data.m_name.ToString();
        character_Class.text = data.m_class.ToString();
        character_Level.text = "Lv. " + data.m_level.ToString();
        character_HP.text = ((int)data.m_hp).ToString();
        character_MaxHP.text = ((int)data.m_maxhp).ToString();
        character_AttackPower.text = ((int)data.m_attackPower).ToString();
        character_ChainAttackPower.text = ((int)data.m_chainAttackPower).ToString();
        character_DefensePower.text = ((int)data.m_defensePower).ToString();

        if(data.m_property == "물리")
        {
            property_Image.sprite = Resources.Load<Sprite>("Sword");
        }
        else if (data.m_property == "사격")
        {
            property_Image.sprite = Resources.Load<Sprite>("Bow");
        }
        else if (data.m_property == "마법")
        {
            property_Image.sprite = Resources.Load<Sprite>("Magic");
        }

    }

    private RectTransform rt;
    private void Set_NameAndImage()
    {
        rt = character_Image.gameObject.GetComponent<RectTransform>();
        character_Image.sprite = Resources.Load<Sprite>(data.m_resourcePath);

        switch (data.m_name)
        {
            case "아카메": // _1
                rt.anchoredPosition = new Vector2(-710f, -341f);
                break;

            case "크리스": // _2
                rt.anchoredPosition = new Vector2(-601f, -341f);
                break;

            case "카미나": // _3
                rt.anchoredPosition = new Vector2(-524f, -341f);
                break;

            case "멜리사": // _4
                rt.anchoredPosition = new Vector2(-710f, -341f);
                break;

            case "플라틴": // _5
                rt.anchoredPosition = new Vector2(-584f, -415f);
                break;

            case "아이네": // _6
                rt.anchoredPosition = new Vector2(-524f, -341f);
                break;

            case "제이브": // _7
                rt.anchoredPosition = new Vector2(-584f, -415f);
                break;

            case "바네사": // _8
                rt.anchoredPosition = new Vector2(-620f, -222f);
                break;
        }
    }
    #endregion


    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }

        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OffSelectedUnitChanged -= UnitActionSystem_OffSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted -= UnitActionSystem_OnActionStarted;
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        InGame_UIManager.instance.OnCharacterInstance -= InGame_UIManager_OnCharacterInstance;
    }
}
