using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    public event EventHandler OnScenesChange;

    [SerializeField] private GameObject ChanScene;
    [SerializeField] private GameObject JunScene;

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
    }

    public void ScenesChange()
    {
        OnScenesChange?.Invoke(this, EventArgs.Empty);
    }

    private void ScenesManager_OnScenesChange(object sender, EventArgs e)
    {
        ChanScene.SetActive(true);
        Invoke("Time", 0.2f);
    }

    void Time()
    {
        JunScene.SetActive(false);
    }
}
