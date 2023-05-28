using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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