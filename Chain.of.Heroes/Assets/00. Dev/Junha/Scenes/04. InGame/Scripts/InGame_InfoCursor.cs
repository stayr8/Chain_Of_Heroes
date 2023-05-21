using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_InfoCursor : CursorBase
{
    private RectTransform rt;
    public static GameObject currentSelected;

    [SerializeField] Transform Parent;
    private List<PartyInfoCell> Cells = new List<PartyInfoCell>();

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        for(int i = 0; i < 8; i++)
        {
            var Cell = Instantiate(Resources.Load<PartyInfoCell>("PartyInfoCell"), Parent);
            Cell.name = "_" + i;
            Cell.gameObject.SetActive(false);
            Cells.Add(Cell);
        }
        
        for(int i = 0; i < Cells.Count; i++)
        {
            Navigation NewNav = new Navigation();
            NewNav.mode = Navigation.Mode.Explicit;
            NewNav.selectOnUp = i == 0 ? Cells[7].Selectable : Cells[i - 1].Selectable;
            NewNav.selectOnDown = i == 7 ? Cells[0].Selectable : Cells[i + 1].Selectable;

            Cells[i].Selectable.navigation = NewNav;
        }
        Cells[0].gameObject.SetActive(true);

        if (UnitManager.Instance)
        {
            var UnitList = UnitManager.Instance.GetFriendlyUnitList();

            for(int i = 0; i < UnitList.Count; i ++)
            {
                Cells[i].gameObject.SetActive(true);
                Cells[i].UpdateText(UnitList[i]);
            }
        }
    }

    private const float INIT_X = 3.5f; private const float INIT_Y = 310f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_0");
    }

    private const float MOVE_DISTANCE = 110f;
    private const float MAX_POSITION_Y = 310f;
    private const float MIN_POSITION_Y = -460f;

    private void Update()
    {
        Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_Y, MAX_POSITION_Y);
    }
}