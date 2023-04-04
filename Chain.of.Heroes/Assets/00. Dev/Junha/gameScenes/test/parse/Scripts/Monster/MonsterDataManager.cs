using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    [SerializeField, Header("ø¢ºø ¿Ã∏ß")] private string CharacterName;

    [Header("ƒ≥∏Ø≈Õ µ•¿Ã≈Õ")]
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

    private Goblin[] _Array;
    private Goblin firstArray;
    private void Awake()
    {
        var data = Resources.Load<TextAsset>(CharacterName);

        var Root = SimpleJSON.JSON.Parse(data.text);

        _Array = new Goblin[Root.Count];

        for (int i = 0; i < Root.Count; ++i)
        {
            var node = Root[i];

            var Goblin = new Goblin();
            Goblin.Parse(node);

            _Array[i] = Goblin;
        }

        firstArray = _Array[0];

        initInfo();
    }

    private void Update()
    {
        
    }

    private void initInfo()
    {
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

        firstArray = _Array[m_level - 1];
    }
}
