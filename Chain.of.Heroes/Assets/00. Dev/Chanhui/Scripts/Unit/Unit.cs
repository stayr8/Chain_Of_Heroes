using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;


    [SerializeField] private bool isEnemy;

    public enum EnemyType
    {
        Empty,
        Archer,
        Sword,
    }

    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private BaseAction[] baseActionArray;
    private CharacterDataManager characterDataManager;

    [SerializeField] private int newEnemyActionPoints = 2;
    [SerializeField] private int SoloEnemyActionPoints = 0;

    [SerializeField] private EnemyType enemyType;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
        characterDataManager = GetComponent<CharacterDataManager>();
    }

    private void Start()
    {
        transform.rotation = Quaternion.Euler((isEnemy == false) ? Vector3.zero : new Vector3(0, 180, 0));

        BindingManager.Bind(TurnSystem.Property, "IsEnemyPointCheck", (object value) =>
        {
            if (IsEnemy() && !TurnSystem.Property.IsPlayerTurn)
            {
                SoloEnemyActionPoints = newEnemyActionPoints;
                //newEnemyActionPoints = 2;
                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            }
            TurnSystem.Property.IsEnemyPointCheck = false;
        });

        BindingManager.Bind(TurnSystem.Property, "IsTurnEnd", (object value) =>
        {
            newEnemyActionPoints = 0;
            SoloEnemyActionPoints = 0;
        },false);

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        SoloEnemyActionPoints = newEnemyActionPoints;

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void Update()
    {
        if (!AttackActionSystem.Instance.attacking)
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
                // Character changed Grid Position
                GridPosition oldGridPosition = gridPosition;
                gridPosition = newGridPosition;

                LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
            }
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
        {
            if(baseAction is T)
            {
                return (T)baseAction;
            }
        }
        return null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (!isEnemy)
        {
            if (CanSpendActionPointsToTakeAction(baseAction))
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (CanEnemySpendActionPointsToTakeAction(baseAction))
            {
                EnemySpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }
            else
            {
                return false;
            }
        }
    }


    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(TurnSystem.Property.ActionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanEnemySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (SoloEnemyActionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // 플레이어 포인터 감소
    private void SpendActionPoints(int amount)
    {
        TurnSystem.Property.ActionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    // 몬스터 포인터 감소
    private void EnemySpendActionPoints(int amount)
    {
        SoloEnemyActionPoints -= amount;
        TurnSystem.Property.ActionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    // 내 위치
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    // 위치 설정
    public Vector3 SetPosition(Vector3 position)
    {
        return transform.position = position;
    }    
    // 몬스터인지 아닌지
    public bool IsEnemy()
    {
        return isEnemy;
    }
    // 현재 몬스터 포인터
    public int GetEnemyActionPoint()
    {
        return newEnemyActionPoints;
    }
    // 몬스터 포인터 값 얻어오기
    public int SetnewEnemyActionPoint(int action)
    {
        return newEnemyActionPoints += action;
    }
    // 몬스터, 플레이어 데미지 부분
    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
    }
    // 몬스터, 플레이어 죽음 및 확인
    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        /*
        if (UnitManager.Instance.VictoryPlayer() || UnitManager.Instance.VictoryEnemy())
        {
            TurnSystem.Property.IsTurnEnd = true;
        }*/

        Destroy(gameObject, 4.0f);
    }
    // 피 표준화
    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }
    public float GetHealth()
    {
        return healthSystem.GetHealth();
    }
    // 몬스터 타입
    public EnemyType GetEnemyVisualType()
    {
        return enemyType;
    }

    public CharacterDataManager GetCharacterDataManager()
    {
        return characterDataManager;
    }

    private void OnDisable()
    {
        healthSystem.OnDead -= HealthSystem_OnDead;
    }
}
