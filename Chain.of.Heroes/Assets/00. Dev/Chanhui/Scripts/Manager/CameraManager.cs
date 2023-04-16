using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] private GameObject AttackBeforeCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        BaseAction.OnAnyAttackStarted += BaseAction_OnAnyAttackStarted;
        BaseAction.OnAnyAttackCompleted += BaseAction_OnAnyAttackCompleted;

        HideActionCamera();
    }

    // Attack 장면에 있는 카메라
    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    // Attack 전에 들어가는 효과 카메라
    private void ShowAttackActionCamera()
    {
        AttackBeforeCameraGameObject.SetActive(true);
    }

    private void HideAttackActionCamera()
    {
        AttackBeforeCameraGameObject.SetActive(false);
    }


    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        ShowActionCamera();
        
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        HideActionCamera();
        
    }

    private void BaseAction_OnAnyAttackStarted(object sender, EventArgs e)
    {
        ShowAttackActionCamera();
       
    }
    private void BaseAction_OnAnyAttackCompleted(object sender, EventArgs e)
    {
        HideAttackActionCamera();
        
    }

    private void OnDisable()
    {
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
        BaseAction.OnAnyAttackStarted -= BaseAction_OnAnyAttackStarted;
        BaseAction.OnAnyAttackCompleted -= BaseAction_OnAnyAttackCompleted;
    }
}
