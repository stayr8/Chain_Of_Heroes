using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField, Header("���� �̸�")] private string CharacterName;

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
            Debug.Log("����ġ ȹ��! +3\n" + firstArray.CurrentExp + " / " + firstArray.MaxExp);
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

        // ���� ����ġ�� �ִ� ����ġ���� ���ٸ�?
        if(m_currentExp >= m_maxExp)
        {
            ++firstArray.Level;
            firstArray.CurrentExp = 0;
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