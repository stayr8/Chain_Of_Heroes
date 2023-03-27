using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
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

    private Sheet1[] _sheet1Array;
    private Sheet1 firstSheet1;
    private void Awake()
    {
        var data = Resources.Load<TextAsset>("Sheet1");

        var Root = SimpleJSON.JSON.Parse(data.text);

        _sheet1Array = new Sheet1[Root.Count];

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

            var sheet1 = new Sheet1();
            sheet1.Parse(node);

            _sheet1Array[i] = sheet1;
        }

        firstSheet1 = _sheet1Array[0];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            firstSheet1.CurrentExp += 3;
            Debug.Log("경험치 획득! +3\n" + firstSheet1.CurrentExp + " / " + firstSheet1.MaxExp);
        }
        updateInfo();
    }

    private void updateInfo()
    {
        m_id = firstSheet1.ID;
        m_name = firstSheet1.Name;
        m_level = firstSheet1.Level;
        m_maxLevel = firstSheet1.MaxLevel;
        m_currentExp = firstSheet1.CurrentExp;
        m_maxExp = firstSheet1.MaxExp;
        m_attackPower = firstSheet1.AttackPower;
        m_chainAttackPower = firstSheet1.ChainAttackPower;
        m_defensePower = firstSheet1.DefensePower;
        m_hp = firstSheet1.Hp;
        m_criticalRate = firstSheet1.CriticalRate;
        m_criticalDamage = firstSheet1.CriticalDamage;

        firstSheet1 = _sheet1Array[m_level - 1];

        // 현재 경험치가 최대 경험치보다 많다면?
        if(m_currentExp >= m_maxExp)
        {
            ++firstSheet1.Level;
            firstSheet1.CurrentExp = 0;
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