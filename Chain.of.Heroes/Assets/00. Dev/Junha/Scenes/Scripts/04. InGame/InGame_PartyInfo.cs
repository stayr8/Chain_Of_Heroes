using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGame_PartyInfo : CursorBase
{
    [SerializeField] Transform Parent;
    private List<PartyInfoCell> Cells = new List<PartyInfoCell>();

    private void Awake()
    {
        for (int i = 0; i < 8; i++)
        {
            var Cell = Instantiate(Resources.Load<PartyInfoCell>("PartyInfoCell"), Parent);
            Cell.name = "_" + i;
            Cell.gameObject.SetActive(false);
            Cells.Add(Cell);
        }

        for (int i = 0; i < Cells.Count; i++)
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

            for (int i = 0; i < UnitList.Count; i++)
            {
                Cells[i].gameObject.SetActive(true);
                Cells[i].UpdateText(UnitList[i]);
            }
        }
    }
}