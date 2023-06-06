using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataManager : MonoBehaviour
{
    [Header("� Json ������ �ҷ��� ���ΰ�?")] public string CharacterName;
    [Header("ĳ���� ������")]
    public int m_id; // [���̵�]
    public string m_name; // [�̸�]

    public int m_level; // [���� ����]
    public int m_maxLevel; // [�ִ� ����]

    public float m_currentExp; // [���� ����ġ]
    public float m_maxExp; // [�ִ� ����ġ]

    public float m_attackPower; // [���ݷ�]
    public float m_chainAttackPower; // [������]
    public float m_defensePower; // [����]

    public float m_hp; // [���� ü��]

    public float m_criticalRate; // [ũ��Ƽ�� Ȯ��]
    public float m_criticalDamage; // [ũ��Ƽ�� ������]

    public string m_class; // [Ŭ����/Ÿ��]
    public string m_property; // [�Ӽ�]

    public string m_resourcePath; // [ĳ���� �̹���]
    public string m_char_resourcePath; // [ĳ���� �� �̹���]
    public string m_back_resourcePath; // [ĳ���� ��� �̹���]

    [Header("������ ������")]
    public float m_maxhp; // [�ִ� ü��]
    public float m_damagereductionRate; // [������ ������]
    public float m_skilldamagecoefficient; // [��ų ������ ���]
    public int m_UnlockMapID;
    public int NumForLvUp = 0; // [�÷��̾� ��¥ ����]
    private int previousNumLv = 0;

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

        initInfo();
        m_maxhp = m_hp;
        m_damagereductionRate = 0;
        m_skilldamagecoefficient = 0;
        previousNumLv = NumForLvUp;
    }



    private void Update()
    {
        if(NumForLvUp < 39)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ++NumForLvUp;
                initInfo();
            }
        }

        LevelUp();
        
        if((m_currentExp == 0) && (previousNumLv != NumForLvUp))
        {
            initInfo();
            m_maxhp = m_hp;
            previousNumLv = NumForLvUp;
        }

        if (m_hp <= 0)
        {
            m_hp = 0;
        }
    }

    public void LevelUp()
    {
        while (m_currentExp >= m_maxExp)
        {
            ++NumForLvUp;
            previousNumLv = NumForLvUp;
            m_currentExp -= m_maxExp;
            float exp = m_currentExp;
            initInfo();
            m_maxhp = m_hp;
            m_currentExp = exp;
        }
    }

    
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
        m_char_resourcePath = firstArray.CResourcePath;
        m_back_resourcePath = firstArray.BResourcePath;

        m_UnlockMapID = firstArray.UnlockMapID;
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
        MonsterDataManager _mdm = AttackActionSystem.Instance.GetMonsterDataManager();
        if (_mdm.m_property == "����" && m_property == "���")
        {
            characterBase.Calc_Attack(this, _mdm, 1.15f);
        }
        else if (_mdm.m_property == "���" && m_property == "����")
        {
            characterBase.Calc_Attack(this, _mdm, 1.15f);
        }
        else if (_mdm.m_property == "����" && m_property == "����")
        {
            characterBase.Calc_Attack(this, _mdm, 1.15f);
        }
        else
        {
            characterBase.Calc_Attack(this, _mdm, 1f);
        }
        

        if (m_hp <= 0)
        {
            OnPlayerDie?.Invoke(this, EventArgs.Empty);
            player.GetAnyUnitDead();
            LevelGrid.Instance.RemoveUnitAtGridPosition(player.GetGridPosition(), player);
            player.SetIsGrid(true);

            //Destroy(gameObject, 4.0f);
            Invoke("DeadSpon", 3f);

            
        }
        else
        {
            OnPlayerDamage?.Invoke(this, EventArgs.Empty);

        }
    }

    public void SkillDamage()
    {
        ScreenShake.Instance.Shake();
        MonsterDataManager _mdm = AttackActionSystem.Instance.GetMonsterDataManager();
        if (_mdm.m_property == "����" && m_property == "���")
        {
            characterBase.Calc_SkillAttack(this, _mdm, 1.15f);
        }
        else if (_mdm.m_property == "���" && m_property == "����")
        {
            characterBase.Calc_SkillAttack(this, _mdm, 1.15f);
        }
        else if (_mdm.m_property == "����" && m_property == "����")
        {
            characterBase.Calc_SkillAttack(this, _mdm, 1.15f);
        }
        else
        {
            characterBase.Calc_SkillAttack(this, _mdm, 1f);
        }

        if (m_hp <= 0)
        {
            OnPlayerDie?.Invoke(this, EventArgs.Empty);
            player.GetAnyUnitDead();
            LevelGrid.Instance.RemoveUnitAtGridPosition(player.GetGridPosition(), player);
            player.SetIsGrid(true);
            //Destroy(gameObject, 4.0f);
            Invoke("DeadSpon", 3f);

            
        }
        else
        {
            OnPlayerDamage?.Invoke(this, EventArgs.Empty);
        }
    }

    private void DeadSpon()
    {
        Vector3 pos = new Vector3(0f, -20f, 0f);
        player.SetPosition(pos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "EnemyMelee")
        {
            Damage();
        }
        else if (other.transform.tag == "EnemySkillMelee")
        {
            SkillDamage();
        }
    }
}