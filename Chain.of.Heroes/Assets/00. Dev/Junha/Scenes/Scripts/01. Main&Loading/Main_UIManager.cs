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

    #region �޴� ȭ�鿡�� �� ��ư�� ���� ����
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

    #region [���̵� ��] ����
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
    #endregion
}