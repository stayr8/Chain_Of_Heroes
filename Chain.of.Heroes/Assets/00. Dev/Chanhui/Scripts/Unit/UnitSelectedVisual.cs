using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedCharacterChanged;
        UnitActionSystem.Instance.OffSelectedUnitChanged += UnitActionSystem_OffSelectedCharacterChanged;


        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedCharacterChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UnitActionSystem_OffSelectedCharacterChanged(object sender, EventArgs empty)
    {
        OffVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelecterdUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }

    private void OffVisual()
    {
        meshRenderer.enabled = false;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedCharacterChanged;
        UnitActionSystem.Instance.OffSelectedUnitChanged -= UnitActionSystem_OffSelectedCharacterChanged;
    }
}
