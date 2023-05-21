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

    private void Awake()
    {
        mapData.Add(Resources.Load<MapData>("Stage_1"));
        mapData.Add(Resources.Load<MapData>("Stage_2"));
        //if (Instance == null)
        //{
        //    Instance = this;
        //}
        //// 인스턴스가 존재하는 경우 새로 생기는 인스턴스를 삭제한다.
        //else
        //{
        //    Destroy(gameObject);
        //}
        //// 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        //DontDestroyOnLoad(gameObject); 
    }



}
