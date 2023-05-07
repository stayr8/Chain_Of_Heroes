using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class StageUI : MonoBehaviour
{
    public static StageUI Instance { get; private set; }

    [SerializeField] private GameObject PlayerTurn;
    [SerializeField] private GameObject EnemyTurn;
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject AttackUI;
    [SerializeField] private GameObject TurnSystemUI;
    [SerializeField] private GameObject GameCondition;
    [SerializeField] private Image Panel;

    [SerializeField] private TextMeshProUGUI turnNumberText;

    private float time = 0f;

    private float F_time = 0.2f;
    private float F_time2 = 1f;


    private bool IsMenu;

    private List<Binding> Binds = new List<Binding>();

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
        //SoundManager.instance.Sound_Battle();

        IsMenu = false;

        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (TurnSystem.Property.IsPlayerTurn)
            {
                PlayerShow();
                EnemyHide();
                turnNumberText.text = "TURN " + TurnSystem.Property.TurnNumber;
            }
            else
            {
                if (!UnitManager.Instance.VictoryPlayer())
                {
                    EnemyShow();
                }
                PlayerHide();
                turnNumberText.text = "TURN " + TurnSystem.Property.TurnNumber;
            }

        },false);
        Binds.Add(Bind);

        PlayerHide();
        EnemyHide();
        turnNumberText.text = "TURN " + TurnSystem.Property.TurnNumber;
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

    private void ConditionShow()
    {
        GameCondition.SetActive(true);
    }

    public void AttackShow()
    {
        AttackUI.SetActive(true);
    }

    public void AttackHide()
    {
        AttackUI.SetActive(false);
    }

    public void TurnSystemShow()
    {
        TurnSystemUI.SetActive(true);
    }

    public void TurnSystemHide()
    {
        TurnSystemUI.SetActive(false);
    }

    public void Fade()
    {
        StartCoroutine(Fadeinout());
    }

    public void FadeIn()
    {
        StartCoroutine(Fadein());
    }

    public void FadeOut()
    {
        StartCoroutine(Fadeout());
    }


    IEnumerator Fadein()
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

        Panel.gameObject.SetActive(false);

        yield return null;
    }
    IEnumerator Fadeout()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time2;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);

        yield return null;
    }

    IEnumerator Fadeinout()
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

        time = 0f;

        yield return new WaitForSeconds(0.5f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time2;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);

        yield return null;
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
        

        // Unit Destroy
        UnitManager.Instance.playerpos = 0;
        UnitManager.Instance.enemypos = 0;

        SceneManager.LoadScene("ChoiceScene");
    }

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }
    }

}
