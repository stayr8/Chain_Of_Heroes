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
                    Debug.Log("Unlock");
                }
                else
                {
                    Lock(Data, character);
                    Debug.Log("lock");
                }
            }
        }
    }

    private void Unlock(CharacterDataManager Data, Image Character)
    {
        switch (Data.m_name)
        {
            case "아카메":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Swordsman(F)");
                break;
            case "크리스":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Knight");
                break;
            case "카미나":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Samurai");
                break;
            case "멜리사":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Archer(F)");
                break; ;
            case "플라틴":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Guardian");
                break; ;
            case "아이네":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Priest(F)");
                break;
            case "제이브":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Wizard(M)");
                break;
            case "바네사":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Valkyrie");
                break;
        }
    }

    private void Lock(CharacterDataManager Data, Image Character)
    {
        switch (Data.m_name)
        {
            case "아카메":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Swordsman(F)_Lock");
                break;
            case "크리스":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Knight_Lock");
                break;
            case "카미나":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Samurai_Lock");
                break;
            case "멜리사":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Archer(F)_Lock");
                break; ;
            case "플라틴":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Guardian_Lock");
                break; ;
            case "아이네":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Priest(F)_Lock");
                break;
            case "제이브":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Wizard(M)_Lock");
                break;
            case "바네사":
                Character.sprite = Resources.Load<Sprite>("Party/Info_Valkyrie_Lock");
                break;
        }
    }
}
