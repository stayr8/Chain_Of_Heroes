using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{

    private Image Image;

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
                break;
            case 1:
                Image.sprite = GameAssets.i.characterUI[1];
                break;
            case 2:
                Image.sprite = GameAssets.i.characterUI[2];
                break;
            case 3:
                Image.sprite = GameAssets.i.characterUI[3];
                break;
            
        }
    }

}
