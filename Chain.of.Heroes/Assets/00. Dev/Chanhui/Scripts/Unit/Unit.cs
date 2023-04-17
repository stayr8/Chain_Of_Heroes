using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    public event EventHandler OnUnitDamage;
    public event EventHandler OnUnitDie;


    [SerializeField] private bool isEnemy;
    [SerializeField] private Transform CameraPos;
    [SerializeField] private Transform CameraFollow;

    public enum EnemyType
    {
        Empty,
        Archer,
        Sword,
    }

    private GridPosition gridPosition;
    private BaseAction[] baseActionArray;
    private CharacterDataManager characterDataManager;
    private CharacterBase characterBase;
    private MonsterDataManager monsterDataManager;
    private MonsterBase monsterBase;

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
        }
        else if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterdatamanager))
        {
            this.monsterDataManager = monsterdatamanager;
        }

        if (TryGetComponent<CharacterBase>(out CharacterBase characterBase))
        {
            this.characterBase = characterBase;
        }
        else if (TryGetComponent<MonsterBase>(out MonsterBase monsterBase))
        {
            this.monsterBase = monsterBase;
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
        if (!AttackActionSystem.Instance.OnAttackGroundCheck())
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
   
    
    // 몬스터, 플레이어 죽음 및 피격!
    public void Damage()
    {
        if(IsEnemy())
        {
            monsterBase.Calc_Attack(AttackActionSystem.Instance.GetCharacterDataManager(), monsterDataManager);

            if (monsterDataManager.GetHealth() <= 0)
            {
                OnUnitDie?.Invoke(this, EventArgs.Empty);
                OnAnyUnitDead?.Invoke(this, EventArgs.Empty);

                LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

                Destroy(gameObject, 4.0f);

            }
            else
            {
                OnUnitDamage?.Invoke(this, EventArgs.Empty);
            }
        }
        else if(!IsEnemy())
        {
            characterBase.Calc_Attack(characterDataManager, AttackActionSystem.Instance.GetMonsterDataManager());

            if (characterDataManager.GetHealth() <= 0)
            {
                OnUnitDie?.Invoke(this, EventArgs.Empty);
                OnAnyUnitDead?.Invoke(this, EventArgs.Empty);

                LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);

                Destroy(gameObject, 4.0f);

            }
            else
            {
                OnUnitDamage?.Invoke(this, EventArgs.Empty);
                
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Melee")
        {
            ScreenShake.Instance.Shake();
            Damage();
        }
    }


    // 피 표준화
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
   
    // 몬스터 타입
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

    public Transform GetCameraPos()
    {
        return CameraPos;
    }
    public Transform GetCameraFollow()
    {
        return CameraFollow;
    }
}
