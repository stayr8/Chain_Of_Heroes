using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 11f;
    private const float MAX_FOLLOW_Y_OFFSET = 20f;

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
        AttackActionSystem.OnActionStarted += AttackActionSystem_OnActionStarted;
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
        mousepos = UnitActionSystem.Instance.GetSelecterdUnit().transform.position;
        mousepos.y = transform.position.y;
    }

    private void ScenesSystem_OnScenesChange(object sender, EventArgs e)
    {
        camerazoom = false;
        TopViewVirtualingCamera.SetActive(false);
        transform.position = new Vector3(4.9f, 0.5f, 2.3f);
    }

    private void AttackActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        TopViewVirtualingCamera.SetActive(false);
    }

    // 카메라 이동
    private void HandleMovement()
    {
        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        float moveSpeed = 10f;

        // X-axis 범위 제한
        float clampedX = Mathf.Clamp(transform.position.x + inputMoveDir.x * moveSpeed * Time.deltaTime, 0f, 60f);
        float deltaX = clampedX - transform.position.x;

        // Y-axis 범위 제한
        float clampedY = Mathf.Clamp(transform.position.z + inputMoveDir.y * moveSpeed * Time.deltaTime, 0f, 60f);
        float deltaY = clampedY - transform.position.z;

        Vector3 moveVector = new Vector3(deltaX, 0f, deltaY);

        transform.position += moveVector;
    }

    // 카메라 회전
    private void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);

        rotationVector.y = InputManager.Instance.GetCameraRotateAmount();

        float rotationSpeed = 100f;
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    // 마우스 휠(줌)
    private void HandleZoom()
    {
        float speed = 1.5f;
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * speed;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        float zoomSpeed = 5f;
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);


        float inputMoveDir = InputManager.Instance.GetCameraZoomAmount();

        float moveSpeed = 10f;

        // Y-axis 범위 제한
        float clampedY = Mathf.Clamp(transform.position.y + inputMoveDir * moveSpeed * Time.deltaTime, 1f, 3f);
        float deltaY = clampedY - transform.position.y;

        Vector3 moveVector = new Vector3(0f, deltaY, 0f);

        transform.position += moveVector;
    }

    private void TopView()
    {
        if (InputManager.Instance.GetOnCamera())
        {
            if (!camerazoom)
            {
                TopViewVirtualingCamera.SetActive(true);
                Debug.Log("켜짐");
                StartCoroutine(TimeStart());
            }
            else
            {
                TopViewVirtualingCamera.SetActive(false);
                Debug.Log("꺼짐");
                StartCoroutine(TimeStart());
            }
        }
    }

    IEnumerator TimeStart()
    {
        yield return new WaitForSeconds(2f);

        if (!camerazoom)
            camerazoom = true;
        else
            camerazoom = false;
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        ScenesSystem.Instance.OnScenesChange -= ScenesSystem_OnScenesChange;
    }
}
