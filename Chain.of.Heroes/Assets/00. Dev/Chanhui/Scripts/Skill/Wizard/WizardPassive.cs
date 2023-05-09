using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardPassive : MonoBehaviour
{
    private CharacterDataManager _cdm;

    private void Awake()
    {
        _cdm = GetComponent<CharacterDataManager>();
    }


    private void Start()
    {
        Passive();
    }


    private void Passive()
    {
        _cdm.m_criticalRate -= 12;
        _cdm.m_criticalDamage += 3.0f;
    }

    // ���߿� ��ų �ൿ�� ������ -1 ���� ��Ű��
}
