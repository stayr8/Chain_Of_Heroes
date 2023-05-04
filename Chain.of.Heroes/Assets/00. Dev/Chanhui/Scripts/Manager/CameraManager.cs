using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private GameObject actionCameraGameObject;
    [SerializeField] private GameObject actionCameraGameObject_1;
    [SerializeField] private GameObject AttackBeforeCameraGameObject;

    [SerializeField] private GameObject ChainAttackui;

    private void Start()
    {
        BaseAction.OnAnyAttackStarted += BaseAction_OnAnyAttackStarted;
        BaseAction.OnAnyAttackCompleted += BaseAction_OnAnyAttackCompleted;

        BaseAction.OnAnyActionStarted_1 += BaseAction_OnAnyActionStarted_1;
        BaseAction.OnAnyActionCompleted_1 += BaseAction_OnAnyActionCompleted_1;

        AttackActionSystem.OnActionStarted += AttackActionSystem_OnActionStarted;
        AttackActionSystem.OnActionCompleted += AttackActionSystem_OnActionCompleted;

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

    // Chain Attack 카메라 효과
    private void ShowActionCamera_1()
    {
        actionCameraGameObject_1.SetActive(true);
        ChainAttackui.SetActive(true);
    }

    private void HideActionCamera_1()
    {
        actionCameraGameObject_1.SetActive(false);
        ChainAttackui.SetActive(false);
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


    private void AttackActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        ShowActionCamera();
        
    }

    private void AttackActionSystem_OnActionCompleted(object sender, EventArgs e)
    {
        HideActionCamera();
        
    }

    private void BaseAction_OnAnyActionStarted_1(object sender, EventArgs e)
    {
        ShowActionCamera_1();

    }

    private void BaseAction_OnAnyActionCompleted_1(object sender, EventArgs e)
    {
        HideActionCamera_1();

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
        BaseAction.OnAnyAttackStarted -= BaseAction_OnAnyAttackStarted;
        BaseAction.OnAnyAttackCompleted -= BaseAction_OnAnyAttackCompleted;

        BaseAction.OnAnyActionStarted_1 -= BaseAction_OnAnyActionStarted_1;
        BaseAction.OnAnyActionCompleted_1 -= BaseAction_OnAnyActionCompleted_1;

        AttackActionSystem.OnActionStarted -= AttackActionSystem_OnActionStarted;
        AttackActionSystem.OnActionCompleted -= AttackActionSystem_OnActionCompleted;
    }
}
