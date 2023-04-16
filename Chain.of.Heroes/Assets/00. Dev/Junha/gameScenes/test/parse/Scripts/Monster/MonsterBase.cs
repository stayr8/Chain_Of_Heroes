using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    //[SerializeField, Header("몬스터 데이터 매니저")] private MonsterDataManager MDM;
    //public CharacterDataManager CDM;


    #region 공격 공식
    /*
     * // 몬스터 데미지 = 몬스터 공격력 * (100 / (100 + 대상 캐릭터 방어력))

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
        monsterDamage = monsterAP * (100 / (100 + characterDP)) * (1 /*- 데미지 감소율 );
        Debug.Log("몬스터 데미지: " + (int)monsterDamage);

        // 데미지 넣기
        // Attack(monsterDamage);
        CDM.m_hp -= (int)monsterDamage;
        //CDM.Damage();
        Debug.Log("캐릭터 피격! 캐릭터의 남은 체력은: " + CDM.m_hp);
    }*/
    #endregion

    #region 방어 공식

    private float finalDamage; // 최종 데미지
    private float characterAP; // 캐릭터 공격력
    private float characterCD; // 캐릭터 크리티컬 피해 증가량 (으로 수정해야 함.)
    private float monsterDP; // 대상 몬스터 방어력
    private bool isCritical; // 크리티컬인가?
    public void Calc_Attack(CharacterDataManager CDM, MonsterDataManager MDM)
    {
        characterAP = CDM.m_attackPower;
        Debug.Log("캐릭터 공격력: " + characterAP);
        monsterDP = MDM.m_defensePower;
        Debug.Log("대상 몬스터 방어력: " + monsterDP);

        isCritical = Calc_Critical(CDM);
        if (!isCritical)
        {
            Debug.Log("크리티컬 미발동!");

            // 최종 데미지 결정
            finalDamage = characterAP * (100 / (100 + monsterDP));
        }
        else if (isCritical)
        {
            Debug.Log("크리티컬 발동!");

            characterCD = CDM.m_criticalDamage;
            Debug.Log("크리티컬 피해 증가량" + characterCD);

            // 크리티컬 최종 데미지 결정
            finalDamage = (characterAP * (100 / (100 + monsterDP))) * (1.3f + characterCD);
        }
        Debug.Log("최종 데미지: " + (int)finalDamage);

        // 데미지 넣기
        // Attack((int)finalDamage);
        MDM.m_hp -= (int)finalDamage;
        //MDM.Damage();
        Debug.Log("몬스터 피격! 몬스터의 남은 체력은: " + MDM.m_hp);
    }

    // 크리티컬 확률 계산 공식

    private float characterCR; // 크리티컬 확률
    private bool Calc_Critical(CharacterDataManager CDM)
    {
        characterCR = CDM.m_criticalRate / 100f;
        float random = Random.value;
        Debug.Log("캐릭터 크리티컬 확률: " + characterCR + " | " + random + " :랜덤 수치");

        if (random <= characterCR)
        {
            return true;
        }
        else
        {
            return false;
        }
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