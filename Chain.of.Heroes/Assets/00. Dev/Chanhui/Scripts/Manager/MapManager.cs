using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance = null;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if (Instance == null)
        {
            GameObject Entity = new GameObject("MapManager");

            Instance = Entity.AddComponent<MapManager>();
            
            DontDestroyOnLoad(Entity.gameObject);
        }
    }

    public List<MapData> mapData = new List<MapData>();
    public int stageNum = 0;

    private void Start()
    {
        MapDataInitialize();
    }

    public void MapDataInitialize()
    {
        mapData.Add(Resources.Load<MapData>("Stage_0"));
        mapData.Add(Resources.Load<MapData>("Stage_1"));
        mapData.Add(Resources.Load<MapData>("Stage_2"));
        mapData.Add(Resources.Load<MapData>("Stage_3"));
        mapData.Add(Resources.Load<MapData>("Stage_4"));
        mapData.Add(Resources.Load<MapData>("Stage_5"));
        mapData.Add(Resources.Load<MapData>("Stage_6"));
        mapData.Add(Resources.Load<MapData>("Stage_7"));
        mapData.Add(Resources.Load<MapData>("Stage_8"));
        mapData.Add(Resources.Load<MapData>("Stage_9"));
        mapData.Add(Resources.Load<MapData>("Stage_10"));
    }
}