using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField, Header("몬스터 데이터 매니저")] private MonsterDataManager MDM;
    [SerializeField, Header("대상 캐릭터")] private GameObject Character;
    CharacterDataManager CDM;

    private void Start()
    {
        MDM = GetComponent<MonsterDataManager>();
        CDM = Character.GetComponent<CharacterDataManager>();

        // 그러나 캐릭터는 여기에다가 GetComponent<~>();로 받아오면 안돼!
        // 그렇다면 어떤 방식으로 받아와야 하나?
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



    #region 공격 공식
    // 몬스터 데미지 = 몬스터 공격력 * (100 / (100 + 대상 캐릭터 방어력))
    // 데미지 감소율 추가 필요!

    private float monsterDamage; // 몬스터 데미지
    private float monsterAP; // 몬스터 공격력
    private float characterDP; // 대상 캐릭터 방어력
    private void Calc_Attack()
    {
        monsterAP = MDM.m_attackPower;
        Debug.Log("몬스터 공격력: " + monsterAP);
        characterDP = CDM.m_defensePower;
        Debug.Log("대상 캐릭터 방어력: " +  characterDP);
                
        // 몬스터 데미지 결정
        monsterDamage = monsterAP * (100 / (100 + characterDP));
        Debug.Log("몬스터 데미지: " + monsterDamage);

        // 데미지 넣기
        // Attack(monsterDamage);
        CDM.m_hp -= (int)monsterDamage;
        Debug.Log("캐릭터 피격! 캐릭터의 남은 체력은: " + CDM.m_hp);
    }
    #endregion

    #region 방어 공식
    // 몬스터 피격 데미지 = 캐릭터 데미지 * (100 / (100 + 몬스터 방어력))

    private float getDamage; // 몬스터 피격 데미지
    private float characterAP; // 캐릭터 데미지
    private float monsterDP; // 몬스터 방어력
    private void Calc_Defense()
    {
        characterAP = CDM.m_attackPower;
        Debug.Log("캐릭터 데미지: " + characterAP);
        monsterDP = MDM.m_defensePower;
        Debug.Log("몬스터 방어력: " +  monsterDP);

        getDamage = characterAP * (100 / (100 + monsterDP));
        Debug.Log("몬스터 피격 데미지: " + getDamage);

        // 데미지 받기
        // Damage((int)getDamage);
        MDM.m_hp -= (int)getDamage;
        Debug.Log("몬스터 피격! 몬스터의 남은 체력은: " +  MDM.m_hp);
    }
    #endregion
}
