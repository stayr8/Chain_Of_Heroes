using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class AttackActionSystem : MonoBehaviour
{
    public static AttackActionSystem Instance { get; private set; }

    private Vector3 unitpos;
    private Vector3 targetpos;
    private Vector3 chainpos_1;
    private Vector3 chainpos_2;
    private Quaternion unitrotation;
    private Quaternion targetrotation;
    private Quaternion chainrotation_1;
    private Quaternion chainrotation_2;

    private CharacterDataManager characterDataManager;
    private MonsterDataManager monsterDataManager;
    private Unit player;
    private Unit enemy;

    private bool OnAttackAtGround;
    private bool isAtk;
    private bool isChainAtk;


    public Slider player_bar;
    public Slider enemy_bar;
    [SerializeField] private TextMeshProUGUI PlayerHPText;
    [SerializeField] private TextMeshProUGUI EnemyHPText;
    [SerializeField] private TextMeshProUGUI PlayerName;
    [SerializeField] private TextMeshProUGUI EnemyName;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one AttackActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        OnAttackAtGround = false;
        isAtk = false;
        isChainAtk = false;
    }

    private void Update()
    {
        if (OnAttackAtGround && player != null)
        {
            player_bar.value = player.GetCharacterDataManager().m_hp / 1000;
            PlayerHPText.text = "" + (int)player.GetCharacterDataManager().m_hp;
            PlayerName.text = "" + player.GetUnitName();
        }

        if (OnAttackAtGround && enemy != null)
        {
            enemy_bar.value = enemy.GetMonsterDataManager().m_hp / 100;
            EnemyHPText.text = "" + (int)enemy.GetMonsterDataManager().m_hp;
            EnemyName.text = "" + enemy.GetUnitName();
        }
    }


    public void OnAtLocationMove(Unit unit, Unit target)
    {

        characterDataManager = unit.GetCharacterDataManager();
        monsterDataManager = target.GetMonsterDataManager();
        player = unit;
        enemy = target;

        unit.SetIsGrid(true);
        target.SetIsGrid(true);

        OnAttackAtGround = true;

        unitpos = unit.GetWorldPosition();
        targetpos = target.GetWorldPosition();
        unitrotation = unit.transform.rotation;
        targetrotation = target.transform.rotation;

        Vector3 playerlocationMove = new Vector3(0, 150, -3);

        Vector3 enemylocationMove = new Vector3(0, 150, 3);

       
        LevelGrid.Instance.RemoveUnitAtGridPosition(unit.GetGridPosition(), unit);
        LevelGrid.Instance.RemoveUnitAtGridPosition(target.GetGridPosition(), target);

        unit.SetPosition(playerlocationMove);
        target.SetPosition(enemylocationMove);

        unit.transform.rotation = Quaternion.Euler(0, 0, 0);
        target.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void OffAtLocationMove(Unit unit, Unit target)
    {

        if (unit != null)
        {
            unit.SetPosition(unitpos);
        }

        if (target != null)
        {
            target.SetPosition(targetpos);
        }

        if (unit.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(unit.GetGridPosition(), unit);
            unit.transform.rotation = unitrotation;
        }
        
        if (target.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(target.GetGridPosition(), target);
            target.transform.rotation = targetrotation;
        }

        unit.SetIsGrid(false);
        target.SetIsGrid(false);

        OnAttackAtGround = false;

    }

    public void OnAtChainLocationMove_1(Unit chainunit)
    {
        characterDataManager = chainunit.GetCharacterDataManager();

        chainunit.SetIsGrid(true);

        chainpos_1 = chainunit.GetWorldPosition();
        chainrotation_1 = chainunit.transform.rotation;

        LevelGrid.Instance.RemoveUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);

        Vector3 chainlocationMove = new Vector3(-5f, 150, 3.2f);
        chainunit.SetPosition(chainlocationMove);


        chainunit.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    public void OffAtChainLocationMove_1(Unit chainunit)
    {
        chainunit.SetPosition(chainpos_1);

        LevelGrid.Instance.AddUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);
        chainunit.transform.rotation = chainrotation_1;

        chainunit.SetIsGrid(false);
        chainunit.SetChainfirst(false);
    }

    public void OnAtChainLocationMove_2(Unit chainunit)
    {
        characterDataManager = chainunit.GetCharacterDataManager();

        chainunit.SetIsGrid(true);

        chainpos_2 = chainunit.GetWorldPosition();
        chainrotation_2 = chainunit.transform.rotation;

        LevelGrid.Instance.RemoveUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);

        Vector3 chainlocationMove = new Vector3(1.9f, 150, 4.4f);
        chainunit.SetPosition(chainlocationMove);


        chainunit.transform.rotation = Quaternion.Euler(0, -140, 0);
    }

    public void OffAtChainLocationMove_2(Unit chainunit)
    {
        chainunit.SetPosition(chainpos_2);

        LevelGrid.Instance.AddUnitAtGridPosition(chainunit.GetGridPosition(), chainunit);
        chainunit.transform.rotation = chainrotation_2;

        chainunit.SetIsGrid(false);
        chainunit.SetChaintwo(false);
    }

    public bool OnAttackGroundCheck()
    {
        return OnAttackAtGround;
    }

    public CharacterDataManager GetCharacterDataManager()
    {
        return characterDataManager;
    }

    public void SetCharacterDataManager(CharacterDataManager characterDataManager)
    {
        this.characterDataManager = characterDataManager;
    }

    public MonsterDataManager GetMonsterDataManager()
    {
        return monsterDataManager;
    }

    public Unit GetPlayer()
    {
        return player;
    }

    public Unit GetEnemy()
    {
        return enemy;
    }

    public bool GetIsAtk()
    {
        return isAtk;
    }

    public void SetIsAtk(bool isAtk)
    {
        this.isAtk = isAtk;
    }

    public bool GetIsChainAtk()
    {
        return isChainAtk;
    }

    public void SetIsChainAtk(bool isChainAtk)
    {
        this.isChainAtk = isChainAtk;
    }
}
