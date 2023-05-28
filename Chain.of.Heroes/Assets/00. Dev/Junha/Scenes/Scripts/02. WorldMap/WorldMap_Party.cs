using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class WorldMap_Party : MonoBehaviour
{
    [SerializeField] List<Image> Characters;

    public void ForceUpdate()
    {
        foreach(var character in Characters) 
        {
            var Data = character.GetComponent<CharacterDataManager>();
            if (Data)
            {
                if(Data.m_UnlockMapID <= StageManager.instance.ClearID)
                {
                    Unlock(Data, character);
                }
                else
                {
                    Lock(Data, character);
                }
            }
        }
    }

    private void Unlock(CharacterDataManager Data, Image Character)
    {
        switch (Data.m_name)
        {
            case "��ī��":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Swordsman(F)");
                break;
            case "ũ����":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Knight");
                break;
            case "ī�̳�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Samurai");
                break;
            case "�Ḯ��":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Archer(F)");
                break; ;
            case "�ö�ƾ":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Guardian");
                break; ;
            case "���̳�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Priest(F)");
                break;
            case "���̺�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Wizard(M)");
                break;
            case "�ٳ׻�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Valkyrie");
                break;
        }
    }

    private void Lock(CharacterDataManager Data, Image Character)
    {
        switch (Data.m_name)
        {
            case "��ī��":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Swordsman(F)_Lock");
                break;
            case "ũ����":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Knight_Lock");
                break;
            case "ī�̳�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Samurai_Lock");
                break;
            case "�Ḯ��":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Archer(F)_Lock");
                break; ;
            case "�ö�ƾ":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Guardian_Lock");
                break; ;
            case "���̳�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Priest(F)_Lock");
                break;
            case "���̺�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Wizard(M)_Lock");
                break;
            case "�ٳ׻�":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Valkyrie_Lock");
                break;
        }
    }
}
