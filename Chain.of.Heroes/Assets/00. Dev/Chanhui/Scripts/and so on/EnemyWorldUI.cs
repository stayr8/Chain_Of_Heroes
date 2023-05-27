using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyWorldUI : MonoBehaviour
{

    [SerializeField] private Unit enemy;
    [SerializeField] private MonsterDataManager monsterDataManager;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private GameObject healthbarUI;


    private void Start()
    {
        monsterDataManager.OnEnemyDamage += MonsterDataManager_OnEnemyDamage;
        monsterDataManager.OnEnemyDie += MonsterDataManager_OnEnemyDamage;
        AttackActionSystem.OnActionStarted += AttackActionSystem_OnActionStarted;
        AttackActionSystem.OnActionCompleted += AttackActionSystem_OnActionCompleted;
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = enemy.GetEnemyHealthNormalized();
    }

    private void MonsterDataManager_OnEnemyDamage(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void AttackActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        healthbarUI.SetActive(false);
    }
    private void AttackActionSystem_OnActionCompleted(object sender, EventArgs e)
    {
        healthbarUI.SetActive(true);
    }

    private void OnDisable()
    {
        monsterDataManager.OnEnemyDamage -= MonsterDataManager_OnEnemyDamage;
        monsterDataManager.OnEnemyDie -= MonsterDataManager_OnEnemyDamage;
        AttackActionSystem.OnActionStarted -= AttackActionSystem_OnActionStarted;
        AttackActionSystem.OnActionCompleted -= AttackActionSystem_OnActionCompleted;
    }
}
