using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageUI : MonoBehaviour
{
    [SerializeField] private GameObject PlayerTurn;
    [SerializeField] private GameObject EnemyTurn;
    [SerializeField] private GameObject MenuUI;

    private bool IsMenu;

    private void Start()
    {
        IsMenu = false;

        BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (TurnSystem.Property.IsPlayerTurn)
            {
                //Debug.Log("Player");
                PlayerShow();
                EnemyHide();
            }
            else
            {
                //Debug.Log("Enemy");
                EnemyShow();
                PlayerHide();
            }

        },false);

        PlayerHide();
        EnemyHide();
    }

    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!IsMenu)
            {
                IsMenu = !IsMenu;
                MenuShow(IsMenu);
            }
        }

    }

    private void PlayerShow()
    {
        PlayerTurn.SetActive(true);
    }

    private void PlayerHide()
    {
        PlayerTurn.SetActive(false);
    }

    private void EnemyShow()
    {
        EnemyTurn.SetActive(true);
    }

    private void EnemyHide()
    {
        EnemyTurn.SetActive(false);
    }

    private void MenuShow(bool isShow)
    {
        MenuUI.SetActive(isShow);
    }
    

    public void OnContinueButton()
    {
        IsMenu = !IsMenu;
        MenuShow(IsMenu);
    }

    public void OnResetButton()
    {

    }

    public void OnExitButton()
    {
        // UI
        MenuUI.SetActive(false);
        EnemyHide();
        PlayerHide();

        // Unit Destroy
        UnitManager.Instance.playerpos = 0;
        UnitManager.Instance.enemypos = 0;
        UnitManager.Instance.Release();

        SceneManager.LoadScene("ChoiceScene");
    }

}
