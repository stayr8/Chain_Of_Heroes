using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private TextMeshProUGUI textMeshProPoint;
    [SerializeField] private TextMeshProUGUI skillCount;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;
    [SerializeField] private GameObject skillGameObject;


    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        textMeshProPoint.text = "AP " + baseAction.GetSingleActionPoint().ToUpper();
        if(baseAction.GetIsSkill())
        {
            if (baseAction.GetSkillCountPoint() > 0)
            {
                skillGameObject.SetActive(true);
                skillCount.text = "" + baseAction.GetSkillCountPoint();
            }
        }

        if (baseAction.GetMaxSkillCount() == baseAction.GetSkillCountPoint())
        {
            button.onClick.AddListener(() =>
            {
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
        }
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        
        if (selectedBaseAction.GetMaxSkillCount() == selectedBaseAction.GetSkillCountPoint())
        {
            selectedGameObject.SetActive(selectedBaseAction == baseAction);
        }
    }

    public void SkillCoolTimeVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedBaseAction.GetIsSkill())
        {
            skillGameObject.SetActive(selectedBaseAction == baseAction);
        }
    }

}
