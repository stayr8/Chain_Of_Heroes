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
    private List<CharacterBuffUI> enemyBuffUIList;

    private void Awake()
    {
        characterBuffUIList = new List<CharacterBuffUI>();
        enemyBuffUIList = new List<CharacterBuffUI>();
    }

    private void Start()
    {
        AttackActionSystem.Instance.OnUIBuff += AttackActionSystem_OnUIBuff;
        AttackActionSystem.Instance.OffUIBuff += AttackActionSystem_OffUIBuff;
    }

    private void CreateUnitActionButtons()
    {
        DestroyBuffButton();

        Unit selectedUnit = AttackActionSystem.Instance.GetCharacterChainFind();

        foreach (BaseBuff baseBuff in selectedUnit.GetBaseBuffArray())
        {
            Transform buffButtonTransform = Instantiate(CharacterBuffImagePrefab, CharacterBuffContainerTransform);
            CharacterBuffUI buffButtonUI = buffButtonTransform.GetComponent<CharacterBuffUI>();
            buffButtonUI.Set_NameAndImage(selectedUnit.GetCharacterDataManager());
            buffButtonUI.SetBaseAction(baseBuff);

            characterBuffUIList.Add(buffButtonUI);
        }
    }

    private void DestroyBuffButton()
    {
        foreach (Transform imageTransform in CharacterBuffContainerTransform)
        {
            Destroy(imageTransform.gameObject);
        }

        characterBuffUIList.Clear();
    }

    private void AttackActionSystem_OnUIBuff(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void AttackActionSystem_OffUIBuff(object sender, EventArgs e)
    {
        DestroyBuffButton();
    }
}
