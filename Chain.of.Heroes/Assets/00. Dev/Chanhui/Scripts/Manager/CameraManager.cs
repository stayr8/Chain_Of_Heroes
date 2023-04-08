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
        switch(sender)
        {
            case ReadyAction shootAction:
                /*
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.3f;

                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                */

                ShowActionCamera();
                break;
            case QueenAction queenAction:
                /*
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.3f;

                Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDir * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeight + shoulderOffset + (shootDir * -1);
                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);
                */

                ShowActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ReadyAction shootAction:
                HideActionCamera();
                break;
            case QueenAction queenAction:
                HideActionCamera();
                break;
        }
    }

    private void BaseAction_OnAnyAttackStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case QueenAction queenAction:
                ShowAttackActionCamera();
                break;
        }
    }
    private void BaseAction_OnAnyAttackCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case QueenAction queenAction:
                HideAttackActionCamera();
                break;
        }
    }

    private void OnDisable()
    {
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }
}
