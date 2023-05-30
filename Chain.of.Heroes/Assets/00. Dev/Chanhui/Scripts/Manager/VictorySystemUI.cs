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

    [SerializeField] private Transform LevelupPlayerPrefab;
    [SerializeField] private Transform LevelupPlayersTransform;

    private Unit _mvpPlayer;

    private List<Binding> Binds = new List<Binding>();

    private bool _gameClear;
    private bool _gameFall;

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsTurnEnd", (object value) =>
        {
            if (TurnSystem.Property.IsTurnEnd)
            {
                if (!InGame_UIManager.instance.GetIsinGameFall())
                {
                    if (UnitManager.Instance.VictoryPlayer())
                    {
                        _gameClear = true;
                        Time.timeScale = 1.0f;
                        SoundManager.instance.Sound_ForceStop();
                        SoundManager.instance.Sound_StageWin();
                        MVPSelectPlayer();
                        Set_NameAndImage();
                        mvpPlayerText.text = "" + _mvpPlayer.GetCharacterDataManager().m_name.ToString();
                        turnNumberText.text = "" + TurnSystem.Property.TurnNumber;

                        StageManager.instance.StageClear(MapManager.Instance.stageNum - 1);

                        UnitManager.Instance.OnDestroys();
                    }
                    else if (UnitManager.Instance.VictoryEnemy())
                    {
                        _gameFall = true;
                        Time.timeScale = 1.0f;
                        SoundManager.instance.Sound_ForceStop();
                        SoundManager.instance.Sound_StageLose();
                        MVPSelectPlayer();
                        Set_NameAndImage();
                        mvpPlayerText.text = "" + _mvpPlayer.GetCharacterDataManager().m_name.ToString();
                        turnNumberText.text = "" + TurnSystem.Property.TurnNumber;

                        UnitManager.Instance.OnDestroys();

                    }
                }
                else
                {
                    Time.timeScale = 1.0f;
                    SoundManager.instance.Sound_ForceStop();
                    SoundManager.instance.Sound_StageLose();
                    MVPSelectPlayer();
                    Set_NameAndImage();
                    mvpPlayerText.text = "" + _mvpPlayer.GetCharacterDataManager().m_name.ToString();
                    turnNumberText.text = "" + TurnSystem.Property.TurnNumber;

                    Debug.Log("짐");


                    UnitManager.Instance.OnDestroys();
                    //_gameClear = true;
                }
            }

        }, false);
        Binds.Add(Bind);

        _gameClear = false;
        _gameFall = false;
    }

    private void Update()
    {
        
        if(_gameClear || _gameFall)
        {
            if (InputManager.Instance.IsMouseButtonDown())
            {
                
                StartCoroutine(LoadScene());
            }
        }
    }

    private List<Unit> playerUnit;
    private List<Unit> deadplayerUnit;
    private void MVPSelectPlayer()
    {
        playerUnit = UnitManager.Instance.GetFriendlyUnitList();
        if (UnitManager.Instance.GetPlayerDeadList() != null)
        {
            deadplayerUnit = UnitManager.Instance.GetPlayerDeadList();
        }

        Debug.Log(playerUnit.Count);
        for (int i = playerUnit.Count; i > 0; i--)
        {
            Unit unit = playerUnit[i - 1];

            if (playerUnit.Contains(unit))
            {
                if (_gameClear)
                {
                    unit.GetCharacterDataManager().m_currentExp = MapManager.Instance.mapData[MapManager.Instance.stageNum].Clear_Exp;
                    DataUpdate(unit.GetCharacterDataManager());
                    Set_LevelUPImage(unit);

                }

                if (_mvpPlayer == null)
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

        for (int i = deadplayerUnit.Count; i > 0; i--)
        {
            Unit unit = deadplayerUnit[i - 1];

            if (deadplayerUnit.Contains(unit))
            {
                if (_gameClear)
                {
                    unit.GetCharacterDataManager().m_currentExp = MapManager.Instance.mapData[MapManager.Instance.stageNum].Clear_Exp;
                    DataUpdate(unit.GetCharacterDataManager());
                    Set_LevelUPImage(unit);

                }

                if (_mvpPlayer == null)
                {
                    _mvpPlayer = unit;
                }

                if (_mvpPlayer.GetKillCount() >= unit.GetKillCount())
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
        Debug.Log(_mvpPlayer);
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

    private void Set_LevelUPImage(Unit unit)
    {
        CharacterDataManager data = unit.GetCharacterDataManager();

        if (data.m_currentExp > data.m_maxExp)
        {
            Transform actionButtonTransform = Instantiate(LevelupPlayerPrefab, LevelupPlayersTransform);
            Image CharacterUI = actionButtonTransform.GetComponent<Image>();
            
            switch (data.m_name)
            {
                case "아카메": // _1
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_SwordWoman");
                    break;

                case "크리스": // _2
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_Night");
                    break;

                case "카미나": // _3
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_Samurai");
                    break;

                case "멜리사": // _4
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_Archer");
                    break;

                case "플라틴": // _5
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_Guardian");
                    break;

                case "아이네": // _6
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_Priest");
                    break;

                case "제이브": // _7
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_Wizard");
                    break;

                case "바네사": // _8
                    CharacterUI.sprite = Resources.Load<Sprite>("SD_Valkyrie");
                    break;
            }

            CharacterUI.SetNativeSize();
        }
    }

    private void DataUpdate(CharacterDataManager cdm)
    {
        CharacterDataManager[] _initCDM = DataManager.Instance.GetInitCDM();
        for (int i = 0; i < 8; i++)
        {
            if (_initCDM[i].CharacterName == cdm.CharacterName)
            {
                _initCDM[i].NumForLvUp = cdm.NumForLvUp;
                _initCDM[i].m_currentExp = cdm.m_currentExp;
            }
        }
    }

    private void DestroyActionButton()
    {
        foreach (Transform buttonTransform in LevelupPlayersTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(1f);
        DestroyActionButton();
        yield return new WaitForSeconds(0.1f);
        LoadingSceneController.LoadScene("WorldMapScene");
    }

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }

}
