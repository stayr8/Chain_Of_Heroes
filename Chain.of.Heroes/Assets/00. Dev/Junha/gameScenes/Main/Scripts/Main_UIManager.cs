using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_UIManager : MonoBehaviour
{
    [SerializeField, Header("메인 화면")] private GameObject Main;
    [SerializeField, Header("\n메뉴 화면")] private GameObject Menu;
    [SerializeField, Header("\n이어서 시작 화면")] private GameObject Continue;
    [SerializeField, Header("\n크레딧 화면")] private GameObject Credit;

    public static Main_UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // 이미지 알파값 0으로 초기화
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
    }

    private void Update()
    {
        UI_STATE();
    }

    public enum STATE { MAIN, MENU, START, CONTINUE, CREDIT }
    public STATE state = STATE.MAIN;
    private void UI_STATE()
    {
        switch (state)
        {
            case STATE.MAIN:
                if (Input.anyKeyDown)
                {
                    Main.SetActive(false);
                    Menu.SetActive(true);

                    state = STATE.MENU;
                }

                break;

            case STATE.MENU:

                break;

            case STATE.START:
                StartCoroutine(ImageFadeIn());
                StartCoroutine(SoundFadeIn());

                break;

            case STATE.CONTINUE:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Continue.SetActive(false);
                    Menu.SetActive(true);

                    state = STATE.MENU;
                }

                break;

            case STATE.CREDIT:
                {
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        Credit.SetActive(false);
                        Menu.SetActive(true);

                        state = STATE.MENU;
                    }
                }

                break;
        }
    }

    #region 메뉴 화면에서 각 버튼에 대한 동작
    public void GameStart() // 처음부터 시작
    {
        Menu.SetActive(false);

        state = STATE.START;

    }
    [SerializeField, Header("페이드 인/아웃")] private Image _image;

    private IEnumerator ImageFadeIn()
    {
        float time = 1f; // FadeIn에 걸리는 시간
        float alphaValue = 0f; // 알파값

        while (alphaValue < 1f)
        {
            alphaValue += Time.deltaTime / time;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }
    [SerializeField, Header("배경 음악")] private AudioSource _background;
    private IEnumerator SoundFadeIn()
    {
        float time = 0f; // time부터 value까지
        float value = 1f; // FadeIn에 걸리는 시간 
        float currentVolume = 0.2f; float targetVolume = 0f; // 현재 볼륨과 타겟 볼륨

        while (time < value)
        {
            time += Time.deltaTime;
            currentVolume = Mathf.Lerp(0.2f, targetVolume, time / value);
            _background.volume = currentVolume;
            yield return null;
        }

        _background.Stop();
    }

    private IEnumerator ImageFadeOut()
    {
        float time = 0f; // FadeOut에 걸리는 시간
        float alphaValue = 1f; // 알파값

        while (alphaValue > 0f)
        {
            alphaValue -= Time.deltaTime / time;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alphaValue);
            yield return null;
        }
    }
    private IEnumerator SoundFadeOut()
    {
        yield return null;
    }

    public void GameContinue() // 이어서 시작
    {
        Menu.SetActive(false);
        Continue.SetActive(true);

        state = STATE.CONTINUE;
    }

    public void GameCredit() // 크레딧
    {
        Menu.SetActive(false);
        Credit.SetActive(true);

        state = STATE.CREDIT;
    }

    public void GameExit() // 게임 종료
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(); // 어플리케이션 종료
#endif
    }
    #endregion
}