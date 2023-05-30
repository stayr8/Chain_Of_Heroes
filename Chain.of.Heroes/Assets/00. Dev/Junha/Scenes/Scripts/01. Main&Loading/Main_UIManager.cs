using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

public class Main_UIManager : MonoBehaviour
{
    #region instance화 :: Awake()함수 포함
    public static Main_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField, Header("[메인 화면] 오브젝트")] private GameObject Main;
    [SerializeField, Header("[메뉴 화면] 오브젝트")] private GameObject Menu;
    [SerializeField, Header("[이어서 시작 화면] 오브젝트")] private GameObject Continue;
    [SerializeField, Header("[크레딧 화면] 오브젝트")] private GameObject Credit;

    private Image _image;

    private void Start()
    {
        _image = GameObject.Find("[Image] Fade").GetComponent<Image>();

        SoundManager.instance.Sound_MainSceneBGM();
    }

    private void Update()
    {
        UI_STATE();
    }

    private enum STATE { MAIN, MENU, START, CONTINUE, CREDIT }
    private STATE state = STATE.MAIN;
    private void UI_STATE()
    {
        switch (state)
        {
            case STATE.MAIN:
                if (Input.GetMouseButton(0))
                {
                    Main.SetActive(false);
                    Menu.SetActive(true);

                    SoundManager.instance.Sound_SelectMenu();

                    state = STATE.MENU;
                }
                break;

            case STATE.MENU:

                break;

            case STATE.START:
                StartCoroutine(ImageFadeIn());
                SoundManager.instance.Sound_FadeStop();
                break;

            case STATE.CONTINUE:
                //OffContinue();
                break;

            case STATE.CREDIT:
                OffCredit();
                break;
        }
    }

    #region 메뉴 화면에서 각 버튼에 대한 동작
    public void GameStart()
    {
        SoundManager.instance.Sound_SelectMenu();

        Menu.SetActive(false);

        state = STATE.START;
    }

    public void OnContiune()
    {
        SoundManager.instance.Sound_SelectMenu();

        LoadingSceneController.LoadScene("WorldMapScene");

        Menu.SetActive(false);
        //Continue.SetActive(true);

        state = STATE.CONTINUE;
    }
    private void OffContinue()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.instance.Sound_SelectMenu();

            Continue.SetActive(false);
            Menu.SetActive(true);

            state = STATE.MENU;
        }
    }

    public void OnCredit()
    {
        SoundManager.instance.Sound_SelectMenu();

        Menu.SetActive(false);
        Credit.SetActive(true);

        state = STATE.CREDIT;
    }
    private void OffCredit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundManager.instance.Sound_SelectMenu();

            Credit.SetActive(false);
            Menu.SetActive(true);

            state = STATE.MENU;
        }
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        SoundManager.instance.Sound_SelectMenu();

        UnityEditor.EditorApplication.isPlaying = false;
#else
        SoundManager.instance.Sound_SelectMenu();

        Application.Quit();
#endif
    }
    #endregion

    #region [페이드 인] 로직
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
        LoadingSceneController.LoadScene("Cinematic");
    }
    #endregion
}