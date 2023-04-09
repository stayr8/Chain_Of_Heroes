using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 25f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualingCamera;
    [SerializeField] private GameObject TopViewVirtualingCamera;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    public bool camerazoom = false;


    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
        TopView();


        if (UnitActionSystem.Instance.GetCameraSelUnit())
        {
            cinemachineVirtualingCamera.LookAt = this.transform;
        }
        else
        {
            cinemachineVirtualingCamera.LookAt = UnitActionSystem.Instance.GetSelecterdUnitEnemy().transform;
        }
    }

    // 카메라 이동
    private void HandleMovement()
    {

        Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();

        float moveSpeed = 10f;

        Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;

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
                Invoke("Trem", 2);
            }
            else
            {
                TopViewVirtualingCamera.SetActive(false);
                Invoke("Trem", 2);
            }
        }
    }

    void Trem()
    {
        if(!camerazoom)
        {
            camerazoom = true;
        }
        else
        {
            camerazoom = false;
        }
    }

}
