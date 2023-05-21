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
        //// �ν��Ͻ��� �����ϴ� ��� ���� ����� �ν��Ͻ��� �����Ѵ�.
        //else
        //{
        //    Destroy(gameObject);
        //}
        //// �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        //DontDestroyOnLoad(gameObject); 
    }



}
