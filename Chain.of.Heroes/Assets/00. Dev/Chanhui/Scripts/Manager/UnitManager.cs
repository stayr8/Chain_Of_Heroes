using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if (Instance == null)
        {
            GameObject Entity = new GameObject("UnitManager");

            Instance = Entity.AddComponent<UnitManager>();

            DontDestroyOnLoad(Entity.gameObject);
        }
    }


    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;
    private List<CharacterUI> characterUiList;

    public MapData mapData;

    [SerializeField] private GameObject[] Character;

    public int playerpos = 0;
    public int enemypos = 0;
    private bool OnChangeFormation;

    private void Awake()
    {
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        characterUiList = new List<CharacterUI>();
        OnChangeFormation = false;
    }

    private void Start()
    {
        // 임시로 저장
        mapData = MapManager.Instance.mapData[0];

        playerpos = 0;
        enemypos = 0;

        characterUiList = ChangeFormationSystem.Instance.GetCharacterUIList();

        //SpawnAllPlayer();
        SpawnAllEnemy();

        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
 
    }


    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if(unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
            unit.SetPosition(mapData.EnemyXY[enemypos]);
            enemypos++;

        }
        else
        {
            friendlyUnitList.Add(unit);
            //Debug.Log(characterUiList);
            if(!OnChangeFormation)
            {
                unit.SetPosition(ChangeFormationSystem.Instance.GetCharacterMovePos()[playerpos]);
            }
            else
            {
                unit.SetPosition(characterUiList[playerpos].GetCharacterUIMovePos());
            }
            
            playerpos++;

        } 
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }
    }


    public void SpawnAllPlayer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (CharacterTypeManager.Instance.GetIsCharacter()[i] == true)
            {
                SpawnSinglePlayer(i);
            }
        }
    }

    private Unit SpawnSinglePlayer(int i)
    {
        Unit cp = Instantiate(Character[i], transform).GetComponent<Unit>();

        return cp;
    }

    private void SpawnAllEnemy()
    {
        for (int i = 0; i < mapData.Enemy_pf.Length; i++)
        {
            if (mapData.Enemy_pf[i] != null)
            {
                SpawnSingleEnemy(i);
            }
        }
    }

    private Unit SpawnSingleEnemy(int i)
    {
        Unit cp = Instantiate(mapData.Enemy_pf[i], transform).GetComponent<Unit>();

        return cp;
    }
   


    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

    public Unit GetFriendlyfristUnit()
    {
        return friendlyUnitList[0];
    }

    public bool VictoryPlayer()
    {
        return enemyUnitList.Count <= 0;
    }

    public bool VictoryEnemy()
    {
        return friendlyUnitList.Count <= 0;
    }

    public void SetOnChangeFormation(bool OnChangeFormation)
    {
        this.OnChangeFormation =  OnChangeFormation;
    }

    private void OnDisable()
    {
        Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }
}
