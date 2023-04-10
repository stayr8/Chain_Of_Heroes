using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region instance화 :: Awake()함수 포함
    public static SoundManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private AudioSource _thisObject;
    private void Start()
    {
        _thisObject = GetComponent<AudioSource>();
    }

    #region ===== SelectMenu Sound =====
    private AudioClip _SelectMenu;
    public void Sound_SelectMenu()
    {
        _SelectMenu = Resources.Load<AudioClip>("SelectMenu");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion

    #region ===== WorldMap UI Open Sound =====
    public void Sound_WorldMapUIOpen()
    {
        _SelectMenu = Resources.Load<AudioClip>("WorldMapUIOpen");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion
}