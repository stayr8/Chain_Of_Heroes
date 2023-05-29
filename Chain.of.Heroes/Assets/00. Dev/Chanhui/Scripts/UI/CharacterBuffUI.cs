using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBuffUI : MonoBehaviour
{
    [SerializeField] private Image buffImage;

    private BaseBuff isbuff;


    public void SetBaseAction(BaseBuff basebuff)
    {
        this.isbuff = basebuff;
    }


    private string path = "Character/Skill/";
    public void Set_NameAndImage(BaseBuff basebuff, Unit unit)
    {
        if (unit.GetBuff<SwordWomanPassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill01_Sww");
        }
        else if (unit.GetBuff<KnightPassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Knight/Skill01_Kni");
        }
        else if (unit.GetBuff<SamuraiPassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Samurai/Skill01_Sam");
        }
        else if (unit.GetBuff<ArcherPassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Archer/Skill01_Arc");
        }
        else if (unit.GetBuff<GuardianPassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Guardian/Skill01_Gur");
        }
        else if (unit.GetBuff<PriestPassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Priest/Skill01_Pri");
        }
        else if (unit.GetBuff<WizardPassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Wizard/Skill01_Wiz");
        }
        else if (unit.GetBuff<ValkyriePassive>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill01_Val");
        }
        else if (unit.GetBuff<KnightSkillBuff>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Knight/Skill03_Kni");
        }
        else if (unit.GetBuff<SpiderSkillDebuff>() == basebuff)
        {
            buffImage.sprite = Resources.Load<Sprite>(path + "Wizard/Skill02_Wiz");
        }
    }

}
