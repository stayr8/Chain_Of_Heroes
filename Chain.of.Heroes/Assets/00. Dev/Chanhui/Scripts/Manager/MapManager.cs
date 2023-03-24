using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance = null;
    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static MapManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(MapManager)) as MapManager;

                if (_instance == null)
                {
                    Debug.Log("no Singleton obj");
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this) // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� ����
        {
            Destroy(gameObject);
        }
    }

    public MapData[] monData;
    public int stageNum = 0;

}
