using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GridSystemVisual;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }


    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        Empty,
        White,
        Blue,
        Red,
        RedSoft,
        Yellow,
        RedMiddle,
        Green,
    }

    [SerializeField] private Transform gridSystemVisualSingPrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;


    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private List<Binding> Binds = new List<Binding>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one CharacterActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Binding Bind = BindingManager.Bind(TurnSystem.Property, "IsPlayerTurn", (object value) =>
        {
            if (!TurnSystem.Property.IsPlayerTurn)
            {
                ShowAllGridPosition();
            }
        },false);
        Binds.Add(Bind);

        gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
            ];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridSystemVisualSingleTransform = Instantiate(gridSystemVisualSingPrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }

        List<GridPosition> gridPositionList = new List<GridPosition>();
        GridPosition StartPosition = new GridPosition(0, 0);
        for (int z = 0; z < 3; z++)
        {
            for(int x = 0; x < 4; x++)
            {
                GridPosition testGridPosition = StartPosition  + new GridPosition(x, z);
                gridPositionList.Add(testGridPosition);
            }
        }
        ShowGridPositionList(gridPositionList, GridVisualType.Blue);

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OffSelectedActionChanged += UnitActionSystem_OffSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

        //UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }
    public void ShowAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                gridSystemVisualSingleArray[x, z].Show(GetGridVisualMaterial());
            }
        }
    }

    private void ShowSingleGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                if(gridPosition != testGridPosition)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -range; x <= range; x++)
        {
            for(int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionRookRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if ((testX != 0) && (testZ != 0))
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionKingRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }


                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionBishopRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if (testX != testZ)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionQueenRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if ((testX != 0) && (testZ != 0) && (testX != testZ))
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    private void ShowGridPositionKnightRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        List<GridPosition> gridPositionList = new List<GridPosition>();
        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                
                int testX = Mathf.Abs(x);
                int testZ = Mathf.Abs(z);
                if (testX == 0 || testZ == 0 || testX == testZ)
                {
                    continue;
                }

                gridPositionList.Add(testGridPosition);
            }
        }

        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }
    public void DestroyGridPositionList()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                Destroy(gridSystemVisualSingleArray[x, z].gameObject);
            }
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelecterdUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;              
                break;
            case EmptyAction EmptyAction:
                gridVisualType = GridVisualType.Empty;

                break;
            case ReadyAction readAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), readAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case KingAction kingAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), kingAction.GetMaxKingDistance(), GridVisualType.RedMiddle);
                break;
            case RookAction rookAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), rookAction.GetMaxRookDistance(), GridVisualType.RedSoft);
                ShowGridPositionRookRange(selectedUnit.GetGridPosition(), rookAction.GetMaxRookDistance(), GridVisualType.RedMiddle);
                break;
            case BishopAction bishopAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), bishopAction.GetMaxBishopDistance(), GridVisualType.RedSoft);
                ShowGridPositionBishopRange(selectedUnit.GetGridPosition(), bishopAction.GetMaxBishopDistance(), GridVisualType.RedMiddle);
                break;
            case QueenAction queenAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), queenAction.GetMaxQueenDistance(), GridVisualType.RedSoft);
                ShowGridPositionQueenRange(selectedUnit.GetGridPosition(), queenAction.GetMaxQueenDistance(), GridVisualType.RedMiddle);
                break;
            case KnightAction knightAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), knightAction.GetMaxKnightDistance(), GridVisualType.RedSoft);
                ShowGridPositionKnightRange(selectedUnit.GetGridPosition(), knightAction.GetMaxKnightDistance(), GridVisualType.RedMiddle);
                break;
            case LongKnightAction longKnightAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), longKnightAction.GetMaxArcherDistance(), GridVisualType.RedSoft);
                ShowGridPositionKnightRange(selectedUnit.GetGridPosition(), longKnightAction.GetMaxArcherDistance(), GridVisualType.RedMiddle);
                break;
            case LongBishopAction longBishopAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), longBishopAction.GetMaxWizardDistance(), GridVisualType.RedSoft);
                ShowGridPositionBishopRange(selectedUnit.GetGridPosition(), longBishopAction.GetMaxWizardDistance(), GridVisualType.RedMiddle);
                break;
            case KnightAttackAction knightAttackAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), knightAttackAction.GetMaxKingDistance(), GridVisualType.RedMiddle);
                break;
            case ChainAction chainAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), chainAction.GetMaxChainDistance(), GridVisualType.RedSoft);
                break;
            case SwordWomanSkill1Action swordWomanSkill1Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), swordWomanSkill1Action.GetMaxSWSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionRookRange(selectedUnit.GetGridPosition(), swordWomanSkill1Action.GetMaxSWSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case SwordWomanSkill2Action swordWomanSkill2Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), swordWomanSkill2Action.GetMaxSWSkill_2_Distance(), GridVisualType.RedSoft);
                ShowGridPositionRookRange(selectedUnit.GetGridPosition(), swordWomanSkill2Action.GetMaxSWSkill_2_Distance(), GridVisualType.RedMiddle);
                break;
            case KnightSkill1Action knightSkill1Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), knightSkill1Action.GetMaxSWSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), knightSkill1Action.GetMaxSWSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case KnightSkill2Action knightSkill2Action:
                gridVisualType = GridVisualType.Green;

                ShowSingleGridPositionRange(selectedUnit.GetGridPosition(), knightSkill2Action.GetMaxSWSkill_2_Distance(), gridVisualType);
                break;
            case SamuraiSkill1Action samuraiSkill1Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), samuraiSkill1Action.GetMaxSrSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionKnightRange(selectedUnit.GetGridPosition(), samuraiSkill1Action.GetMaxSrSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case SamuraiSkill2Action samuraiSkill2Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), samuraiSkill2Action.GetMaxSrSkill_2_Distance(), GridVisualType.RedSoft);
                ShowGridPositionKnightRange(selectedUnit.GetGridPosition(), samuraiSkill2Action.GetMaxSrSkill_2_Distance(), GridVisualType.RedMiddle);
                break;
            case ArcherSkill1Action archerSkill1Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), archerSkill1Action.GetMaxArSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionKnightRange(selectedUnit.GetGridPosition(), archerSkill1Action.GetMaxArSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case ArcherSkill2Action archerSkill2Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), archerSkill2Action.GetMaxArSkill_2_Distance(), GridVisualType.RedSoft);
                ShowGridPositionKnightRange(selectedUnit.GetGridPosition(), archerSkill2Action.GetMaxArSkill_2_Distance(), GridVisualType.RedMiddle);
                break;
            case GuardianSkill1Action guardianSkill1Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRange(selectedUnit.GetGridPosition(), guardianSkill1Action.GetMaxGdSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), guardianSkill1Action.GetMaxGdSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case GuardianSkill2Action guardianSkill2Action:
                gridVisualType = GridVisualType.Green;

                ShowSingleGridPositionRange(selectedUnit.GetGridPosition(), guardianSkill2Action.GetMaxGdSkill_2_Distance(), gridVisualType);
                break;
            case PriestSkill1Action priestSkill1Action:
                gridVisualType = GridVisualType.Yellow;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), priestSkill1Action.GetMaxPsSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionBishopRange(selectedUnit.GetGridPosition(), priestSkill1Action.GetMaxPsSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case PriestSkill2Action priestSkill2Action:
                gridVisualType = GridVisualType.Yellow;

                ShowSingleGridPositionRange(selectedUnit.GetGridPosition(), priestSkill2Action.GetMaxPsSkill_2_Distance(), GridVisualType.RedMiddle);
                break;
            case WizardSkill1Action wizardSkill1Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), wizardSkill1Action.GetMaxWzSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionBishopRange(selectedUnit.GetGridPosition(), wizardSkill1Action.GetMaxWzSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case WizardSkill2Action wizardSkill2Action:
                gridVisualType = GridVisualType.Red;

                ShowSingleGridPositionRange(selectedUnit.GetGridPosition(), wizardSkill2Action.GetMaxWzSkill_2_Distance(), GridVisualType.RedMiddle);
                ShowGridPositionBishopRange(selectedUnit.GetGridPosition(), wizardSkill2Action.GetMaxWzSkill_2_Distance(), GridVisualType.RedMiddle);
                break;
            case ValkyrieSkill1Action valkyrieSkill1Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), valkyrieSkill1Action.GetMaxVkSkill_1_Distance(), GridVisualType.RedSoft);
                ShowGridPositionQueenRange(selectedUnit.GetGridPosition(), valkyrieSkill1Action.GetMaxVkSkill_1_Distance(), GridVisualType.RedMiddle);
                break;
            case ValkyrieSkill2Action valkyrieSkill2Action:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionKingRange(selectedUnit.GetGridPosition(), valkyrieSkill2Action.GetMaxVkSkill_2_Distance(), GridVisualType.RedSoft);
                ShowGridPositionQueenRange(selectedUnit.GetGridPosition(), valkyrieSkill2Action.GetMaxVkSkill_2_Distance(), GridVisualType.RedMiddle);
                break;
        }

        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void UnitActionSystem_OffSelectedActionChanged(object sender, EventArgs e)
    {
        HideAllGridPosition();
    }

    public void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if(gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;
    }

    private Material GetGridVisualMaterial()
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            return gridVisualTypeMaterial.material;
        }

        
        return null;
    }

    private void OnDisable()
    {
        foreach (var bind in Binds)
        {
            BindingManager.Unbind(TurnSystem.Property, bind);
        }

        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OffSelectedActionChanged -= UnitActionSystem_OffSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition -= LevelGrid_OnAnyUnitMovedGridPosition;
    }
}
