using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private Transform CameraPos;
    [SerializeField] private Transform CameraFollow;
    [SerializeField] private Transform CameraPos2;
    [SerializeField] private Transform CameraPos3;

    private GridPosition gridPosition;
    private BaseAction[] baseActionArray;
    private BaseBuff[] baseBuffArray;
    [SerializeField] private bool IsGrid;
    [SerializeField] private bool IsStun;

    [Header("Monster Information")]
    [SerializeField] private bool isEnemy;
    [SerializeField] private int newEnemyActionPoints = 2;
    [SerializeField] private int SoloEnemyActionPoints = 0;

    private MonsterDataManager monsterDataManager;

    private List<GridPosition> BossMonsterGridPositionList;

    public enum EnemyType
    {
        Empty,
        Archer,
        Sword,
        GoblinLoad,
        RedStoneGolem,
        Dragon,
    }

    [SerializeField] private EnemyType enemyType;

    [Header("Player Information")]
    private CharacterDataManager characterDataManager;
    private bool isChainfirst;
    private bool isChaintwo;

    [SerializeField] private String UnitName;
    [SerializeField] private bool IsLongdistance;
    [SerializeField] private bool isChainPossiblestate;
    [SerializeField] private int _killCount;

    private void Awake()
    {
        baseActionArray = GetComponents<BaseAction>();
        baseBuffArray = GetComponents<BaseBuff>();
        if (TryGetComponent<CharacterDataManager>(out CharacterDataManager characterdatamanager))
        {
            this.characterDataManager = characterdatamanager;
        }
        else if (TryGetComponent<MonsterDataManager>(out MonsterDataManager monsterdatamanager))
        {
            this.monsterDataManager = monsterdatamanager;
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
                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            }
            TurnSystem.Property.IsEnemyPointCheck = false;
        });

        
        BindingManager.Bind(TurnSystem.Property, "IsTurnEnd", (object value) =>
        {
            newEnemyActionPoints = 0;
            SoloEnemyActionPoints = 0;
        },false);

        BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (!TurnSystem.Property.IsPlayerTurn)
            {
                if (enemyType == EnemyType.RedStoneGolem || enemyType == EnemyType.Dragon)
                {
                    MonsterGridPosition(this.gridPosition, false);
                }
            }
            else
            {
                if (enemyType == EnemyType.RedStoneGolem || enemyType == EnemyType.Dragon)
                {
                    MonsterGridPosition(this.gridPosition, true);
                }
            }
        }, false);

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        if(enemyType == EnemyType.RedStoneGolem || enemyType == EnemyType.Dragon)
        {
            MonsterGridPosition(gridPosition, true);
        }

        SoloEnemyActionPoints = newEnemyActionPoints;
        IsGrid = false;
        _killCount = 0;

        if (!isEnemy)
        {
            CharacterDataManager[] _initCDM = DataManager.Instance.GetInitCDM();
            for (int i = 0; i < 8; i++)
            {
                if (_initCDM[i].CharacterName == characterDataManager.CharacterName)
                {
                    characterDataManager.NumForLvUp = _initCDM[i].NumForLvUp;
                    characterDataManager.m_currentExp = _initCDM[i].m_currentExp;
                }
            }
        }
    }

    private void Update()
    {
        if (!IsGrid)
        {
            GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition != gridPosition)
            {
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

    public T GetBuff<T>() where T : BaseBuff
    {
        foreach (BaseBuff baseBuff in baseBuffArray)
        {
            if (baseBuff is T)
            {
                return (T)baseBuff;
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

    public BaseBuff[] GetBaseBuffArray()
    {
        return baseBuffArray;
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

    public void MonsterGridPosition(GridPosition unitGridPosition, bool move)
    {
        int maxMonsterDistance = 1;
        BossMonsterGridPositionList = new List<GridPosition>();

        for (int x = -maxMonsterDistance; x <= maxMonsterDistance; x++)
        {
            for (int z = -maxMonsterDistance; z <= maxMonsterDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same Grid Position where the character is already at
                    continue;
                }

                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    Unit player = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if(!player.IsEnemy())
                    {
                        continue;
                    }
                }

                if(testGridPosition == null)
                {
                    continue;
                }

                if (move)
                {
                    LevelGrid.Instance.AddUnitAtGridPosition(testGridPosition, this);
                    BossMonsterGridPositionList.Add(testGridPosition);
                }
                else
                {
                    LevelGrid.Instance.RemoveUnitAtGridPosition(testGridPosition, this);
                    BossMonsterGridPositionList.Remove(testGridPosition);
                }
            }
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
    public List<GridPosition> GetBossMonsterPosition()
    {
        return BossMonsterGridPositionList;
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

    public Transform GetCameraPos()
    {
        return CameraPos;
    }
    public Transform GetCameraPos2()
    {
        return CameraPos2;
    }
    public Transform GetCameraPos3()
    {
        return CameraPos3;
    }
    public Transform GetCameraFollow()
    {
        return CameraFollow;
    }

    public bool GetIsAttackDistance()
    {
        return IsLongdistance;
    }

    // �� ǥ��ȭ
    public float GetHealthNormalized()
    {
        return characterDataManager.GetHealthNormalized();
    }

    public float GetEnemyHealthNormalized()
    {
        return monsterDataManager.GetHealthNormalized();
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

    public void GetAnyUnitDead()
    {
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    public bool GetIsGrid()
    {
        return IsGrid;
    }

    public void SetIsGrid(bool IsGrid)
    {
        this.IsGrid = IsGrid;
    }

    public bool GetIsStun()
    {
        return IsStun;
    }
    public void SetIsStun(bool IsStun)
    {
        this.IsStun = IsStun;
    }

    public bool GetIsChainPossibleState()
    {
        return isChainPossiblestate;
    }

    public void SetIsChainPossibleState(bool isChainPossiblestate)
    {
        this.isChainPossiblestate = isChainPossiblestate;
    }

    public bool GetChainfirst()
    {
        return isChainfirst;
    }

    public bool GetChaintwo()
    {
        return isChaintwo;
    }

    public void SetChainfirst(bool isChainfirst)
    {
        this.isChainfirst = isChainfirst;
    }

    public void SetChaintwo(bool isChaintwo)
    {
        this.isChaintwo = isChaintwo;
    }

    public int GetKillCount()
    {
        return _killCount;
    }
    public void SetKillCount()
    {
        _killCount++;
    }

    public void SetSoloEnemyActionPoints(int SoloEnemyActionPoints)
    {
        this.SoloEnemyActionPoints = SoloEnemyActionPoints;
    }
}
