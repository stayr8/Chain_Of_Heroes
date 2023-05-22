using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBase : MonoBehaviour
{
    private float finalDamage; // 최종 데미지
    private float characterAP; // 캐릭터 공격력
    private float characterCD; // 캐릭터 크리티컬 피해 증가량 (으로 수정해야 함.)
    private float monsterDP; // 대상 몬스터 방어력
    private bool isCritical; // 크리티컬인가?
    public void Calc_Attack(CharacterDataManager CDM, MonsterDataManager MDM) // 공격 공식
    {
        Debug.Log("몬스터 맞음");
        characterAP = CDM.m_attackPower;
        monsterDP = MDM.m_defensePower;

        isCritical = Calc_Critical(CDM);
        if (!isCritical)
        { 
            // 최종 데미지 결정
            finalDamage = characterAP * (100 / (100 + (monsterDP / 10)));
        }
        else if (isCritical)
        {
            characterCD = CDM.m_criticalDamage;

            // 크리티컬 최종 데미지 결정
            finalDamage = (characterAP * (100 / (100 + (monsterDP / 10)))) * (1.3f + characterCD);
        }

        // 데미지 넣기
        MDM.m_hp -= (int)finalDamage;

        Vector3 pos = new Vector3(this.transform.position.x + 1f, this.transform.position.y + 1f, this.transform.position.z);
        DamagePopup.Create(pos, (int)finalDamage, isCritical);
    }

    private float characterCR;
    private bool Calc_Critical(CharacterDataManager CDM) // 크리티컬 확률 공식
    {
        characterCR = CDM.m_criticalRate / 100f;
        float random = Random.value;

        if (random <= characterCR)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Calc_ChainAttack(CharacterDataManager CDM, MonsterDataManager MDM) // 체인 어택 공식
    {
        
    }

    private float characterSKD; // 크리티컬인가?
    public void Calc_SkillAttack(CharacterDataManager CDM, MonsterDataManager MDM) // 스킬 공식
    {
        characterAP = CDM.m_attackPower;
        monsterDP = MDM.m_defensePower;
        characterSKD = CDM.m_skilldamagecoefficient;

        finalDamage = (characterAP * characterSKD * (100 / (100 + (monsterDP / 10))));

        MDM.m_hp -= (int)finalDamage;

        Vector3 pos = new Vector3(this.transform.position.x + 1f, this.transform.position.y + 1f, this.transform.position.z);
        DamagePopup.Create(pos, (int)finalDamage, isCritical);
    } 
}