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
    public int stageNum = 0;

    //���������� �ʱ�ȭ

    public void MapDataInitialize()
    {
        mapData.Add(Resources.Load<MapData>("Stage_1")); // stageNum = 0;
        mapData.Add(Resources.Load<MapData>("Stage_2")); // stageNum = 1;
        mapData.Add(Resources.Load<MapData>("Stage_3")); // stageNum = 2;
        mapData.Add(Resources.Load<MapData>("Stage_4")); // stageNum = 3;
        mapData.Add(Resources.Load<MapData>("Stage_5")); // stageNum = 4;
        mapData.Add(Resources.Load<MapData>("Stage_6")); // stageNum = 5;
        mapData.Add(Resources.Load<MapData>("Stage_7")); // stageNum = 6;
        mapData.Add(Resources.Load<MapData>("Stage_8")); // stageNum = 7;
        mapData.Add(Resources.Load<MapData>("Stage_9")); // stageNum = 8;
        mapData.Add(Resources.Load<MapData>("Stage_10")); // stageNum = 9;
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
    //    //// �ν��Ͻ��� �����ϴ� ��� ���� ����� �ν��Ͻ��� �����Ѵ�.
    //    //else
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //    //// �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
    //    //DontDestroyOnLoad(gameObject); 
    //}



}
