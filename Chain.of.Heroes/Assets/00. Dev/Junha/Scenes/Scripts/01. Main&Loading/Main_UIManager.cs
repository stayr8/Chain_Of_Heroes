using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;

public class Main_UIManager : MonoBehaviour
{
    #region instanceȭ :: Awake()�Լ� ����
    public static Main_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField, Header("[���� ȭ��] ������Ʈ")] private GameObject Main;
    [SerializeField, Header("[�޴� ȭ��] ������Ʈ")] private GameObject Menu;
    [SerializeField, Header("[�̾ ���� ȭ��] ������Ʈ")] private GameObject Continue;
    [SerializeField, Header("[ũ���� ȭ��] ������Ʈ")] private GameObject Credit;

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
                if (Input.anyKeyDown)
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
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Credit.SetActive(false);
                    Menu.SetActive(true);

                    state = STATE.MENU;
                }
                break;
        }
    }

    #region �޴� ȭ�鿡�� �� ��ư�� ���� ����
    public void GameStart() // ó������ ����
    {
        Menu.SetActive(false);

        SoundManager.instance.Sound_SelectMenu();
        SoundManager.instance.Sound_FadeStop();

        state = STATE.START;
    }
    public void GameContinue() // �̾ ����
    {
        Menu.SetActive(false);
        Continue.SetActive(true);

        SoundManager.instance.Sound_SelectMenu();

        state = STATE.CONTINUE;
    }
    public void GameCredit() // ũ����
    {
        Menu.SetActive(false);
        Credit.SetActive(true);

        SoundManager.instance.Sound_SelectMenu();

        state = STATE.CREDIT;
    }
    public void GameExit() // ���� ����
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



    #region FadeIn / FadeOut
    private IEnumerator ImageFadeIn()
    {
        float time = 1f; // FadeIn�� �ɸ��� �ð�
        float alphaValue = 0f; // ���İ�

        while (alphaValue < 1f)
        {
            alphaValue += Time.deltaTime / time;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        LoadingSceneController.LoadScene("Cinematic");
    }
    private IEnumerator SoundFadeIn()
    {
        float time = 0f; // time���� value����
        float value = 1f; // FadeIn�� �ɸ��� �ð� 
        float currentVolume = 0.2f; float targetVolume = 0f; // ���� ������ Ÿ�� ����

        while (time < value)
        {
            time += Time.deltaTime;
            float temp = Mathf.Lerp(currentVolume, targetVolume, time / value);
            //_background.volume = temp;
            yield return null;
        }
    }

    private IEnumerator ImageFadeOut()
    {
        float time = 0f; // FadeOut�� �ɸ��� �ð�
        float alphaValue = 1f; // ���İ�

        while (alphaValue > 0f)
        {
            alphaValue -= Time.deltaTime / time;
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, alphaValue);
            yield return null;
        }
    }
    private IEnumerator SoundFadeOut()
    {
        float time = 0f; // time���� value����
        float value = 1f; // FadeIn�� �ɸ��� �ð� 
        float currentVolume = 0f; float targetVolume = 1f; // ���� ������ Ÿ�� ����

        while (time < value)
        {
            time += Time.deltaTime;
            float temp = Mathf.Lerp(currentVolume, targetVolume, time / value);
            //_background.volume = temp;
            yield return null;
        }
    }
    #endregion
}