using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Main : MonoBehaviour
{
    [SerializeField, Header("메인 화면")] private GameObject Main;
    [SerializeField, Header("\n메뉴 화면")] private GameObject Menu;
    [SerializeField, Header("\n이어서 시작 화면")] private GameObject Continue;
    [SerializeField, Header("\n크레딧 화면")] private GameObject Credit;

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

    #region 메뉴 화면에서 각 버튼에 대한 동작
    public void GameStart() // 처음부터 시작
    {
        Debug.Log("처음부터 시작");
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
