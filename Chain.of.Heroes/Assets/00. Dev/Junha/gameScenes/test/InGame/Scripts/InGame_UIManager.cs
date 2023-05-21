using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InGame_UIManager : MonoBehaviour
{
    #region instance화 :: Awake()함수 포함
    public static InGame_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [SerializeField] private GameObject _Panel;
    [SerializeField] private GameObject _Menu;
    [SerializeField] private GameObject _PartyInfo;

    private void Update()
    {
        UI_STATE();
    }

    private enum STATE { InGame, MENU, PARTY_INFO }
    private STATE _state = STATE.InGame;
    private void UI_STATE()
    {
        switch (_state)
        {
            case STATE.InGame: // 인게임 상태에서 ESC키 입력
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    InGame_Cursor.isInitStart = false;
                    _Panel.SetActive(true);
                    _Menu.SetActive(true);

                    _state = STATE.MENU;
                }
                break;

            case STATE.MENU: // 메뉴 상태에서 ESC키 입력
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    _Menu.SetActive(false);
                    _Panel.SetActive(false);

                    _state = STATE.InGame;
                }
                break;

            case STATE.PARTY_INFO: // 아군 정보 상태에서 ESC키 입력
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    _PartyInfo.SetActive(false);
                    _Menu.SetActive(true);

                    _state = STATE.MENU;
                }
                break;
        }
    }

    public void OnInfo()
    {
        _state = STATE.PARTY_INFO;

        _PartyInfo.SetActive(true);
        _Menu?.SetActive(false);
    }
}
