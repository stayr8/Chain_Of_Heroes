using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitBuffSystemUI : MonoBehaviour
{
    [SerializeField] private Transform CharacterBuffImagePrefab;
    [SerializeField] private Transform CharacterBuffContainerTransform;
    [SerializeField] private Transform EnemyBuffImagePrefab;
    [SerializeField] private Transform EnemyBuffContainerTransform;

    private List<CharacterBuffUI> characterBuffUIList;
    private List<EnemyBuffUI> enemyBuffUIList;

    private void Awake()
    {
        characterBuffUIList = new List<CharacterBuffUI>();
        enemyBuffUIList = new List<EnemyBuffUI>();
    }

    private void Start()
    {
        AttackActionSystem.Instance.OnUIBuff += AttackActionSystem_OnUIBuff;
        AttackActionSystem.Instance.OffUIBuff += AttackActionSystem_OffUIBuff;
    }

    private void CreateCharacterBuffs()
    {
        DestroyCharacterBuffs();

        Unit selectedUnit = AttackActionSystem.Instance.GetCharacterChainFind();

        foreach (BaseBuff baseBuff in selectedUnit.GetBaseBuffArray())
        {
            if((baseBuff == selectedUnit.GetBuff<KnightSkillBuff>() && baseBuff.GetBuffTurnCount() == 0) ||
                (baseBuff == selectedUnit.GetBuff<SpiderSkillDebuff>() && baseBuff.GetBuffTurnCount() == 0) ||
                (baseBuff == selectedUnit.GetBuff<WizardSkillDebuff>() && baseBuff.GetBuffTurnCount() == 0))
            {
                continue;
            }

            Transform buffButtonTransform = Instantiate(CharacterBuffImagePrefab, CharacterBuffContainerTransform);
            CharacterBuffUI buffButtonUI = buffButtonTransform.GetComponent<CharacterBuffUI>();
            buffButtonUI.Set_NameAndImage(baseBuff, selectedUnit);

            characterBuffUIList.Add(buffButtonUI);
        }
    }

    private void DestroyCharacterBuffs()
    {
        foreach (Transform imageTransform in CharacterBuffContainerTransform)
        {
            Destroy(imageTransform.gameObject);
        }

        characterBuffUIList.Clear();
    }

    private void CreateEnemyBuffs()
    {
        DestroyEnemyBuffs();

        Unit selectedUnit = AttackActionSystem.Instance.GetenemyChainFind();

        foreach (BaseBuff baseBuff in selectedUnit.GetBaseBuffArray())
        {
            if (baseBuff == selectedUnit.GetBuff<WizardSkillDebuff>() && baseBuff.GetBuffTurnCount() == 0)
            {
                continue;
            }

            Transform buffButtonTransform = Instantiate(EnemyBuffImagePrefab, EnemyBuffContainerTransform);
            EnemyBuffUI buffUI = buffButtonTransform.GetComponent<EnemyBuffUI>();
            buffUI.Set_NameAndImage(baseBuff, selectedUnit);

            enemyBuffUIList.Add(buffUI);
        }
    }

    private void DestroyEnemyBuffs()
    {
        foreach (Transform imageTransform in EnemyBuffContainerTransform)
        {
            Destroy(imageTransform.gameObject);
        }

        enemyBuffUIList.Clear();
    }

    private void AttackActionSystem_OnUIBuff(object sender, EventArgs e)
    {
        CreateCharacterBuffs();
        CreateEnemyBuffs();
    }

    private void AttackActionSystem_OffUIBuff(object sender, EventArgs e)
    {
        DestroyCharacterBuffs();
        DestroyEnemyBuffs();
    }
}
