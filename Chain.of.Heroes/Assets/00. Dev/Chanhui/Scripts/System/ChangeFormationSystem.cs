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

    [SerializeField] private int number;

    private void Awake()
    {
        characterUIList = new List<CharacterUI>();
    }

    private void Start()
    {

        BattleReady_UIManager.instance.OnCharacterChangeFormation += BattleReady_UIManager_OnCharacterChangeFormation;
        //CreateCharacterUI(number, 8);
    }

    public void CreateCharacterUI(int Charnumber, int pos)
    {
        //DestroyCharacterUI();

        Transform CharacterTransform = Instantiate(CharacterUIPrefab, Characterpos[pos]);
        CharacterUI CharacterUI = CharacterTransform.GetComponent<CharacterUI>();
        CharacterUI.SelectedImage(Charnumber);
        CharacterUI.GetComponent<Image>().SetNativeSize();

        characterUIList.Add(CharacterUI);
        
    }

    public void DestroyCharacterUI()
    {
        foreach (CharacterUI buttonTransform in characterUIList)
        {
            Destroy(buttonTransform.gameObject);
        }

        characterUIList.Clear();
    }

    public void BattleReady_UIManager_OnCharacterChangeFormation(object sender, EventArgs e)
    {
        Debug.Log("½ÇÇà");
        CreateCharacterUI(number, 1);
        CreateCharacterUI(number, 3);
        CreateCharacterUI(number, 2);
    }

}
