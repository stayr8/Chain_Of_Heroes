using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VictorySystemUI : MonoBehaviour
{
   
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private TextMeshProUGUI mvpPlayerText;
    [SerializeField] private GameObject playerVictoryVisualGameObject;
    [SerializeField] private GameObject enemyVictoryVisualGameObject;
    [SerializeField] private Image character_Image;

    private Unit _mvpPlayer;

    private void Start()
    {
        BindingManager.Bind(TurnSystem.Property, "IsTurnEnd", (object value) =>
        {
            if (UnitManager.Instance.VictoryPlayer())
            {
                SoundManager.instance.Sound_StageWin();
                MVPSelectPlayer();
                Set_NameAndImage();
                mvpPlayerText.text = "" + _mvpPlayer.GetCharacterDataManager().m_name.ToString();
                turnNumberText.text = "" + TurnSystem.Property.TurnNumber;
            }
            else if (UnitManager.Instance.VictoryEnemy())
            {
                SoundManager.instance.Sound_StageLose();
                MVPSelectPlayer();
                Set_NameAndImage();
                mvpPlayerText.text = "" + _mvpPlayer.GetCharacterDataManager().m_name.ToString();
                turnNumberText.text = "" + TurnSystem.Property.TurnNumber;
            }
    
        });

    }

    private void Update()
    {
        /*
        if(InputManager.Instance.IsMouseButtonDown())
        {
            Invoke("LoadScene", 5f);
        }*/
    }

    private void MVPSelectPlayer()
    {
        List<Unit> playerUnit = UnitManager.Instance.GetFriendlyUnitList();

        for (int i = playerUnit.Count; i > 0; i--)
        {
            Unit unit = playerUnit[i - 1];

            if (playerUnit.Contains(unit))
            {
                if(_mvpPlayer == null)
                {
                    _mvpPlayer = unit;
                }

                if(_mvpPlayer.GetKillCount() >= unit.GetKillCount())
                {
                    continue;
                }
                else
                {
                    _mvpPlayer = unit;
                }
            }
        }
    }

    private void Set_NameAndImage()
    {
        CharacterDataManager data = _mvpPlayer.GetCharacterDataManager();

        switch (data.m_name)
        {
            case "아카메": // _1
                character_Image.sprite = Resources.Load<Sprite>("Chat_Swordsman");
                break;

            case "크리스": // _2
                character_Image.sprite = Resources.Load<Sprite>("Chat_Knight");
                break;

            case "카미나": // _3
                character_Image.sprite = Resources.Load<Sprite>("Chat_Samurai");
                break;

            case "멜리사": // _4
                character_Image.sprite = Resources.Load<Sprite>("Chat_Archer");
                break;

            case "플라틴": // _5
                character_Image.sprite = Resources.Load<Sprite>("Chat_Guardian");
                break;

            case "아이네": // _6
                character_Image.sprite = Resources.Load<Sprite>("Chat_Priest");
                break;

            case "제이브": // _7
                character_Image.sprite = Resources.Load<Sprite>("Chat_Wizard");
                break;

            case "바네사": // _8
                character_Image.sprite = Resources.Load<Sprite>("Chat_Valkyrie");
                break;
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene("WorldMapScene");
    }

}
