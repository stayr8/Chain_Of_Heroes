using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{

    [SerializeField] private Image image;
    [SerializeField] private CharacterTypeManager.CharacterType characterType;
    [SerializeField] private int UIPosNumber;
    [SerializeField] private Vector3 characterUimovepos;

    private CharacterDataManager _cdm;

    private void Awake()
    {
        //image = GetComponent<Image>();
    }

    private void Start()
    {
        _cdm = GetComponent<CharacterDataManager>();

        CharacterDataManager[] _initCDM = DataManager.Instance.GetInitCDM();
        for (int i = 0; i < 8; i++)
        {
            if (_initCDM[i].CharacterName == _cdm.CharacterName)
            {
                _cdm.NumForLvUp = _initCDM[i].NumForLvUp;
                _cdm.m_currentExp = _initCDM[i].m_currentExp;
            }
        }
    }


    public CharacterTypeManager.CharacterType ImageType()
    {
        return characterType;
    }

    public int GetCharUIpos()
    {
        return UIPosNumber;
    }

    public void SetCharUIpos(int UIPosNumber)
    {
        this.UIPosNumber = UIPosNumber;
    }

    public Image GetCharacterUI()
    {
        return image;
    }

    public void SetCharacterUI(Sprite image)
    {
        this.image.sprite = image;
    }

    public Vector3 GetCharacterUIMovePos()
    {
        return characterUimovepos;
    }

    public void SetCharacterUIMovePos(Vector3 characterUimovepos)
    {
        this.characterUimovepos = characterUimovepos;
    }
}
