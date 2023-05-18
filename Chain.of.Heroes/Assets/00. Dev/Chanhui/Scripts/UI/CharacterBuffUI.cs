using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBuffUI : MonoBehaviour
{
    [SerializeField] private Image buffImage;



    public void SetBaseAction(BaseBuff basebuff)
    {
       //TODO 어떤 버프 효과가 들어갈지 결정하는 곳.
    }


    private string path = "Character/Skill/";
    public void Set_NameAndImage(CharacterDataManager data)
    {
        switch (data.m_name)
        {
            #region 아카메, SwordWoman
            case "아카메":
                buffImage.sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill01_Sww");
                
                break;
            #endregion

            #region No.2 크리스, Knight
            case "크리스":

                buffImage.sprite = Resources.Load<Sprite>(path + "Knight/Skill01_Kni");
                
                break;
            #endregion

            #region No.3 카미나, Samurai
            case "카미나":

                buffImage.sprite = Resources.Load<Sprite>(path + "Samurai/Skill01_Sam");
                
                break;
            #endregion

            #region No.4 멜리사, Archer
            case "멜리사":

                buffImage.sprite = Resources.Load<Sprite>(path + "Archer/Skill01_Arc");
                
                break;
            #endregion

            #region No.5 플라틴, Guardian
            case "플라틴":

                buffImage.sprite = Resources.Load<Sprite>(path + "Guardian/Skill01_Gur");
                
                break;
            #endregion

            #region No.6 아일린, Priest
            case "아이네":

                buffImage.sprite = Resources.Load<Sprite>(path + "Priest/Skill01_Pri");
                
                break;
            #endregion

            #region No.7 제이브, Wizard
            case "제이브":

                buffImage.sprite = Resources.Load<Sprite>(path + "Wizard/Skill01_Wiz");
                
                break;
            #endregion

            #region No.8 바네사, Vanessa
            case "바네사":

                buffImage.sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill01_Val");
                
                break;
                #endregion
        }
    }

}
