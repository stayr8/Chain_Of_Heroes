using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region instance ȭ
    public static DataManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one CharacterActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    [SerializeField] private CharacterDataManager[] _initcdm;

    private void Start()
    {
        _initcdm = GetComponents<CharacterDataManager>();
    }


    public CharacterDataManager[] GetInitCDM()
    {
        return _initcdm;
    }
}