using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public int stageNum = 1;

    //전투씬에만 초기화

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

    private void Start()
    {
        MapDataInitialize();
    }
    //private void Awake()
    //{

    //    //if (Instance == null)
    //    //{
    //    //    Instance = this;
    //    //}
    //    //// 인스턴스가 존재하는 경우 새로 생기는 인스턴스를 삭제한다.
    //    //else
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //    //// 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
    //    //DontDestroyOnLoad(gameObject); 
    //}



}
