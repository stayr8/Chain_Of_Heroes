using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    public static StageUI Instance { get; private set; }

    [SerializeField] private GameObject PlayerTurn;
    [SerializeField] private GameObject EnemyTurn;
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private Image Panel;

    private float time = 0f;

    private float F_time = 0.2f;
    private float F_time2 = 1.5f;


    private bool IsMenu;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one StageUI! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

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

    public void FadeIn()
    {
        StartCoroutine(FadeFlow());
    }

   
    IEnumerator FadeFlow()
    {
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        time = 0f;

        yield return new WaitForSeconds(0.5f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time2;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        //Panel.gameObject.SetActive(false);

        yield return null;
    }
    public void OnFade()
    {
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
        }

        time = 0f;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time2;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
        }
        //Panel.gameObject.SetActive(false);

        return;
    }

    /*
    public void FadeIn()
    {
        StartCoroutine(OnFadeIn());
    }
    public void FadeOut()
    {
        StartCoroutine(OffFadeOut());
    }

    IEnumerator OnFadeIn()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        yield return null;
    }

    IEnumerator OffFadeOut()
    {
        Color alpha = Panel.color;
        time = 0f;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time2;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        yield return null;
    }*/

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
