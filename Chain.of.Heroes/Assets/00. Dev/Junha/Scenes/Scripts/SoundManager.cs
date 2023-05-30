using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{
    #region instance 화
    public static SoundManager instance;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if (instance == null)
        {
            GameObject Entity = new GameObject("SoundManager");

            instance = Entity.AddComponent<SoundManager>();
            Entity.AddComponent<AudioSource>();
            Entity.AddComponent<AudioSource>();
            Entity.AddComponent<AudioSource>();

            DontDestroyOnLoad(Entity.gameObject);
        }
    }
    #endregion

    public AudioSource[] _audioSources;
    public AudioClip[] _audioClips;
    private const int ARRAY_SIZE = 3;

    private void Start()
    {
        _audioSources = new AudioSource[ARRAY_SIZE];
        _audioSources = GetComponents<AudioSource>();

        _audioClips = new AudioClip[ARRAY_SIZE];
    }

    private void BGMSound(string _name, float _volume, bool _loop)
    {
        _audioClips[0] = Resources.Load<AudioClip>("AudioSource/" + _name);
        _audioSources[0].clip = _audioClips[0];
        _audioSources[0].volume = _volume;
        _audioSources[0].loop = _loop;
        _audioSources[0].Play();
    }
    private void EtcSound(string _name)
    {
        _audioClips[1] = Resources.Load<AudioClip>("AudioSource/" + _name);
        _audioSources[1].clip = _audioClips[1];
        _audioSources[1].PlayOneShot(_audioClips[1]);
    }
    private void SkillSound(string _name)
    {
        _audioClips[2] = Resources.Load<AudioClip>("AudioSource/Character/" + _name);
        _audioSources[2].clip = _audioClips[2];
        _audioSources[2].PlayOneShot(_audioClips[2]);
    }



    #region [스테이지] 사운드
    public void Sound_MainSceneBGM()
    {
        BGMSound("Main_BGM", 0.2f, true);
    }
    // 시네마틱 BGM은 자체적으로 실행
    public void Sound_TutorialBGM()
    {
        BGMSound("Tutorial_BGM", 0.25f, true);
    }
    public void Sound_WorldMapBGM()
    {
        BGMSound("WorldMap_BGM", 0.25f, true);
    }
    public void Sound_TalkBGM()
    {
        BGMSound("Talk_BGM", 0.25f, true);
    }
    public void Sound_StageBGM()
    {
        BGMSound("Stage_BGM", 0.25f, true);
    }
    public void Sound_BossStageBGM()
    {
        BGMSound("BossStage_BGM", 0.375f, true);
    }
    // 시네마틱 BGM은 자체적으로 실행
    #endregion



    #region [메뉴 열림/닫힘] 사운드
    public void Sound_MenuUIOpen()
    {
        EtcSound("MenuUIOpen");
    }
    #endregion

    #region [메뉴 선택] 사운드
    public void Sound_SelectMenu()
    {
        EtcSound("SelectMenu");
    }
    #endregion

    #region [승리/패배] 사운드
    public void Sound_StageWin()
    {
        BGMSound("StageWin", 0.25f, false);
    }
    public void Sound_StageLose()
    {
        BGMSound("StageLose", 0.25f, false);
    }
    #endregion

    #region [BGM 페이드인]
    public void Sound_FadeStop()
    {
        StartCoroutine(BGMSound_FadeIn());
    }
    private IEnumerator BGMSound_FadeIn()
    {
        float time = 0f; // time부터 value까지
        float value = 1f; // FadeIn에 걸리는 시간 
        float currentVolume = _audioSources[0].volume; float targetVolume = 0f; // 현재 볼륨과 타겟 볼륨

        while (time < value)
        {
            time += Time.deltaTime;
            float temp = Mathf.Lerp(currentVolume, targetVolume, time / value);
            _audioSources[0].volume = temp;
            yield return null;
        }

        _audioSources[0].Stop();
    }
    public void Sound_ForceStop()
    {
        _audioSources[0].Stop();
    }
    #endregion



    #region [캐릭터 스킬] 사운드
    public void SwordWoman_1()
    {
        SkillSound("SwordWoman_Skill01");
    }
    public void SwordWoman_2()
    {
        SkillSound("SwordWoman_Skill02");
    }

    public void Knight_1()
    {
        SkillSound("WarrioKc_Skill01");
    }
    public void Knight_2()
    {
        SkillSound("WarrioKc_Skill02");
    }

    public void Samurai_1()
    {
        SkillSound("Samurai_Skill01");
    }
    public void Samurai_2()
    {
        SkillSound("Samurai_Skill02");
    }

    public void Archer()
    {
        SkillSound("Archer_Skill");
    }

    public void Guardian_1()
    {
        SkillSound("Guardian_Skill01");
    }
    public void Guardian_2()
    {
        SkillSound("Guardian_Skill02");
    }

    public void Priest_1()
    {
        SkillSound("Priest_Skill01");
    }
    public void Priest_2()
    {
        SkillSound("Priest_Skill02");
    }

    public void Wizard_1()
    {
        SkillSound("Wizard_Skill01");
    }
    public void Wizard_2()
    {
        SkillSound("Wizard_Skill02");
    }

    public void Valkyrie_1()
    {
        SkillSound("Valkyrie_Skill01");
    }
    public void Valkyrie_2()
    {
        SkillSound("Valkyrie_Skill02");
    }
    #endregion
}