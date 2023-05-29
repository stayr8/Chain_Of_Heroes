using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBuffUI : MonoBehaviour
{
    [SerializeField] private Image buffImage;


    public void SetBaseAction(BaseBuff basebuff)
    {
        //TODO 어떤 버프 효과가 들어갈지 결정하는 곳.
    }

    private string path = "Character/Skill/";
    public void Set_NameAndImage(BaseBuff basebuff, Unit unit)
    {
        if (unit.GetBuff<WizardSkillDebuff>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Wizard/Skill02_Wiz");
        }
        else
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Guardian/Skill01_Gur");
        }
    }
}
