using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private float targetZoom;

    private Transform playerTransform;
    //private float width;
    //private float height;
    private void Awake()
    {
        targetZoom = Camera.main.orthographicSize;

        playerTransform = GameObject.Find("_player").GetComponent<Transform>();

        //width = height * Screen.width / Screen.height;
        //height = Camera.main.orthographicSize;
    }

    private void Update()
    {
        CameraArea();

        FreeCamera();
    }

    [SerializeField, Header("카메라 영역 제한 / 영역 중심")] private Vector2 mapSize;
    [SerializeField] private Vector2 center;
    private void CameraArea()
    {
        float width = Camera.main.orthographicSize * Camera.main.aspect;
        float height = Camera.main.orthographicSize;

        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private bool isFree = false;
    private void FreeCamera()
    {
        if (!isFree)
        {
            targetZoom = 5f;
            playerCamera();
        }
        else // isFree
        {
            CameraMove();
            CameraZoom();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isFree)
            {
                isFree = true;
            }
            else // isFree
            {
                isFree = false;
            }
        }
    }

    [SerializeField, Header("카메라 이동 속도")] private float cameraMoveSpeed;
    [SerializeField, Header("카메라 위치")] private Vector3 cameraPosition;
    private void playerCamera()
    {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5f, zoomSmoothness * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position,
                                          playerTransform.position + cameraPosition,
                                          Time.deltaTime * cameraMoveSpeed);
    }

    #region 프리 카메라 로직
    [SerializeField, Header("프리 카메라 시 이동 속도")] private float moveSpeed;
    private void CameraMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        Vector2 velocity = direction * moveSpeed * Time.deltaTime;

        transform.Translate(velocity);
    }
    [SerializeField, Header("프리 카메라 시 줌 속도")] private float zoomSpeed;
    [SerializeField, Header("최소/최대 줌")] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField, Header("보간 정도")] private float zoomSmoothness;
    private void CameraZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scrollInput * zoomSpeed * Time.deltaTime;

        targetZoom -= zoomAmount;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomSmoothness * Time.deltaTime);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}
