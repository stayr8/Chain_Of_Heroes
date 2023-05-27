using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region instance화
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

    #region [월드맵 메뉴 열림/닫힘] 사운드
    public void Sound_WorldMapUIOpen()
    {
        _SelectMenu = Resources.Load<AudioClip>("AudioSource/WorldMapUIOpen");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion

    #region [메뉴 선택] 사운드
    private AudioClip _SelectMenu;
    public void Sound_SelectMenu()
    {
        _SelectMenu = Resources.Load<AudioClip>("AudioSource/SelectMenu");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion

    #region [인게임 전투] 사운드
    public void Sound_Battle()
    {
        _SelectMenu = Resources.Load<AudioClip>("AudioSource/BattleSound");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion

    #region [승리/패배] 사운드
    public void Sound_StageWin()
    {
        _thisObject.Stop();

        _SelectMenu = Resources.Load<AudioClip>("AudioSource/StageWin");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    public void Sound_StageLose()
    {
        _thisObject.Stop();

        _SelectMenu = Resources.Load<AudioClip>("AudioSource/StageLose");

        _thisObject.PlayOneShot(_SelectMenu);
    }
    #endregion
}