using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    //[SerializeField, Header("캐릭터 데이터 매니저")] private CharacterDataManager CDM;
    //public MonsterDataManager MDM;


    #region 방어 공식
    // 몬스터 데미지 = 몬스터 공격력 * (100 / (100 + 대상 캐릭터 방어력))

    // 캐릭터 기준으로 피격 시 다음과 같은 식을 사용하고 있었음.
    // 피격 데미지 = ( 몬스터 데미지 * 100 / (100 + 캐릭터 방어력) * (1 – 데미지 감소율) )
    // 따라서 위 기준처럼 식을 변경함.

    private float monsterDamage; // 몬스터 데미지
    private float monsterAP; // 몬스터 공격력
    private float characterDP; // 대상 캐릭터 방어력
    public void Calc_Attack(CharacterDataManager CDM, MonsterDataManager MDM)
    {
        Debug.Log("플레이어 맞음");
        monsterAP = MDM.m_attackPower;
        //Debug.Log("몬스터 공격력: " + monsterAP);
        characterDP = CDM.m_defensePower;
        //Debug.Log("대상 캐릭터 방어력: " + characterDP);

        // 몬스터 데미지 결정
        monsterDamage = monsterAP * (100 / (100 + (characterDP / 10))) * (1 - CDM.m_damagereductionRate);
        //Debug.Log("몬스터 데미지: " + (int)monsterDamage);

        // 데미지 넣기
        // Attack(monsterDamage);
        CDM.m_hp -= (int)monsterDamage;

        Vector3 pos = new Vector3(this.transform.position.x - 1.5f, this.transform.position.y + 1f, this.transform.position.z);
        DamagePopup.Create(pos, (int)monsterDamage, false);
        //CDM.Damage();
        //Debug.Log("캐릭터 피격! 캐릭터의 남은 체력은: " + CDM.m_hp);
    }
    #endregion

    #region 공격 공식
    /*
    // 최종 데미지 = 캐릭터 공격력 * ( 100 / (100 + 대상 몬스터 방어력))
    // 데미지 감소율 = 100 + 대상 몬스터 방어력

    // 크리티컬 최종 데미지 =
    // (캐릭터 공격력 * (100 / (100 + 대상 몬스터 방어력))) * (1.3 + 크리티컬 피해 증가량)

    // 1.3은 모든 캐릭터 공통적인 배수
    // 크리티컬 피해 증가량은 criticalDamage 가 아님!!

    private float finalDamage; // 최종 데미지
    private float characterAP; // 캐릭터 공격력
    private float characterCD; // 캐릭터 크리티컬 피해 증가량 (으로 수정해야 함.)
    private float monsterDP; // 대상 몬스터 방어력
    private bool isCritical; // 크리티컬인가?
    public void Calc_Attack(CharacterDataManager CDM , MonsterDataManager MDM)
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
    */
    #endregion

    #region 체인 공격 공식
    // 체인 데미지 = ( 체인 발동 캐릭터 공격력 * 100 / (100 + 대상 몬스터 방어력) * 0.7 )
    /*
    private float chainDamage; // 체인 데미지
    private float characterCAP; // 체인 발동 캐릭터 공격력
    public bool isChain = false; // 체인 상태인가?
    private void Calc_ChainAttack(CharacterDataManager CDM, MonsterDataManager MDM)
    {
        if (isChain)
        {
            characterCAP = CDM.m_chainAttackPower;
            Debug.Log("체인 발동 캐릭터 공격력: " + characterCAP);
            monsterDP = MDM.m_defensePower;
            Debug.Log("대상 몬스터 방어력: " + monsterDP);

            // 최종 데미지 결정
            chainDamage = characterCAP * 100 / (100 + monsterDP) * 0.7f;
            Debug.Log("체인 데미지: " + (int)chainDamage);

            // 데미지 넣기
            // Attack((int)chainDamage);
            MDM.m_hp -= (int)chainDamage;
            Debug.Log("몬스터 체인 피격! 몬스터의 남은 체력은: " + MDM.m_hp);
        }
    }
    */
    #endregion

    #region 힐 공식


    private void HealthUp()
    {

    }
    #endregion



    #region 개발 전
    // 레벨업 방식
    private float currentExp; // 현재 경험치 량
    private void getExp(CharacterDataManager CDM, float exp)
    {
        currentExp = CDM.m_currentExp;
        currentExp += exp;
    }

    // 레벨업
    private float maxExp; // 최대 경험치 량
    private void levelUp(CharacterDataManager CDM)
    {
        maxExp = CDM.m_maxExp;

        if (currentExp >= maxExp)
        {
            ++CDM.m_level;
        }
    }
    #endregion



    #region 보류
    #region 방어 공식
    //// 피격 데미지 = ( 몬스터 데미지 * 100 / (100 + 캐릭터 방어력) * (1 – 데미지 감소율) )

    //private float getDamage; // 피격 데미지
    //private float monsterDamage; // 몬스터 데미지
    //private float characterDP; // 캐릭터 방어력
    //private void Calc_Defense()
    //{
    //    characterDP = CDM.m_defensePower;
    //    Debug.Log("캐릭터 방어력: " + characterDP);
    //    monsterDamage = MDM.m_attackPower;
    //    Debug.Log("몬스터 데미지: " + monsterDamage);

    //    getDamage = monsterDamage * (100 / (100 + characterDP)) * (1 /*- 데미지 감소율 */);
    //    Debug.Log("피격 데미지: " + (int)getDamage);

    //    // 데미지 받기
    //    // Damaged((int)getDamage);
    //    CDM.m_hp -= (int)getDamage;
    //    Debug.Log("캐릭터 피격! 캐릭터의 남은 체력은: " + CDM.m_hp);
    //}
    #endregion
    #endregion

    
    

}