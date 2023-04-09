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


    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

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
