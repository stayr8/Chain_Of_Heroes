using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;

    private GridPosition gridPosition;

    public MapData mapData;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {

        SpawnAllPlayer();
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
            Debug.Log("몬스터");
            enemyUnitList.Add(unit);
        }
        else
        {
            Debug.Log("플레이어");
            friendlyUnitList.Add(unit);
        }
        PositionAllPlayer();
        PositionAllEnemy();
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

    private void SpawnAllPlayer()
    {
        for (int i = 0; i < mapData.Player_pf.Length; i++)
        {
            if (true)
            {
                SpawnSinglePlayer(i);
            }
        }
    }

    private Unit SpawnSinglePlayer(int i)
    {
        Unit cp = Instantiate(mapData.Player_pf[i], transform).GetComponent<Unit>();

        return cp;
    }

    private void SpawnAllEnemy()
    {
        for (int i = 0; i < mapData.Enemy_pf.Length; i++)
        {
            if (mapData.Player_pf[i] != null)
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

    private void PositionAllPlayer()
    {
        for (int i = 0; i < friendlyUnitList.Count; i++)
        {
            Debug.Log("들어옴_2?");
            // if문 정의하기
            if (friendlyUnitList[i] != null)
            {
                Debug.Log("들어옴_1?");
                Vector3 pos = new Vector3(mapData.PlayerXY[i].x, 0, mapData.PlayerXY[i].y);
                friendlyUnitList[i].SetPosition(pos);
            }
        }
        
    }

    private void PositionAllEnemy()
    {
        for (int i = 0; i < enemyUnitList.Count; i++)
        {
            Debug.Log("들어옴_2?");
            // if문 정의하기
            if (enemyUnitList[i] != null)
            {
                Debug.Log("들어옴_2?");
                Vector3 pos = new Vector3(mapData.EnemyXY[i].x, 0, mapData.EnemyXY[i].y);
                enemyUnitList[i].SetPosition(pos);
            }
        }

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

    public bool VictoryPlayer()
    {
        return enemyUnitList.Count <= 0;
    }

    public bool VictoryEnemy()
    {
        return friendlyUnitList.Count <= 0;
    }
}
