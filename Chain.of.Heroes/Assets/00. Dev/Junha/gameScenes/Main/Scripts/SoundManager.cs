using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region instanceȭ :: Awake()�Լ� ����
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

    #region ===== Battle Sound =====
    public void Sound_Battle()
    {
        _SelectMenu = Resources.Load<AudioClip>("BattleSound");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion

    #region ===== Win / Lose Sound =====
    public void Sound_StageWin()
    {
        _thisObject.Stop();

        _SelectMenu = Resources.Load<AudioClip>("StageWin");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    public void Sound_StageLose()
    {
        _thisObject.Stop();

        _SelectMenu = Resources.Load<AudioClip>("StageLose");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion
}