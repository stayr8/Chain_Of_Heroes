using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamage;

    [SerializeField, Header("어떤 Json 파일을 불러올 것인가?")] private string CharacterName;

    [Header("캐릭터 데이터")]
    public int m_id;
    public string m_name;
    public int m_level;
    public float m_attackPower;
    public float m_defensePower;
    public float m_hp;
    public int m_actionPoint;
    public float m_speed;
    public int m_movementRange;
    public int m_attackRange;
    public int m_rank;

    private GoblinWarrior[] _Array;
    private GoblinWarrior firstArray;
    private void Awake()
    {
        var data = Resources.Load<TextAsset>(CharacterName);

        var Root = SimpleJSON.JSON.Parse(data.text);

        _Array = new GoblinWarrior[Root.Count];

        for (int i = 0; i < Root.Count; ++i)
        {
            var node = Root[i];

            var GoblinWarrior = new GoblinWarrior();
            GoblinWarrior.Parse(node);

            _Array[i] = GoblinWarrior;
        }

        //firstArray = _Array[0];
        initInfo();
    }

    private void Update()
    {
        if (m_hp < 0)
        {
            m_hp = 0;
            Die();
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
        m_actionPoint = firstArray.Monster_ActionPoint;
        m_speed = firstArray.Monster_Speed;
        m_movementRange = firstArray.Monster_MovementRange;
        m_attackRange = firstArray.Monster_AttackRange;
        m_rank = firstArray.Monster_Rank;
    }

    public float GetHealthNormalized()
    {
        return (float)m_hp / 100;
    }

    public float GetHealth()
    {
        return m_hp;
    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public void Damage()
    {
        OnDamage?.Invoke(this, EventArgs.Empty);
    }
}