using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBuffUI : MonoBehaviour
{
    [SerializeField] private Image buffImage;



    public void SetBaseAction(BaseBuff basebuff)
    {
       //TODO � ���� ȿ���� ���� �����ϴ� ��.
    }


    private string path = "Character/Skill/";
    public void Set_NameAndImage(CharacterDataManager data)
    {
        switch (data.m_name)
        {
            #region ��ī��, SwordWoman
            case "��ī��":
                buffImage.sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill01_Sww");
                
                break;
            #endregion

            #region No.2 ũ����, Knight
            case "ũ����":

                buffImage.sprite = Resources.Load<Sprite>(path + "Knight/Skill01_Kni");
                
                break;
            #endregion

            #region No.3 ī�̳�, Samurai
            case "ī�̳�":

                buffImage.sprite = Resources.Load<Sprite>(path + "Samurai/Skill01_Sam");
                
                break;
            #endregion

            #region No.4 �Ḯ��, Archer
            case "�Ḯ��":

                buffImage.sprite = Resources.Load<Sprite>(path + "Archer/Skill01_Arc");
                
                break;
            #endregion

            #region No.5 �ö�ƾ, Guardian
            case "�ö�ƾ":

                buffImage.sprite = Resources.Load<Sprite>(path + "Guardian/Skill01_Gur");
                
                break;
            #endregion

            #region No.6 ���ϸ�, Priest
            case "���̳�":

                buffImage.sprite = Resources.Load<Sprite>(path + "Priest/Skill01_Pri");
                
                break;
            #endregion

            #region No.7 ���̺�, Wizard
            case "���̺�":

                buffImage.sprite = Resources.Load<Sprite>(path + "Wizard/Skill01_Wiz");
                
                break;
            #endregion

            #region No.8 �ٳ׻�, Vanessa
            case "�ٳ׻�":

                buffImage.sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill01_Val");
                
                break;
                #endregion
        }
    }

}
