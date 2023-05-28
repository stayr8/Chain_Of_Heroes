using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    [SerializeField, Header("어떤 Json 파일을 불러올 것인가?")] private string MonsterName;
    [Header("캐릭터 데이터")]
    public int m_id; // [아이디]
    public string m_name; // [이름]

    public int m_level; // [현재 레벨]

    public float m_attackPower; // [공격력]
    public float m_defensePower; // [방어력]

    public float m_hp; // [현재 체력]

    public float m_criticalRate; // [크리티컬 확률]
    public float m_criticalDamage; // [크리티컬 데미지]

    public int m_actionPoint; // [몬스터 행동력]

    public float m_speed; // [몬스터 이동속도]
    public int m_movementRange; // [몬스터 최대 이동거리]

    public int m_attackRange; // [몬스터 공격 사거리]

    public string m_rank; // [몬스터 등급]
    public string m_property; // [속성]

    [Header("별도의 데이터")]
    public float m_maxhp; // [최대 체력]
    public float m_damagereductionRate; // [데미지 감소율]
    public float m_skilldamagecoefficient; // [스킬 데미지 계수]

    private GoblinWarrior[] _Array;
    private GoblinWarrior firstArray;

    public event EventHandler OnEnemyDamage;
    public event EventHandler OnEnemyDie;

    private Unit monster;
    private MonsterBase monsterBase;

    private void Awake()
    {
        var data = Resources.Load<TextAsset>(MonsterName);

        var Root = SimpleJSON.JSON.Parse(data.text);

        _Array = new GoblinWarrior[Root.Count];

        for (int i = 0; i < Root.Count; ++i)
        {
            var node = Root[i];

            var GoblinWarrior = new GoblinWarrior();
            GoblinWarrior.Parse(node);

            _Array[i] = GoblinWarrior;
        }

        monster = GetComponent<Unit>();
        monsterBase = GetComponent<MonsterBase>();

        initInfo();
        m_maxhp = m_hp;
        m_damagereductionRate = 0;
        m_skilldamagecoefficient = 0;
    }

    private void Update()
    {
        if (m_hp <= 0)
        {
            m_hp = 0;
        }
    }


    private int StageLevel = 0;
    private void initInfo()
    {
        firstArray = _Array[StageLevel];

        m_id = firstArray.ID;
        m_name = firstArray.Monster_Name;

        m_level = firstArray.Monster_Level;

        m_attackPower = firstArray.AttackPower;
        m_defensePower = firstArray.Monster_DefensePower;

        m_hp = firstArray.Monster_Hp;

        m_criticalRate = firstArray.CriticalRate;
        m_criticalDamage = firstArray.CriticalDamage;

        m_actionPoint = firstArray.Monster_ActionPoint;

        m_speed = firstArray.Monster_Speed;
        m_movementRange = firstArray.Monster_MovementRange;

        m_attackRange = firstArray.Monster_AttackRange;

        m_rank = firstArray.Monster_Rank;
        m_property = firstArray.UnitProperty;
    }

    public float GetHealthNormalized()
    {
        return (float)m_hp / m_maxhp;
    }

    public float GetHealth()
    {
        return m_hp;
    }

    public void Damage()
    {
        ScreenShake.Instance.Shake();
        CharacterDataManager _cdm = AttackActionSystem.Instance.GetCharacterDataManager();
        Unit player = AttackActionSystem.Instance.GetCharacterChainFind();

        if (!player.GetChainfirst() || !player.GetChaintwo()) {
            if (_cdm.m_property == "물리" && m_property == "사격")
            {
                monsterBase.Calc_Attack(_cdm, this, 1.15f);
            }
            else if (_cdm.m_property == "사격" && m_property == "마법")
            {
                monsterBase.Calc_Attack(_cdm, this, 1.15f);
            }
            else if (_cdm.m_property == "마법" && m_property == "물리")
            {
                monsterBase.Calc_Attack(_cdm, this, 1.15f);
            }
            else
            {
                monsterBase.Calc_Attack(_cdm, this, 1f);
            }
        }
        else
        {
            monsterBase.Calc_ChainAttack(_cdm, this);
        }

        if (m_hp <= 0)
        {
            player.SetKillCount();

            OnEnemyDie?.Invoke(this, EventArgs.Empty);
            monster.GetAnyUnitDead();

            LevelGrid.Instance.RemoveUnitAtGridPosition(monster.GetGridPosition(), monster);
            monster.MonsterGridPosition(monster.GetGridPosition(), false);
            Destroy(gameObject, 4.0f);

        }
        else
        {
            OnEnemyDamage?.Invoke(this, EventArgs.Empty);

        }
    }

    public void SkillDamage()
    {
        ScreenShake.Instance.Shake();
        CharacterDataManager _cdm = AttackActionSystem.Instance.GetCharacterDataManager();
        if (_cdm.m_property == "물리" && m_property == "사격")
        {
            monsterBase.Calc_SkillAttack(_cdm, this, 1.15f);
        }
        else if (_cdm.m_property == "사격" && m_property == "마법")
        {
            monsterBase.Calc_SkillAttack(_cdm, this, 1.15f);
        }
        else if (_cdm.m_property == "마법" && m_property == "물리")
        {
            monsterBase.Calc_SkillAttack(_cdm, this, 1.15f);
        }
        else
        {
            monsterBase.Calc_SkillAttack(_cdm, this, 1f);
        }
        

        if (m_hp <= 0)
        {
            Unit player = AttackActionSystem.Instance.GetCharacterChainFind();
            player.SetKillCount();

            OnEnemyDie?.Invoke(this, EventArgs.Empty);
            monster.GetAnyUnitDead();

            LevelGrid.Instance.RemoveUnitAtGridPosition(monster.GetGridPosition(), monster);
            monster.MonsterGridPosition(monster.GetGridPosition(), false);
            Destroy(gameObject, 4.0f);

        }
        else
        {
            OnEnemyDamage?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Melee")
        {
            Damage();
        }
        else if (other.transform.tag == "SkillMelee")
        {
            SkillDamage();
        }
    }
}