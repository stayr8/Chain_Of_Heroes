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
        DestroyActionButton();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelecterdUnit();

        foreach (BaseBuff baseBuff in selectedUnit.GetBaseBuffArray())
        {
            Transform buffButtonTransform = Instantiate(CharacterBuffImagePrefab, CharacterBuffContainerTransform);
            CharacterBuffUI buggButtonUI = buffButtonTransform.GetComponent<CharacterBuffUI>();
            buggButtonUI.SetBaseAction(baseBuff);

            characterBuffUIList.Add(buggButtonUI);
        }
    }

    private void DestroyActionButton()
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
        DestroyActionButton();
    }
}
