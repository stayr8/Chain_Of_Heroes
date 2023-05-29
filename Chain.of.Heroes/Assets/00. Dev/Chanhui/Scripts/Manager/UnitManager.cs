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

    [SerializeField] private List<GameObject> Character;
    private List<Binding> Binds = new List<Binding>();

    public int playerpos;
    public int enemypos;
    private bool OnChangeFormation;

    private void Start()
    {
        playerpos = 0;
        enemypos = 0;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        characterUiList = new List<CharacterUI>();
        OnChangeFormation = false;
        Character = new List<GameObject>();

        Character.Add(Resources.Load<GameObject>("SwordWoman"));
        Character.Add(Resources.Load<GameObject>("Knight"));
        Character.Add(Resources.Load<GameObject>("Samurai"));
        Character.Add(Resources.Load<GameObject>("Archer"));
        Character.Add(Resources.Load<GameObject>("Guardian"));
        Character.Add(Resources.Load<GameObject>("Priest"));
        Character.Add(Resources.Load<GameObject>("Wizard"));
        Character.Add(Resources.Load<GameObject>("Valkyrie"));

        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;

        Binding Binded = BindingManager.Bind(TurnSystem.Property, "IsTurnEnd", (object value) =>
        {
            if(BattleReady_UIManager.instance.GetSceneback() && TurnSystem.Property.IsTurnEnd)
            {
                UnitInit();
                BattleReady_UIManager.instance.SetSceneback(false);
            }
            else if (TurnSystem.Property.IsTurnEnd)
            {
                Invoke("UnitInit", 2f);
                Debug.Log("Bind Entered");
            }
        },false);
        Binds.Add(Binded);
 
    }

    public void UnitInitialize()
    {

        // 임시로 저장
        mapData = MapManager.Instance.mapData[MapManager.Instance.stageNum];

        playerpos = 0;
        enemypos = 0;

        characterUiList = ChangeFormationSystem.Instance.GetCharacterUIList();

        //SpawnAllPlayer();
        SpawnAllEnemy();
    }


    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if(unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
            unit.GetMonsterDataManager().StageLevel = mapData.Stage_MonsterLV;
            unit.SetPosition(mapData.EnemyXY[enemypos]);
            enemypos++;

        }
        else
        {
            friendlyUnitList.Add(unit);
            //Debug.Log(playerpos);
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

    private void UnitInit()
    {
        Debug.Log("Unit Init Entered");
        
        
        for (int i = unitList.Count; i > 0; i--)
        {

            Unit unitToRemove = unitList[i - 1];

            if (unitList.Contains(unitToRemove))
            {
                unitList.Remove(unitToRemove);
            }

            if (friendlyUnitList.Contains(unitToRemove))
            {
                friendlyUnitList.Remove(unitToRemove);
            }

            if (enemyUnitList.Contains(unitToRemove))
            {
                enemyUnitList.Remove(unitToRemove);
            }
            Destroy(unitToRemove.gameObject);
        }

        unitList.Clear();
        friendlyUnitList.Clear();
        enemyUnitList.Clear();
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

    public void OnDestroys()
    {
        playerpos = 0;
        enemypos = 0;
    }
    

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
        
        Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
    }
}
