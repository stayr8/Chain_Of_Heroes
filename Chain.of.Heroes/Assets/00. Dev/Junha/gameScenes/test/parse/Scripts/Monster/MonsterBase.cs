using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    [SerializeField, Header("몬스터 데이터 매니저")] private MonsterDataManager MDM;
    public CharacterDataManager CDM;

    public static MonsterBase Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        MDM = GetComponent<MonsterDataManager>();
    }

    private void Update()
    {

    }

    #region 공격 공식
    // 몬스터 데미지 = 몬스터 공격력 * (100 / (100 + 대상 캐릭터 방어력))

    // 캐릭터 기준으로 피격 시 다음과 같은 식을 사용하고 있었음.
    // 피격 데미지 = ( 몬스터 데미지 * 100 / (100 + 캐릭터 방어력) * (1 – 데미지 감소율) )
    // 따라서 위 기준처럼 식을 변경함.

    private float monsterDamage; // 몬스터 데미지
    private float monsterAP; // 몬스터 공격력
    private float characterDP; // 대상 캐릭터 방어력
    public void Calc_Attack()
    {
        monsterAP = MDM.m_attackPower;
        Debug.Log("몬스터 공격력: " + monsterAP);
        characterDP = CDM.m_defensePower;
        Debug.Log("대상 캐릭터 방어력: " + characterDP);

        // 몬스터 데미지 결정
        monsterDamage = monsterAP * (100 / (100 + characterDP)) * (1 /*- 데미지 감소율 */);
        Debug.Log("몬스터 데미지: " + (int)monsterDamage);

        // 데미지 넣기
        // Attack(monsterDamage);
        CDM.m_hp -= (int)monsterDamage;
        Debug.Log("캐릭터 피격! 캐릭터의 남은 체력은: " + CDM.m_hp);
    }
    #endregion



    #region 보류
    #region 방어 공식
    //// 몬스터 피격 데미지 = 캐릭터 데미지 * (100 / (100 + 몬스터 방어력))

    //private float getDamage; // 몬스터 피격 데미지
    //private float characterAP; // 캐릭터 데미지
    //private float monsterDP; // 몬스터 방어력
    //private void Calc_Defense()
    //{
    //    characterAP = CDM.m_attackPower;
    //    Debug.Log("캐릭터 데미지: " + characterAP);
    //    monsterDP = MDM.m_defensePower;
    //    Debug.Log("몬스터 방어력: " +  monsterDP);

    //    getDamage = characterAP * (100 / (100 + monsterDP));
    //    Debug.Log("몬스터 피격 데미지: " + (int)getDamage);

    //    // 데미지 받기
    //    // Damage((int)getDamage);
    //    MDM.m_hp -= (int)getDamage;
    //    Debug.Log("몬스터 피격! 몬스터의 남은 체력은: " +  MDM.m_hp);
    //}
    #endregion
    #endregion
}