using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFormationSystem : MonoBehaviour
{
    public static ChangeFormationSystem Instance { get; private set; }


    private List<CharacterUI> characterUIList;
    private bool[] isImage;

    [SerializeField] private Transform[] Characterpos;
    [SerializeField] private Vector3[] CharacterMovePos;
    private bool[] isGround;

    [SerializeField] private Transform CharacterUIPrefab;


    //private bool OnChangeFormation;

    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        characterUIList = new List<CharacterUI>();

        isImage = new bool[9];
        isGround = new bool[12];
    }

    private void Start()
    {
        BattleReady_UIManager.instance.OnCharacterChangeFormation += BattleReady_UIManager_OnCharacterChangeFormation;
    }

    public void CreateCharacterUI(int Charnumber, int pos)
    {
        
        Transform CharacterTransform = Instantiate(CharacterUIPrefab, Characterpos[pos]);
        CharacterUI CharacterUI = CharacterTransform.GetComponent<CharacterUI>();
        CharacterUI.SelectedImage(Charnumber);
        CharacterUI.SetCharUIpos(pos);
        CharacterUI.SetCharacterUIMovePos(CharacterMovePos[pos]);
        Debug.Log(CharacterMovePos[pos]);
        CharacterUI.GetComponent<Image>().SetNativeSize();

        characterUIList.Add(CharacterUI);
        characterUIList.Sort(new CharacterTypeComparer());
        isGround[pos] = true;
        isImage[Charnumber] = true;
    }

    public void AnyDestroyCharacterUI()
    {
        foreach (CharacterUI buttonTransform in characterUIList)
        {
            Destroy(buttonTransform.gameObject);
        }

        for(int i = 0; i < isImage.Length; i++)
        {
            isImage[i] = false;
        }
        for (int i = 0; i < isGround.Length; i++)
        {
            isGround[i] = false;
        }

        characterUIList.Clear();
    }


    public void SingleDestroyCharacterUI(CharacterUI charui, int type, int pos)
    {
        foreach (CharacterUI buttonTransform in characterUIList)
        {
            if ((int)buttonTransform.ImageType() == type)
            {
                Debug.Log(type);
                Destroy(buttonTransform.gameObject);
                
            }
        }

        isImage[(int)type] = false;
        isGround[pos] = false;

        characterUIList.Remove(charui);
    }

    public void BattleReady_UIManager_OnCharacterChangeFormation(object sender, EventArgs e)
    {
        Unit_formation();
    }


    private void Unit_formation()
    {
        UnitManager.Instance.SetOnChangeFormation(true);
        for (int i = 0; i < CharacterTypeManager.Instance.GetIsCharacter().Length; i++)
        {
            if(!isImage[0] && CharacterTypeManager.Instance.GetIsCharacter()[0] == true)
            {
                CreateCharacterUI(0, i);
                isImage[0] = true;
            }
            else if(!isImage[1] && CharacterTypeManager.Instance.GetIsCharacter()[1] == true)
            {
                CreateCharacterUI(1, i);
                isImage[1] = true;
            }
            else if (!isImage[2] && CharacterTypeManager.Instance.GetIsCharacter()[2] == true)
            {
                CreateCharacterUI(2, i);
                isImage[2] = true;
            }
            else if (!isImage[3] && CharacterTypeManager.Instance.GetIsCharacter()[3] == true)
            {
                CreateCharacterUI(3, i);
                isImage[3] = true;
            }
            else if (!isImage[4] && CharacterTypeManager.Instance.GetIsCharacter()[4] == true)
            {
                CreateCharacterUI(4, i);
                isImage[4] = true;
            }
            else if (!isImage[5] && CharacterTypeManager.Instance.GetIsCharacter()[5] == true)
            {
                CreateCharacterUI(5, i);
                isImage[5] = true;
            }
            else if (!isImage[6] && CharacterTypeManager.Instance.GetIsCharacter()[6] == true)
            {
                CreateCharacterUI(6, i);
                isImage[6] = true;
            }
            else if (!isImage[7] && CharacterTypeManager.Instance.GetIsCharacter()[7] == true)
            {
                CreateCharacterUI(7, i);
                isImage[7] = true;
            }
            else if (!isImage[8] && CharacterTypeManager.Instance.GetIsCharacter()[8] == true)
            {
                CreateCharacterUI(8, i);
                isImage[8] = true;
            }
            
        }
    }

    public List<CharacterUI> GetCharacterUIList()
    {
        return characterUIList;
    }

    public Transform[] GetCharacterpos()
    {
        return Characterpos;
    }
    
    public Vector3[] GetCharacterMovePos()
    {
        return CharacterMovePos;
    }

    public bool[] GetIsImage()
    {
        return isImage;
    }

    public bool[] GetIsGround()
    {
        return isGround;
    }


    private class CharacterTypeComparer : Comparer<CharacterUI>
    {
        public override int Compare(CharacterUI x, CharacterUI y)
        {
            if(x == null || y == null)
            {
                return 0;
            }

            return x.ImageType().CompareTo(y.ImageType());
        }
    }

}
