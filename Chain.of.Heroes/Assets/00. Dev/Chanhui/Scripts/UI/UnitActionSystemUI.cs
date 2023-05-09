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

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            UpdateActionPoints();
        });

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OffSelectedUnitChanged += UnitActionSystem_OffSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

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
            if(baseAction.GetActionName() == "Empty" || baseAction.GetActionName() == "체인 근거리 공격" || baseAction.GetActionName() == "체인 원거리 공격")
                continue;

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
        UpdateData();
    }

    private void UnitActionSystem_OffSelectedUnitChanged(object sender, EventArgs e)
    {
        DestroyActionButton();
        UpdateData();
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
    private CharacterDataManager data;
    private void UpdateData()
    {
        Unit obj = UnitActionSystem.Instance.GetSelecterdUnit();
        if (obj.gameObject.GetComponent<CharacterDataManager>() == null)
        {
            return;
        }
        data = obj.gameObject.GetComponent<CharacterDataManager>();

        Set_NameAndImage();
        character_Name.text = data.m_name;
        //character_Class.text = data.m_class.ToString();
        character_Level.text = "Lv. " + data.m_level.ToString();
        character_HP.text = data.m_hp.ToString();
        character_MaxHP.text = data.m_maxhp.ToString();
        character_AttackPower.text = data.m_attackPower.ToString();
        character_ChainAttackPower.text = data.m_chainAttackPower.ToString();
        character_DefensePower.text = data.m_defensePower.ToString();
    }

    private RectTransform rt;
    private void Set_NameAndImage()
    {
        rt = character_Image.gameObject.GetComponent<RectTransform>();

        switch (data.m_name)
        {
            case "Akame": // _1
                character_Name.text = "아카메";
                rt.anchoredPosition = new Vector2(-710f, -341f);
                break;

            case "Kris": // _2
                character_Name.text = "크리스";
                rt.anchoredPosition = new Vector2(-601f, -341f);
                break;

            case "Teo": // _3
                character_Name.text = "태오";
                rt.anchoredPosition = new Vector2(-524f, -341f);
                break;

            case "Melia": // _4
                character_Name.text = "멜리아";
                rt.anchoredPosition = new Vector2(-710f, -341f);
                break;

            case "Platin": // _5
                character_Name.text = "플라틴";
                //rt.anchoredPosition = new Vector2(433f, -415f);
                break;

            case "Raiden": // _6
                character_Name.text = "라이덴";
                //rt.anchoredPosition = new Vector2(433f, -415f);
                break;

            case "Eileene": // _7
                character_Name.text = "아일린";
                rt.anchoredPosition = new Vector2(-524f, -341f);
                break;

            case "Jave": // _8
                character_Name.text = "제이브";
                //rt.anchoredPosition = new Vector2(433f, -415f);
                break;

            case "Vanessa": // _9
                character_Name.text = "바네사";
                rt.anchoredPosition = new Vector2(-620f, -222f);
                break;
        }
        character_Image.sprite = Resources.Load<Sprite>("Character/Illustration/" + data.m_name);
    }
    #endregion


    private void OnDisable()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OffSelectedUnitChanged -= UnitActionSystem_OffSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted -= UnitActionSystem_OnActionStarted;
        Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
    }
}
