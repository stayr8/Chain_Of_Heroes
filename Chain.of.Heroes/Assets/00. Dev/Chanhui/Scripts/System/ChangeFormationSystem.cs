using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFormationSystem : MonoBehaviour
{
    public static ChangeFormationSystem Instance { get; private set; }


    private List<CharacterUI> characterUIList;
    [SerializeField] private Transform CharacterUIPrefab;
    [SerializeField] private Transform[] Characterpos;

    //[SerializeField] private int number;

    private bool[] isImage;

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
        for(int i = 0; i < 9; i++)
        {
            isImage[i] = false;
        }
    }

    private void Start()
    {

        BattleReady_UIManager.instance.OnCharacterChangeFormation += BattleReady_UIManager_OnCharacterChangeFormation;
        //CreateCharacterUI(number, 8);
    }

    public void CreateCharacterUI(int Charnumber, int pos)
    {
        
        Transform CharacterTransform = Instantiate(CharacterUIPrefab, Characterpos[pos]);
        CharacterUI CharacterUI = CharacterTransform.GetComponent<CharacterUI>();
        CharacterUI.SelectedImage(Charnumber);
        CharacterUI.GetComponent<Image>().SetNativeSize();

        characterUIList.Add(CharacterUI);

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

        characterUIList.Clear();
    }

    public void SingleDestroyCharacterUI(CharacterTypeManager.CharacterType type)
    {
        foreach (CharacterUI buttonTransform in characterUIList)
        {
            if (buttonTransform.ImageType() == type)
            {
                Debug.Log(type);
                Destroy(buttonTransform.gameObject);
                
            }
        }

        characterUIList.Clear();
        isImage[(int)type] = false;
    }

    public void BattleReady_UIManager_OnCharacterChangeFormation(object sender, EventArgs e)
    {
        Unit_formation();
    }


    private void Unit_formation()
    {
        for(int i = 0; i < CharacterTypeManager.Instance.GetIsCharacter().Length; i++)
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

    public bool[] GetIsImage()
    {
        return isImage;
    }

}
