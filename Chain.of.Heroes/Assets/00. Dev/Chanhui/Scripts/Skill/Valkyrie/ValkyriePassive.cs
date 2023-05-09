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


    // 트리서를 통해 9칸 내에 닿는 플레이어 유닛들은 모두 공격력 + 5%(* 1.05f)
}
