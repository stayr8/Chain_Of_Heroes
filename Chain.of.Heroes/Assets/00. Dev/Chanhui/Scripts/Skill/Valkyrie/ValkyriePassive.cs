using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValkyriePassive : MonoBehaviour
{
    private CharacterDataManager _cdm;

    private void Awake()
    {
        _cdm = GetComponent<CharacterDataManager>();
    }


    // Ʈ������ ���� 9ĭ ���� ��� �÷��̾� ���ֵ��� ��� ���ݷ� + 5%(* 1.05f)
}
