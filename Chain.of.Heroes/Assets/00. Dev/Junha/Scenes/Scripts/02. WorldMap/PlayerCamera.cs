using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private float targetZoom;
    [SerializeField, Header("ī�޶� ���� ����/���� �߽�")] private Vector2 mapSize;
    [SerializeField] private Vector2 center;

    [SerializeField, Header("[�÷��̾� ������Ʈ] Ʈ������")] private Transform tr;
    [SerializeField, Header("ī�޶� �̵� �ӵ�")] private float cameraMoveSpeed;
    [SerializeField, Header("ī�޶� ��ġ")] private Vector3 cameraPosition;

    public static bool isFree = false;
    [SerializeField, Header("���� ī�޶� �� �̵� �ӵ�")] private float moveSpeed;
    [SerializeField, Header("���� ī�޶� �� �� �ӵ�")] private float zoomSpeed;
    [SerializeField, Header("�ּ�/�ִ� ��")] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField, Header("���� ����")] private float zoomSmoothness;

    //private void Awake() { }

    private void Start()
    {
        targetZoom = Camera.main.orthographicSize;
    }

    private void Update()
    {
        CameraArea();

        FreeCamera();
    }

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

    private void playerCamera()
    {
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5f, zoomSmoothness * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position,
                                          tr.position + cameraPosition,
                                          Time.deltaTime * cameraMoveSpeed);
    }

    #region ���� ī�޶� ����
    private void CameraMove()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        Vector2 velocity = direction * moveSpeed * Time.deltaTime;

        transform.Translate(velocity);
    }
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
