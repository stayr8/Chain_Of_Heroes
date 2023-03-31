using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public partial class TurnSystem : MonoBehaviour
{
    public class TurnProperty : ViewModel
    {
        private int _turnNumber = 0;
        private int _actionPoints = 0;
        private int _allPlayerPoints = 0;
        private int _allEnemyPoints = 0;
        public bool _isPlayerTurn = true;
        public bool _isEnemyPointCheck = false;
        public bool _isTurnEnd = false;
        
        public int TurnNumber
        {
            get
            {
                return _turnNumber;
            }
            set
            {
                Set<int>(ref _turnNumber, value);
            }
        }
        public int ActionPoints
        {
            get
            {
                return _actionPoints;
            }
            set
            {
                Set<int>(ref _actionPoints, value);
            }
        }
        public int AllPlayerPoint
        {
            get
            {
                return _allPlayerPoints;
            }
            set
            {
                Set<int>(ref _allPlayerPoints, value);
            }
        }
        public int AllEnemyPoint
        {
            get
            {
                return _allEnemyPoints;
            }
            set
            {
                Set<int>(ref _allEnemyPoints, value);
            }
        }
        public bool IsPlayerTurn
        {
            get
            {
                return _isPlayerTurn;
            }
            set
            {
                Set<bool>(ref _isPlayerTurn, value);
            }
        }
        public bool IsEnemyPointCheck
        {
            get
            {
                return _isEnemyPointCheck;
            }
            set
            {
                Set<bool>(ref _isEnemyPointCheck, value);
            }
        }
        public bool IsTurnEnd
        {
            get
            {
                return _isTurnEnd;
            }
            set
            {
                Set<bool>(ref _isTurnEnd, value);
            }
        }
    }

    public static TurnProperty Property
    {
        get;
        private set;
    } = new TurnProperty();


    public static event EventHandler OnAnyActionPointsChanged;
    
    //public event EventHandler OnAnyEnemyPoint;
    public event EventHandler OffPlayerGrid;

    private List<Unit> AllEnemyList;
    private int currentEnemyPoint;

    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject victorySystemUI;
    [SerializeField] private GameObject playerVictoryVisualGameObject;
    [SerializeField] private GameObject enemyVictoryVisualGameObject;


    private void Start()
    {
        Initialize();

        BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            NextTurn();

            OnTurnChanged();

            OnAnyEnemyPoint();
        });

        BindingManager.Bind(TurnSystem.Property, "IsTurnEnd", (object value) =>
        {
            if (UnitManager.Instance.VictoryPlayer())
            {
                OnVictorySystemUI();
                OnVictoryPlayerVisual();
                turnNumberText.text = "" + TurnSystem.Property.TurnNumber;
            }
            else if (UnitManager.Instance.VictoryEnemy())
            {
                OnVictorySystemUI();
                OnVictoryEnemyVisual();
                turnNumberText.text = "" + TurnSystem.Property.TurnNumber;
            }

        },false);

        Property.ActionPoints = Property.AllPlayerPoint;
    }

    public void NextTurn()
    {
        if (Property.IsPlayerTurn)
        {
            OffPlayerGrid?.Invoke(this, EventArgs.Empty);
        }

        Property.TurnNumber++;
    }

    private void OnTurnChanged()
    {
        if (!Property.IsPlayerTurn)
        {
            Property.ActionPoints = Property.AllEnemyPoint;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Property.ActionPoints = Property.AllPlayerPoint;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

    }

    private void OnAnyEnemyPoint()
    {
        AllEnemyList = new List<Unit>();
        AllEnemyList = UnitManager.Instance.GetEnemyUnitList();

        currentEnemyPoint = 0;

        for (int i = 0; i < AllEnemyList.Count; i++)
        {
            currentEnemyPoint += AllEnemyList[i].GetEnemyActionPoint();
        }

        if (currentEnemyPoint < Property.AllEnemyPoint)
        {
            
            int newEnemyPoint = Property.AllEnemyPoint - currentEnemyPoint;

            for (int i = 0; i < AllEnemyList.Count; i++)
            {
                if (AllEnemyList[i] != null)
                {
                    AllEnemyList[i].SetnewEnemyActionPoint(newEnemyPoint);
                    break;
                }
            }
            Debug.Log(currentEnemyPoint);
        }

        Property.IsEnemyPointCheck = true;
    }

    private void OnVictorySystemUI()
    {
        victorySystemUI.SetActive(true);
    }

    private void OnVictoryPlayerVisual()
    {
        playerVictoryVisualGameObject.SetActive(true);
    }

    private void OnVictoryEnemyVisual()
    {
        enemyVictoryVisualGameObject.SetActive(true);
    }

}