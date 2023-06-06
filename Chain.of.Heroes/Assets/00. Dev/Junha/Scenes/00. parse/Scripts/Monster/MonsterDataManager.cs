using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    [SerializeField, Header("� Json ������ �ҷ��� ���ΰ�?")] private string MonsterName;
    [Header("ĳ���� ������")]
    public int m_id; // [���̵�]
    public string m_name; // [�̸�]

    public int m_level; // [���� ����]

    public float m_attackPower; // [���ݷ�]
    public float m_defensePower; // [����]

    public float m_hp; // [���� ü��]

    public float m_criticalRate; // [ũ��Ƽ�� Ȯ��]
    public float m_criticalDamage; // [ũ��Ƽ�� ������]

    public int m_actionPoint; // [���� �ൿ��]

    public float m_speed; // [���� �̵��ӵ�]
    public int m_movementRange; // [���� �ִ� �̵��Ÿ�]

    public int m_attackRange; // [���� ���� ��Ÿ�]

    public string m_rank; // [���� ���]
    public string m_property; // [�Ӽ�]

    [Header("������ ������")]
    public float m_maxhp; // [�ִ� ü��]
    public float m_damagereductionRate; // [������ ������]
    public float m_skilldamagecoefficient; // [��ų ������ ���]
    public int StageLevel = 0; // [���� ��¥ ����]
    private int previousNumLv = 0;

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
        previousNumLv = StageLevel;
    }

    private void Update()
    {
        if ((previousNumLv != StageLevel))
        {
            initInfo();
            m_maxhp = m_hp;
            previousNumLv = StageLevel;
        }

        if (m_hp <= 0)
        {
            m_hp = 0;
        }
    }


    
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
            if (_cdm.m_property == "����" && m_property == "���")
            {
                monsterBase.Calc_Attack(_cdm, this, 1.15f);
            }
            else if (_cdm.m_property == "���" && m_property == "����")
            {
                monsterBase.Calc_Attack(_cdm, this, 1.15f);
            }
            else if (_cdm.m_property == "����" && m_property == "����")
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

            if(monster.GetEnemyVisualType() == Unit.EnemyType.RedStoneGolem ||
               monster.GetEnemyVisualType() == Unit.EnemyType.Dragon)
            {
                monster.MonsterGridPosition(monster.GetGridPosition(), false);
                //monster.GetBossMonsterPosition().Clear();
            }
            else
            {
                LevelGrid.Instance.RemoveUnitAtGridPosition(monster.GetGridPosition(), monster);
            }
            
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
        if (_cdm.m_property == "����" && m_property == "���")
        {
            monsterBase.Calc_SkillAttack(_cdm, this, 1.15f);
        }
        else if (_cdm.m_property == "���" && m_property == "����")
        {
            monsterBase.Calc_SkillAttack(_cdm, this, 1.15f);
        }
        else if (_cdm.m_property == "����" && m_property == "����")
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

            if (monster.GetEnemyVisualType() == Unit.EnemyType.RedStoneGolem ||
              monster.GetEnemyVisualType() == Unit.EnemyType.Dragon)
            {
                monster.MonsterGridPosition(monster.GetGridPosition(), false);
                //monster.GetBossMonsterPosition().Clear();
            }
            else
            {
                LevelGrid.Instance.RemoveUnitAtGridPosition(monster.GetGridPosition(), monster);
            }
            Destroy(gameObject, 4.0f);

        }
        else
        {
            OnEnemyDamage?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ChainDamage()
    {
        ScreenShake.Instance.Shake();
        CharacterDataManager _cdm = AttackActionSystem.Instance.GetCharacterDataManager();

        monsterBase.Calc_ChainAttack(_cdm, this);

        if (m_hp <= 0)
        {
            Unit player = AttackActionSystem.Instance.GetCharacterChainFind();
            player.SetKillCount();

            OnEnemyDie?.Invoke(this, EventArgs.Empty);
            monster.GetAnyUnitDead();

            if (monster.GetEnemyVisualType() == Unit.EnemyType.RedStoneGolem ||
              monster.GetEnemyVisualType() == Unit.EnemyType.Dragon)
            {
                monster.MonsterGridPosition(monster.GetGridPosition(), false);
                //monster.GetBossMonsterPosition().Clear();
            }
            else
            {
                LevelGrid.Instance.RemoveUnitAtGridPosition(monster.GetGridPosition(), monster);
            }
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