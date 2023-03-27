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
        //    //Debug.Log("���: " + node);
        //    // �� �������� �ڷ�

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
            Debug.Log("����ġ ȹ��! +3\n" + firstSheet1.CurrentExp + " / " + firstSheet1.MaxExp);
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

        // ���� ����ġ�� �ִ� ����ġ���� ���ٸ�?
        if(m_currentExp >= m_maxExp)
        {
            ++firstSheet1.Level;
            firstSheet1.CurrentExp = 0;
            Debug.Log("���� ��! ���� ������: " + m_level);
        }
    }
}

#region ���� �����
/*

    public void DebugInfo()
    {
        Debug.Log("���̵�: " + ID
            + ("\n�̸�: ") + Name
            + ("\n����: ") + Level
            + ("\n�ִ� ����: ") + MaxLevel
            + ("\n���� ����ġ: ") + CurrentExp
            + ("\n�ִ� ����ġ: ") + MaxExp
            + ("\n���ݷ�: ") + AttackPower
            + ("\n������: ") + ChainAttackPower
            + ("\n����: ") + DefensePower
            + ("\nü��: ") + Hp
            + ("\nũ��Ƽ�� Ȯ��: ") + CriticalRate
            + ("\nũ��Ƽ�� ������: ") + CriticalDamage
            );
    }

 */
#endregion