using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackActionSystem : MonoBehaviour
{
    public static AttackActionSystem Instance { get; private set; }

    private Vector3 playerpos;
    private Vector3 enemypos;
    private Quaternion playerrotation;
    private Quaternion enemyrotation;

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


    public void OnAtLocationMove(Unit unit, Unit target)
    {
        attacking = true;

        playerpos = unit.GetWorldPosition();
        enemypos = target.GetWorldPosition();
        playerrotation = unit.transform.rotation;
        enemyrotation = unit.transform.rotation;

        Vector3 playerlocationMove = new Vector3(0, 150, -3);

        Vector3 enemylocationMove = new Vector3(0, 150, 3);

       
        LevelGrid.Instance.RemoveUnitAtGridPosition(unit.GetGridPosition(), unit);
        LevelGrid.Instance.RemoveUnitAtGridPosition(target.GetGridPosition(), target);

        unit.SetPosition(playerlocationMove);
        target.SetPosition(enemylocationMove);

        unit.transform.rotation = Quaternion.Euler(0, 0, 0);
        target.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void OffAtLocationMove(Unit unit, Unit target)
    {
        unit.SetPosition(playerpos);
        if (target != null)
        {
            target.SetPosition(enemypos);
        }

        if (unit.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(unit.GetGridPosition(), unit);
            unit.transform.rotation = playerrotation;
        }
        
        if (target.GetHealth() > 0)
        {
            LevelGrid.Instance.AddUnitAtGridPosition(target.GetGridPosition(), target);
            target.transform.rotation = enemyrotation;
        }
        attacking = false;
    }
}
