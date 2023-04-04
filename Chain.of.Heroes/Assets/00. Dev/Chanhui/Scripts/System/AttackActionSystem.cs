using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionSystem : MonoBehaviour
{
    public static AttackActionSystem Instance { get; private set; }

    private Vector3 playerpos;
    private Vector3 enemypos;

    public bool attacking = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one AttackActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        attacking = false;
    }


    public void OnAtLocationMove(Unit unit, Unit enemy)
    {
        attacking = true;

        playerpos = unit.GetWorldPosition();
        enemypos = enemy.GetWorldPosition();

        Vector3 playerlocationMove = new Vector3(0, 150, -3);

        Vector3 enemylocationMove = new Vector3(0, 150, 3);

       
        LevelGrid.Instance.RemoveUnitAtGridPosition(unit.GetGridPosition(), unit);
        LevelGrid.Instance.RemoveUnitAtGridPosition(enemy.GetGridPosition(), enemy);

        unit.SetPosition(playerlocationMove);
        enemy.SetPosition(enemylocationMove);
        unit.transform.rotation = Quaternion.Euler(0, 0, 0);
        enemy.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void OffAtLocationMove(Unit unit, Unit enemy)
    {
        unit.SetPosition(playerpos);
        enemy.SetPosition(enemypos);

        if (unit.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(unit.GetGridPosition(), unit);
        }
        
        if (enemy.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(enemy.GetGridPosition(), enemy);
        }
        attacking = false;
    }
}
