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
    [SerializeField] private CharacterBase selectedEnemy;
    [SerializeField] private LayerMask EnemyLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;
    [SerializeField] private bool DoubleSelectedUnit;

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
    }

    private void Update()
    {
        
        if (isBusy)
        {
            return;
        }
        //  몬스터 턴
        if(!TurnSystem.Property.IsPlayerTurn)
        {
            return;
        }

        if(EventSystem.current.IsPointerOverGameObject())
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

    public bool TryHandleEnemySelection()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, EnemyLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<CharacterBase>(out CharacterBase unit))
                {

                    if (unit == selectedEnemy)
                    {
                        // character is already selected
                        return true;
                    }

                    /*
                    if (!unit.IsEnemy())
                    {
                        // Clicked on an enemy
                        return false;
                    }*/

                    SetSelectedEnemy(selectedEnemy);
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

    private void SetSelectedEnemy(CharacterBase unit)
    {
        selectedEnemy = unit;
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

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    public void SetDoubleSelUnit(bool DoubleSelectedUnit)
    {
        this.DoubleSelectedUnit = DoubleSelectedUnit;
    }
}
