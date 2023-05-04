using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{

    private Image Image;
    [SerializeField] private CharacterTypeManager.CharacterType characterType;
    [SerializeField] private int UIPosNumber;
    [SerializeField] private Vector3 characterUimovepos;

    private void Awake()
    {
        Image = GetComponent<Image>();
    }


    public void SelectedImage(int number)
    {
        switch(number)
        {
            case 0:
                Image.sprite = GameAssets.i.characterUI[0];
                characterType = CharacterTypeManager.CharacterType.Womanknight;
                break;
            case 1:
                Image.sprite = GameAssets.i.characterUI[1];
                characterType = CharacterTypeManager.CharacterType.Knight;
                break;
            case 2:
                Image.sprite = GameAssets.i.characterUI[2];
                characterType = CharacterTypeManager.CharacterType.Samurai;
                break;
            case 3:
                Image.sprite = GameAssets.i.characterUI[3];
                characterType = CharacterTypeManager.CharacterType.Archer;
                break;
            case 4:
                Image.sprite = GameAssets.i.characterUI[4];
                characterType = CharacterTypeManager.CharacterType.Guardian;
                break;
            case 5:
                Image.sprite = GameAssets.i.characterUI[5];
                characterType = CharacterTypeManager.CharacterType.Manknight;
                break;

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

    public Vector3 GetCharacterUIMovePos()
    {
        return characterUimovepos;
    }

    public void SetCharacterUIMovePos(Vector3 characterUimovepos)
    {
        this.characterUimovepos = characterUimovepos;
    }
}
