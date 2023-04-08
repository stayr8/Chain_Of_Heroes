using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    // �̱��� ������ ����ϱ� ���� �ν��Ͻ� ����
    private static MapManager _instance;

    // �ν��Ͻ��� �����ϱ� ���� ������Ƽ
    public static MapManager Instance 
    { 
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(MapManager)) as MapManager;

                if(_instance == null)
                {
                    Debug.Log("no Singleton obj");
                }
            }
            return _instance;
        }
    }

    public MapData[] mapData;
    public int stageNum = 0;

    private void Awake()
    {

        if (_instance == null)
        {
            _instance = this;
        }
        // �ν��Ͻ��� �����ϴ� ��� ���� ����� �ν��Ͻ��� �����Ѵ�.
        else
        {
            Destroy(gameObject);
        }
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
        DontDestroyOnLoad(gameObject); 
    }



}
