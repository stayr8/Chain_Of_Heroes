using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class InGame_UIManager : MonoBehaviour
{
    public event EventHandler Onfall;

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
    [SerializeField] private GameObject _fallUI;

    [SerializeField] private LayerMask UILayerMask;

    private bool isinGameFall;

    private void Update()
    {
        UI_STATE();

        if(isinGameFall)
        {
            /*
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, UILayerMask))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }*/
        }
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
        _Menu.SetActive(false);
    }

    public void OnTurnfo()
    {
        _Menu.SetActive(false);
        _Panel.SetActive(false);
        if (!TurnSystem.Property.IsTurnEnd && (TurnSystem.Property.IsPlayerTurn && (TurnSystem.Property.ActionPoints > 0)))
        {
            if (!AttackActionSystem.Instance.GetIsChainAtk_1() && !AttackActionSystem.Instance.GetIsChainAtk_2())
            {
                Debug.Log("턴 교체");
                TurnSystem.Property.ActionPoints = 0;
                TurnSystem.Property.IsPlayerTurn = false;
            }
        }
    }

    public void Onfallfo()
    {
        isinGameFall = true;
        TurnSystem.Property.IsTurnEnd = true;
        _Menu.SetActive(false);
        _Panel.SetActive(false);
        _fallUI.SetActive(true);
        Onfall?.Invoke(this, EventArgs.Empty);
        
    }

    public bool GetIsinGameFall()
    {
        return isinGameFall;
    }

}
