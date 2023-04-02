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

    public MapData mapData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        mapData = MapManager.Instance.mapData[MapManager.Instance.stageNum];

        SpawnAllPlayer();
        SpawnAllEnemy();

        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;

        PositionAllEnemy();
        PositionAllPlayer();
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if(unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
            //PositionAllEnemy();
        }
        else
        {
            friendlyUnitList.Add(unit);
            //PositionAllPlayer();
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

    private void SpawnAllPlayer()
    {
        for (int i = 0; i < mapData.Player_pf.Length; i++)
        {
            if (mapData.Player_pf[i] != null)
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

    private void PositionAllPlayer()
    {
        for (int i = 0; i < mapData.Player_pf.Length; i++)
        {
            Vector3 pos = new Vector3(mapData.PlayerXY[i].x, 0, mapData.PlayerXY[i].y);
            Debug.Log(mapData.Player_pf[i].transform.position);
            if (mapData.Player_pf[i].transform.position != pos)
            {
                //Debug.Log("플레이어 이동");
                mapData.Player_pf[i].GetComponent<Unit>().transform.position = pos;
            }
        }  
    }

    private void PositionAllEnemy()
    {
        for (int i = 0; i < mapData.Enemy_pf.Length; i++)
        {
            Vector3 pos = new Vector3(mapData.EnemyXY[i].x, 0, mapData.EnemyXY[i].y);
            Debug.Log(mapData.Enemy_pf[i].transform.position);
            if (mapData.Enemy_pf[i].transform.position != pos)
            {
                //Debug.Log("몬스터 이동");
                mapData.Enemy_pf[i].GetComponent<Unit>().transform.position = pos;
            }
        }
    }

    public void DestroyUnitList()
    {
        for(int i = 0; i < unitList.Count; i++)
        {
            if(unitList[i] != null)
            {
                unitList.Remove(unitList[i]);
            }
            //unitList[i] = null;
        }
    }
    public void DestroyfriendlyList()
    {
        for (int i = 0; i < friendlyUnitList.Count; i++)
        {
            if (friendlyUnitList[i] != null)
            {
                friendlyUnitList.Remove(friendlyUnitList[i]);
                LevelGrid.Instance.RemoveUnitAtGridPosition(friendlyUnitList[i].GetGridPosition(), friendlyUnitList[i]);
            }
            //friendlyUnitList[i] = null;
        }
    }
    public void DestroyEnemyList()
    {
        for (int i = 0; i < enemyUnitList.Count; i++)
        {
            if (enemyUnitList[i] != null)
            {
                enemyUnitList.Remove(enemyUnitList[i]);
                LevelGrid.Instance.RemoveUnitAtGridPosition(enemyUnitList[i].GetGridPosition(), enemyUnitList[i]);
            }
            //enemyUnitList[i] = null;
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
