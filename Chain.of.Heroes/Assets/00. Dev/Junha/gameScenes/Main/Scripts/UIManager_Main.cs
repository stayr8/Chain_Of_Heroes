using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Main : MonoBehaviour
{
    [SerializeField, Header("���� ȭ��")] private GameObject Main;
    [SerializeField, Header("\n�޴� ȭ��")] private GameObject Menu;
    [SerializeField, Header("\n�̾ ���� ȭ��")] private GameObject Continue;
    [SerializeField, Header("\nũ���� ȭ��")] private GameObject Credit;

    public static UIManager_Main instance;
    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        UI_STATE();
    }

    public enum STATE { MAIN, MENU, CONTINUE, CREDIT }
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
        Debug.Log("ó������ ����");
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
