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
        monsterBase.Calc_Attack(AttackActionSystem.Instance.GetCharacterDataManager(), this);

        if (m_hp <= 0)
        {
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
            //Debug.Log(other.gameObject.name);
            ScreenShake.Instance.Shake();
            Damage();
        }
    }
}