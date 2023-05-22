using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataManager : MonoBehaviour
{
    [Header("어떤 Json 파일을 불러올 것인가?")] public string CharacterName;
    [Header("캐릭터 데이터")]
    public int m_id; // [아이디]
    public string m_name; // [이름]

    public int m_level; // [현재 레벨]
    public int m_maxLevel; // [최대 레벨]

    public float m_currentExp; // [현재 경험치]
    public float m_maxExp; // [최대 경험치]

    public float m_attackPower; // [공격력]
    public float m_chainAttackPower; // [협공력]
    public float m_defensePower; // [방어력]

    public float m_hp; // [현재 체력]

    public float m_criticalRate; // [크리티컬 확률]
    public float m_criticalDamage; // [크리티컬 데미지]

    public string m_class; // [클래스/타입]
    public string m_property; // [속성]

    public string m_resourcePath; // [캐릭터 이미지]
    public string m_back_resourcePath; // [캐릭터 배경 이미지]

    [Header("별도의 데이터")]
    public float m_maxhp; // [최대 체력]
    public float m_damagereductionRate; // [데미지 감소율]



    private SwordWoman[] _Array;
    private SwordWoman firstArray;

    public event EventHandler OnPlayerDamage;
    public event EventHandler OnPlayerDie;

    private CharacterBase characterBase;
    private Unit player;

    private void Awake()
    {
        if (CharacterName == "") { return; }

        var data = Resources.Load<TextAsset>(CharacterName);
        var Root = SimpleJSON.JSON.Parse(data.text);
        _Array = new SwordWoman[Root.Count];

        /*
        //foreach (var node in Root.AsArray)
        //{
        //    //Debug.Log("노드: " + node);
        //    // 한 뭉탱이의 자료

        //    //var _sheet1 = new Sheet1();
        //    var _sheet1 = new Sheet1();

        //    _sheet1.Parse(node.Value);

        //    //Debug.Log(_sheet1.ID);
        //    //Debug.Log(_sheet1.Name);
        //    //_sheet1.DebugInfo();
        //}
        */

        for (int i = 0; i < Root.Count; ++i)
        {
            var node = Root[i];

            var SwordWoman = new SwordWoman();
            SwordWoman.Parse(node);

            _Array[i] = SwordWoman;
        }

        player = GetComponent<Unit>();
        if (TryGetComponent<CharacterBase>(out CharacterBase characterBase))
        {
            this.characterBase = characterBase;
        }

        //firstArray = _Array[0]; // Init
        initInfo();
        m_maxhp = m_hp;
        m_damagereductionRate = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            ++NumForLvUp;
            initInfo();
        }

        if (m_hp <= 0)
        {
            m_hp = 0;
        }
    }

    private int NumForLvUp = 0;
    private void initInfo()
    {
        firstArray = _Array[NumForLvUp];

        m_id = firstArray.ID;
        m_name = firstArray.Name;

        m_level = firstArray.Level;
        m_maxLevel = firstArray.MaxLevel;

        m_currentExp = firstArray.CurrentExp;
        m_maxExp = firstArray.MaxExp;

        m_attackPower = firstArray.AttackPower;
        m_chainAttackPower = firstArray.ChainAttackPower;
        m_defensePower = firstArray.DefensePower;

        m_hp = firstArray.Hp;

        m_criticalRate = firstArray.CriticalRate;
        m_criticalDamage = firstArray.CriticalDamage;

        m_class = firstArray.Class;
        m_property = firstArray.UnitProperty;

        m_resourcePath = firstArray.ResourcePath;
        m_back_resourcePath = firstArray.BResourcePath;
    }

    public float GetHealthNormalized()
    {
        return (float)m_hp;
    }

    public float GetHealth()
    {
        return m_hp;
    }

    public void Damage()
    {
        characterBase.Calc_Attack(this, AttackActionSystem.Instance.GetMonsterDataManager());

        if (m_hp <= 0)
        {
            OnPlayerDie?.Invoke(this, EventArgs.Empty);
            player.GetAnyUnitDead();

            LevelGrid.Instance.RemoveUnitAtGridPosition(player.GetGridPosition(), player);

            Destroy(gameObject, 4.0f);

        }
        else
        {
            OnPlayerDamage?.Invoke(this, EventArgs.Empty);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "EnemyMelee")
        {
            Debug.Log(other.gameObject.name);
            ScreenShake.Instance.Shake();
            Damage();
        }
    }
}