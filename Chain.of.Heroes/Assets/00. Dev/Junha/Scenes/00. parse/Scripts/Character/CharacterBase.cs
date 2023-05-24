using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    private float finalDamage; // 최종 데미지
    private float monsterAP; // 몬스터 공격력
    private float monsterCD; // 몬스터 크리티컬 피해 증가량 (으로 수정해야 함.)
    private float characterDP; // 대상 캐릭터 방어력
    private bool isCritical; // 크리티컬인가?
    public void Calc_Attack(CharacterDataManager CDM, MonsterDataManager MDM) // 공격 공식
    {
        Debug.Log("플레이어 맞음");
        monsterAP = MDM.m_attackPower;

        isCritical = Calc_Critical(MDM);
        if (!isCritical)
        {
            // 최종 데미지 결정
            finalDamage = monsterAP * (100 / (100 + (characterDP / 10)));
        }
        else if (isCritical)
        {
            monsterCD = MDM.m_criticalDamage;

            // 크리티컬 최종 데미지 결정
            finalDamage = (monsterAP * (100 / (100 + (characterDP / 10)))) * (1.3f + monsterCD);
        }

        // 몬스터 데미지 결정
        finalDamage = monsterAP * (100 / (100 + (characterDP / 10))) * (1 - CDM.m_damagereductionRate);

        // 데미지 넣기
        CDM.m_hp -= (int)finalDamage;

        Vector3 pos = new Vector3(this.transform.position.x - 1.5f, this.transform.position.y + 1f, this.transform.position.z);
        DamagePopup.Create(pos, (int)finalDamage, false);
    }

    private float monsterCR;
    private bool Calc_Critical(MonsterDataManager MDM) // 크리티컬 확률 공식
    {
        monsterCR = MDM.m_criticalRate / 100f;
        float random = Random.value;

        if (random <= monsterCR)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float monsterSKD; // 크리티컬인가?
    public void Calc_SkillAttack(CharacterDataManager CDM, MonsterDataManager MDM) // 스킬 공식
    {
        monsterAP = MDM.m_attackPower;
        characterDP = CDM.m_defensePower;
        monsterSKD = MDM.m_skilldamagecoefficient;

        finalDamage = (monsterAP * monsterSKD * (100 / (100 + (characterDP / 10)))) * (1 - CDM.m_damagereductionRate);

        CDM.m_hp -= (int)finalDamage;

        Vector3 pos = new Vector3(this.transform.position.x + 1f, this.transform.position.y + 1f, this.transform.position.z);
        DamagePopup.Create(pos, (int)finalDamage, isCritical);
    }

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
}