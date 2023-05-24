using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_StateManager : MonoBehaviour
{
    private void Awake()
    {

    }

    public GameObject[] _unit;
    private int ARRAY_SIZE;
    private void Start()
    {
        ARRAY_SIZE = MapManager.Instance.mapData[MapManager.Instance.stageNum].Count_Unlock;
        _unit = new GameObject[ARRAY_SIZE];
        for (int i = 0; i < ARRAY_SIZE; ++i)
        {
            _unit[i] = GameObject.Find("_" + (i + 1));
        }
    }

    private void Update()
    {
        Check_Unlock();
    }

    private void Check_Unlock()
    {
        for (int i = 0; i < Mathf.Min(ARRAY_SIZE, 7); i++)
        {
            _unit[i].GetComponent<BattleReady_FormationState>().isUnlock = true;
        }
    }
}
