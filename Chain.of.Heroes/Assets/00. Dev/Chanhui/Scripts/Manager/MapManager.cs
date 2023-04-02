using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance { get; private set; }

    public MapData[] mapData;
    public int stageNum = 0;

    private void Awake()
    {

        var obj = FindObjectsOfType<MapManager>();

        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (Instance != null)
        {
            Debug.LogError("There's more than one MapManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }



}
