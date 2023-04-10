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
    private BaseAction[] baseActionArray;
    private CharacterDataManager characterDataManager;
    private MonsterDataManager monsterDataManager;

    [SerializeField] private int newEnemyActionPoints = 2;
    [SerializeField] private int SoloEnemyActionPoints = 0;

    [SerializeField] private EnemyType enemyType;

    [SerializeField] private String UnitName;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        if (TryGetComponent<CharacterDataManager>(out CharacterDataManager characterdatamanager))
        {
            this.characterDataManager = characterdatamanager;
            characterDataManager.OnDead += CharacterDataManager_OnDead;
        }
        if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterdatamanager))
        {
            this.monsterDataManager = monsterdatamanager;
            monsterDataManager.OnDead += CharacterDataManager_OnDead;
        }

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

    }

    private void Update()
    {
        if (!AttackActionSystem.Instance.OnAttackAtGround)
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

    // �÷��̾� ������ ����
    private void SpendActionPoints(int amount)
    {
        TurnSystem.Property.ActionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    // ���� ������ ����
    private void EnemySpendActionPoints(int amount)
    {
        SoloEnemyActionPoints -= amount;
        TurnSystem.Property.ActionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    // �� ��ġ
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    // ��ġ ����
    public Vector3 SetPosition(Vector3 position)
    {
        return transform.position = position;
    }    
    // �������� �ƴ���
    public bool IsEnemy()
    {
        return isEnemy;
    }
    // ���� ���� ������
    public int GetEnemyActionPoint()
    {
        return newEnemyActionPoints;
    }
    // ���� ������ �� ������
    public int SetnewEnemyActionPoint(int action)
    {
        return newEnemyActionPoints += action;
    }
   
    
    // ����, �÷��̾� ���� �� Ȯ��
    private void CharacterDataManager_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
       

        Destroy(gameObject, 4.0f);
    }

    // �� ǥ��ȭ
    public float GetHealthNormalized()
    {
        return characterDataManager.GetHealthNormalized();
    }
    public float GetHealth()
    {
        if (!isEnemy)
        {
            return characterDataManager.GetHealth();
        }
        else
        {
            return monsterDataManager.GetHealth();
        }
    }
   
    // ���� Ÿ��
    public EnemyType GetEnemyVisualType()
    {
        return enemyType;
    }
    
    public CharacterDataManager GetCharacterDataManager()
    {
        return characterDataManager;
    }

    public MonsterDataManager GetMonsterDataManager()
    {
        return monsterDataManager;
    }

    public String GetUnitName()
    {
        return UnitName;
    }


    private void OnDisable()
    {
        if (TryGetComponent<CharacterDataManager>(out CharacterDataManager characterdatamanager))
        {
            characterDataManager.OnDead -= CharacterDataManager_OnDead;
        }
        if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterdatamanager))
        {
            monsterDataManager.OnDead -= CharacterDataManager_OnDead;
        }
    }
}
