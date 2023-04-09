using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main_UIManager : MonoBehaviour
{
    [SerializeField, Header("���� ȭ��")] private GameObject Main;
    [SerializeField, Header("\n�޴� ȭ��")] private GameObject Menu;
    [SerializeField, Header("\n�̾ ���� ȭ��")] private GameObject Continue;
    [SerializeField, Header("\nũ���� ȭ��")] private GameObject Credit;

    public static Main_UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // �̹��� ���İ� 0���� �ʱ�ȭ
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

    #region �޴� ȭ�鿡�� �� ��ư�� ���� ����
    public void GameStart() // ó������ ����
    {
        Menu.SetActive(false);

        state = STATE.START;

    }
    [SerializeField, Header("���̵� ��/�ƿ�")] private Image _image;

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
        SceneManager.LoadScene("Main");
    }
    [SerializeField, Header("��� ����")] private AudioSource _background;
    private IEnumerator SoundFadeIn()
    {
        float time = 0f; // time���� value����
        float value = 1f; // FadeIn�� �ɸ��� �ð� 
        float currentVolume = 0.2f; float targetVolume = 0f; // ���� ������ Ÿ�� ����

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
        yield return null;
    }

    public void GameContinue() // �̾ ����
    {
        Menu.SetActive(false);
        Continue.SetActive(true);

        state = STATE.CONTINUE;
    }

    public void GameCredit() // ũ����
    {
        Menu.SetActive(false);
        Credit.SetActive(true);

        state = STATE.CREDIT;
    }

    public void GameExit() // ���� ����
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(); // ���ø����̼� ����
#endif
    }
    #endregion
}