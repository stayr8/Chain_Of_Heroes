using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region instanceȭ
    public static SoundManager instance;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if (instance == null)
        {
            GameObject Entity = new GameObject("SoundManager");

            instance = Entity.AddComponent<SoundManager>();
            Entity.AddComponent<AudioSource>();

            DontDestroyOnLoad(Entity.gameObject);
        }
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
        if(_SelectMenu != null)
        {
            _thisObject.PlayOneShot(_SelectMenu);
        }
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