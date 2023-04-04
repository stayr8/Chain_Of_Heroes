using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField, Header("���� ������ �Ŵ���")] private MonsterDataManager MDM;
    [SerializeField, Header("��� ĳ����")] private GameObject Character;
    CharacterDataManager CDM;

    private void Start()
    {
        MDM = GetComponent<MonsterDataManager>();
        CDM = Character.GetComponent<CharacterDataManager>();

        // �׷��� ĳ���ʹ� ���⿡�ٰ� GetComponent<~>();�� �޾ƿ��� �ȵ�!
        // �׷��ٸ� � ������� �޾ƿ;� �ϳ�?
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Calc_Attack();
        }

        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Calc_Defense();
        }
    }



    #region ���� ����
    // ���� ������ = ���� ���ݷ� * (100 / (100 + ��� ĳ���� ����))
    // ������ ������ �߰� �ʿ�!

    private float monsterDamage; // ���� ������
    private float monsterAP; // ���� ���ݷ�
    private float characterDP; // ��� ĳ���� ����
    private void Calc_Attack()
    {
        monsterAP = MDM.m_attackPower;
        Debug.Log("���� ���ݷ�: " + monsterAP);
        characterDP = CDM.m_defensePower;
        Debug.Log("��� ĳ���� ����: " +  characterDP);
                
        // ���� ������ ����
        monsterDamage = monsterAP * (100 / (100 + characterDP));
        Debug.Log("���� ������: " + monsterDamage);

        // ������ �ֱ�
        // Attack(monsterDamage);
        CDM.m_hp -= (int)monsterDamage;
        Debug.Log("ĳ���� �ǰ�! ĳ������ ���� ü����: " + CDM.m_hp);
    }
    #endregion

    #region ��� ����
    // ���� �ǰ� ������ = ĳ���� ������ * (100 / (100 + ���� ����))

    private float getDamage; // ���� �ǰ� ������
    private float characterAP; // ĳ���� ������
    private float monsterDP; // ���� ����
    private void Calc_Defense()
    {
        characterAP = CDM.m_attackPower;
        Debug.Log("ĳ���� ������: " + characterAP);
        monsterDP = MDM.m_defensePower;
        Debug.Log("���� ����: " +  monsterDP);

        getDamage = characterAP * (100 / (100 + monsterDP));
        Debug.Log("���� �ǰ� ������: " + getDamage);

        // ������ �ޱ�
        // Damage((int)getDamage);
        MDM.m_hp -= (int)getDamage;
        Debug.Log("���� �ǰ�! ������ ���� ü����: " +  MDM.m_hp);
    }
    #endregion
}
