using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBase : MonoBehaviour
{
    [SerializeField, Header("캐릭터 데이터 매니저")] private CharacterDataManager CDM;
    //[SerializeField, Header("몬스터 데이터 매니저")] MonsterDataManager MDM;

    private void Start()
    {
        CDM = GetComponent<CharacterDataManager>();
        // MDM = GetComponent<MonsterDataManager>();

        // 그러나 몬스터는 여기에다가 GetComponent<~>();로 받아오면 안돼!
        // 그렇다면 어떤 방식으로 받아와야 하나?
    }

    private void Update()
    {

    }



    // 공격 공식
    // 최종 데미지 = 캐릭터 공격력 * ( 100 / (100 + 대상 몬스터 방어력))
    // 데미지 감소율 = 100 + 대상 몬스터 방어력

    // 크리티컬 최종 데미지 =
    // (캐릭터 공격력 * (100 / (100 + 대상 몬스터 방어력))) * (1.3 + 크리티컬 피해 증가량)

    // 1.3은 모든 캐릭터 공통적인 배수
    // 크리티컬 피해 증가량은 criticalDamage 가 아님!!

    private float finalDamage; // 최종 데미지

    private float characterAP; // 캐릭터 공격력
    private float characterCD; // 캐릭터 크리티컬 피해 증가량 (으로 수정해야 함.)

    // private float monsterDP; // 대상 몬스터 방어력

    private bool isCritical = false;
    private void Calc_Attack()
    {
        characterAP = CDM.m_attackPower;
        // monsterDP = MDM.m_defensePower;
        characterCD = CDM.m_criticalDamage;

        if (!isCritical)
        {
            // 최종 데미지 결정
            finalDamage = characterAP * (100 / (100 /*+ monsterDP*/));
        }
        else if(isCritical)
        {
            finalDamage = (characterAP * (100 / (100 /*+ monsterDP*/))) * (1.3f /*+ 크리티컬 피해 증가량*/);
        }

        // 데미지 넣기
        // Attack(finalDamage);
    }

    // 체인 공격 공식
    // 체인 데미지 = ( 체인 발동 캐릭터 공격력 * 100 / (100 + 대상 몬스터 방어력) * 0.7 )
    private float chainDamage;
    private float characterCAP;
    private bool isChain = false;
    private void Calc_ChainAttack()
    {
        if(isChain)
        {
            characterCAP = CDM.m_chainAttackPower;
            // monsterDP = MDM.m_defensePower;

            // 최종 데미지 결정
            chainDamage = characterCAP * 100 / (100 /*+ monsterDP*/) * 0.7f;

            // 데미지 넣기
            // Attack(chainDamage);
        }
    }

    // 방어 공식
    // 피격 데미지 = ( 몬스터 데미지 * 100 / (100 + 캐릭터 방어력) * (1 – 데미지 감소율) )
    private float getDamage;
    //private float monsterDamage;
    private float characterDP;
    private void Calc_Defense()
    {
        characterDP = CDM.m_defensePower;

        getDamage = /*monsterDamage **/ 100 / (100 + characterDP) * (1 /*- 데미지 감소율 */);

        // 데미지 받기
        // Damaged(getDamage);
    }

    // 레벨업 방식
    private float currentExp; // 현재 경험치 량
    private void getExp(float exp)
    {
        currentExp = CDM.m_currentExp;
        currentExp += exp;
        CDM.m_currentExp = currentExp;
    }

    // 레벨업
    private float maxExp; // 최대 경험치 량
    private void levelUp()
    {
        maxExp = CDM.m_maxExp;

        if(currentExp >= maxExp)
        {
            ++CDM.m_level;
        }
    }
}