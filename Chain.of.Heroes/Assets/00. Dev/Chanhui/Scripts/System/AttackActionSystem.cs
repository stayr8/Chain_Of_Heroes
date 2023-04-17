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
    private Quaternion unitrotation;
    private Quaternion targetrotation;

    private CharacterDataManager characterDataManager;
    private MonsterDataManager monsterDataManager;
    private Unit player;
    private Unit enemy;

    private bool OnAttackAtGround;
    private bool isAtk;


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
        if(unit != null)
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
        OnAttackAtGround = false;
    }

    public bool OnAttackGroundCheck()
    {
        return OnAttackAtGround;
    }

    public CharacterDataManager GetCharacterDataManager()
    {
        return characterDataManager;
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
}
