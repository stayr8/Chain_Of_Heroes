using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OffSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler OffSelectedActionChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask UnitLayerMask;
    [SerializeField] private MonsterBase selectedEnemy;
    [SerializeField] private LayerMask EnemyLayerMask;
    [SerializeField] private Unit selectedUnitEnemy;

    private BaseAction selectedAction;
    private bool isBusy;
    [SerializeField] private bool DoubleSelectedUnit;
    [SerializeField] private bool CameraSelectedUnit;


    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There's more than one CharacterActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        DoubleSelectedUnit = true;
        CameraSelectedUnit = true;
    }

    private void Update()
    {
        TryHandleEnemySelection();
        TryHandleUnitEnemySelection();
        if (isBusy)
        {
            return;
        }

        //  몬스터 턴
        if(!TurnSystem.Property.IsPlayerTurn)
        {
            return;
        }
        

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
        { 
            return; 
        }

        HandleSelectedAction();

    }

    // 움직임 결정
    private void HandleSelectedAction()
    {
        if(InputManager.Instance.IsMouseButtonDown())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SetBusy()
    {
        isBusy = true;

    }

    private void ClearBusy()
    {
        isBusy = false;

    }

    private bool TryHandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, UnitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {

                    if (unit == selectedUnit && !DoubleSelectedUnit)
                    {
                        // character is already selected
                        OutSelectedUnit(unit);
                        return true;
                    }

                    
                    if(unit.IsEnemy())
                    {
                        // Clicked on an enemy
                        return false;
                    }
                    
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryHandleUnitEnemySelection()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, EnemyLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    SetSelectedUnityEnemy(unit);
                    return true;
                }
            }
        }

        return false;
    }

    public bool TryHandleEnemySelection()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, EnemyLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<MonsterBase>(out MonsterBase unit))
                {
                    Debug.Log("클릭");
                    

                    SetSelectedEnemy(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;

        SetSelectedAction(unit.GetAction<MoveAction>());

        DoubleSelectedUnit = false;

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SetSelectedEnemy(MonsterBase unit)
    {
        selectedEnemy = unit;
    }

    private void SetSelectedUnityEnemy(Unit unit)
    {
        selectedUnitEnemy = unit;
        CameraSelectedUnit = false;
    }

    public void OutSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        DoubleSelectedUnit = true;

        SetSelectedAction(unit.GetAction<EmptyAction>());

        OffSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        OffSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelecterdUnit()
    {
        return selectedUnit;
    }

    public Unit GetSelecterdUnitEnemy()
    {
        return selectedUnitEnemy;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    // MonsterBase 값을 받아오는 구간.
    // 클릭 전에는 호출 하지 않기. (빈 값을 가져가 오류가 생길 수 있기 때문에)
    public MonsterBase GetSelectedEnemy()
    { 
        return selectedEnemy;
    }

    public bool GetDoubleSelUnit()
    {
        return DoubleSelectedUnit;
    }

    public void SetDoubleSelUnit(bool DoubleSelectedUnit)
    {
        this.DoubleSelectedUnit = DoubleSelectedUnit;
    }
    public bool GetCameraSelUnit()
    {
        return CameraSelectedUnit;
    }

    public void SetCameraSelUnit(bool CameraSelectedUnit)
    {
        this.CameraSelectedUnit = CameraSelectedUnit;
    }
}
