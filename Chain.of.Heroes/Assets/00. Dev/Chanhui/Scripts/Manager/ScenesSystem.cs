using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesSystem : MonoBehaviour
{
    public static ScenesSystem Instance { get; private set; }

    public event EventHandler OnScenesChange;

    [SerializeField] private GameObject ChanScene;
    [SerializeField] private GameObject JunScene;

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
