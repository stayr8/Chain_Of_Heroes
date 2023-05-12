using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualingCamera;
    [SerializeField] private GameObject TopViewVirtualingCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    public Transform actiontr;

    public bool camerazoom;
    private Vector3 mousepos;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        ScenesSystem.Instance.OnScenesChange += ScenesSystem_OnScenesChange;
    }

    private void Update()
    {
        if (BattleReady_UIManager.instance.GetChange_FormationCamera())
        {
            HandleMovement();
        }
        else if(!ScenesSystem.Instance.GetIsInGame())
        {
            return;
        }
        else
        {
            HandleMovement();
            HandleRotation();
            HandleZoom();
            TopView();
        }


        if (UnitActionSystem.Instance.GetCameraPointchange())
        {
            if (cinemachineVirtualingCamera != null)
            {
                cinemachineVirtualingCamera.LookAt = AttackActionSystem.Instance.GetUnitAttackFind();
            }
        }
        else
        {
            cinemachineVirtualingCamera.LookAt = this.transform;
        }

        if (UnitActionSystem.Instance.GetCameraSelUnit())
        {
            Vector3 moveDirection = (mousepos - transform.position).normalized;
            if (Vector3.Distance(transform.position, mousepos) > 0.1f)
            {
                float moveSpeed = 10f;
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                UnitActionSystem.Instance.SetCameraSelUnit(false);
            }
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        mousepos = MouseWorld.GetPosition();
    }

    private void ScenesSystem_OnScenesChange(object sender, EventArgs e)
    {
        camerazoom = false;
        TopViewVirtualingCamera.SetActive(false);
        transform.position = new Vector3(4.9f, 0.5f, 2.3f);
    }

    // Ä«¸Þ¶ó ÀÌµ¿
    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

    }

    // Ä«¸Þ¶ó È¸Àü
    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    // ¸¶¿ì½º ÈÙ(ÁÜ)
    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

    private void TopView()
    {
        if (InputManager.Instance.GetOnCamera())
        {
            if (!camerazoom)
            {
                TopViewVirtualingCamera.SetActive(true);
                Debug.Log("ÄÑÁü");
                camerazoom = true;
            }
            else
            {
                TopViewVirtualingCamera.SetActive(false);
                Debug.Log("²¨Áü");
                camerazoom = false;
            }
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        ScenesSystem.Instance.OnScenesChange -= ScenesSystem_OnScenesChange;
    }
}
