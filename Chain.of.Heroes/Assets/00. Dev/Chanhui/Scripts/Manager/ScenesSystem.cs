using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesSystem : MonoBehaviour
{
    public static ScenesSystem Instance { get; private set; }

    public event EventHandler OnScenesChange;

    [SerializeField] TurnSystem turnSystem;

    [SerializeField] private GameObject ChanScene;
    [SerializeField] private GameObject JunScene;
    [SerializeField] private GameObject camSkybox;

    [SerializeField] private Material[] _skyboxmaterial;

    [SerializeField] private bool isInGame;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ScenesManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        camSkybox = GameObject.Find("Main Camera");

        //MapManager.Instance.MapDataInitialize();
        if (MapManager.Instance.stageNum == 1 || MapManager.Instance.stageNum == 2 || MapManager.Instance.stageNum == 3)
        {
            Debug.Log("여기서는 들어오잖아!");
            GameObject map = Resources.Load<GameObject>("Map/Region01");
            Instantiate(map);

            camSkybox.AddComponent<Skybox>().material = _skyboxmaterial[0];
        }
        else if (MapManager.Instance.stageNum == 4 || MapManager.Instance.stageNum == 5 || MapManager.Instance.stageNum == 6)
        {
            GameObject map = Resources.Load<GameObject>("Map/Region02");
            Instantiate(map);

            camSkybox.AddComponent<Skybox>().material = _skyboxmaterial[1];
        }
        else if (MapManager.Instance.stageNum == 7 || MapManager.Instance.stageNum == 8 )
        {
            GameObject map = Resources.Load<GameObject>("Map/Region03");
            Instantiate(map);

            camSkybox.AddComponent<Skybox>().material = _skyboxmaterial[2];
        }
        else if (MapManager.Instance.stageNum == -1 || MapManager.Instance.stageNum == 9 || MapManager.Instance.stageNum == 10)
        {
            GameObject map = Resources.Load<GameObject>("Map/Region04");
            Instantiate(map);

            camSkybox.AddComponent<Skybox>().material = _skyboxmaterial[3];
        }

        UnitManager.Instance.UnitInitialize();
        turnSystem.Initialize();

        OnScenesChange += ScenesManager_OnScenesChange;
        isInGame = false;
    }

    public void ScenesChange()
    {
        OnScenesChange?.Invoke(this, EventArgs.Empty);
    }

    private void ScenesManager_OnScenesChange(object sender, EventArgs e)
    {
        ChanScene.SetActive(true);
        isInGame = true;
        Invoke("Time", 0.5f);
    }

    void Time()
    {
        JunScene.SetActive(false);
    }

    public bool GetIsInGame()
    {
        return isInGame;
    }

    private void OnDisable()
    {
        OnScenesChange -= ScenesManager_OnScenesChange;
    }
}
