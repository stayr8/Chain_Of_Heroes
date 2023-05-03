using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{

    private Image Image;
    [SerializeField] private CharacterTypeManager.CharacterType characterType;

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
                characterType = CharacterTypeManager.CharacterType.Night;
                break;
            case 2:
                Image.sprite = GameAssets.i.characterUI[2];
                characterType = CharacterTypeManager.CharacterType.samurai;
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

}
