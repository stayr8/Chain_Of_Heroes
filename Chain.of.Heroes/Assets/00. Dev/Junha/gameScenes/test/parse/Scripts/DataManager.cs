using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField, Header("엑셀 이름")] private string CharacterName;

    public int m_id;
    public string m_name;
    public int m_level;
    public int m_maxLevel;
    public float m_currentExp;
    public float m_maxExp;
    public float m_attackPower;
    public float m_chainAttackPower;
    public float m_defensePower;
    public float m_hp;
    public float m_criticalRate;
    public float m_criticalDamage;

    private SwordWoman[] _Array;
    private SwordWoman firstArray;
    private void Awake()
    {
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

        for(int i = 0; i < Root.Count; ++i)
        {
            var node = Root[i];

            var sheet1 = new SwordWoman();
            sheet1.Parse(node);

            _Array[i] = sheet1;
        }

        firstArray = _Array[0];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            firstArray.CurrentExp += 3;
            Debug.Log("경험치 획득! +3\n" + firstArray.CurrentExp + " / " + firstArray.MaxExp);
        }
        updateInfo();
    }

    private void updateInfo()
    {
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

        firstArray = _Array[m_level - 1];

        // 현재 경험치가 최대 경험치보다 많다면?
        if(m_currentExp >= m_maxExp)
        {
            ++firstArray.Level;
            firstArray.CurrentExp = 0;
            Debug.Log("레벨 업! 현재 레벨은: " + m_level);
        }
    }
}

#region 정보 디버그
/*

    public void DebugInfo()
    {
        Debug.Log("아이디: " + ID
            + ("\n이름: ") + Name
            + ("\n레벨: ") + Level
            + ("\n최대 레벨: ") + MaxLevel
            + ("\n현재 경험치: ") + CurrentExp
            + ("\n최대 경험치: ") + MaxExp
            + ("\n공격력: ") + AttackPower
            + ("\n협공력: ") + ChainAttackPower
            + ("\n방어력: ") + DefensePower
            + ("\n체력: ") + Hp
            + ("\n크리티컬 확률: ") + CriticalRate
            + ("\n크리티컬 데미지: ") + CriticalDamage
            );
    }

 */
#endregion