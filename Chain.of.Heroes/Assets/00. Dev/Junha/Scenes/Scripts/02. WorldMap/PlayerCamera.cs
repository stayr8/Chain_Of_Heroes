using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    private Transform playerTr;
    private float targetZoom;

    private RectTransform chapterInfoRT;
    private const float maxPosY = -315f; private const float minPosY = -1080f;

    private Image Img_Tip;
    private Sprite focusMode;
    private Sprite freeMode;

    [Header("ī�޶� ���� ������/���� �߽�")]
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private Vector2 center;

    [Header("ī�޶� �̵� �ӵ�/�߽� ��ġ")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3 cameraPosition;

    public static bool isFree = false;
    [Header("���� ī�޶� �� �̵� �ӵ�/�� �ӵ�")]
    [SerializeField] private float freeMoveSpeed;
    [SerializeField] private float freeZoomSpeed;

    [Header("�ּ�/�ִ� �� ����")]
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;

    [Header("���� ����")]
    [SerializeField] private float zoomSmoothness;

    private void Awake()
    {
        chapterInfoRT = GameObject.Find("[Image] Info Background").GetComponent<RectTransform>();

        Img_Tip = GameObject.Find("[Image] CameraTip").GetComponent<Image>();
        focusMode = Resources.Load<Sprite>("Camera_Focus");
        freeMode = Resources.Load<Sprite>("Camera_Free");

        playerTr = GameObject.Find("_player").GetComponent<Transform>();
        targetZoom = Camera.main.orthographicSize;
    }

    private void Update()
    {
        CameraArea();

        if (!WorldMap_UIManager.instance.GetBool("isMenuState") && !WorldMap_UIManager.instance.GetBool("isOnTip"))
        {
            CameraMode();
        }
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

    private void CameraMode()
    {
        if (!isFree)
        {
            FocusCameraMode();
        }
        else // isFree
        {
            FreeCameraMode();
        }

        if (!WorldMap_PlayerController.isMoving && Input.GetKeyDown(KeyCode.R))
        {
            SoundManager.instance.Sound_SelectMenu();

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

    private void FocusCameraMode()
    {
        chapterInfoRT.anchoredPosition = new Vector2(chapterInfoRT.anchoredPosition.x, maxPosY);

        Img_Tip.sprite = focusMode;
        Img_Tip.SetNativeSize();

        targetZoom = 5f;

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 5f, zoomSmoothness * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position,
                                          playerTr.position + cameraPosition,
                                          Time.deltaTime * moveSpeed);
    }
    private void FreeCameraMode()
    {
        chapterInfoRT.anchoredPosition = new Vector2(chapterInfoRT.anchoredPosition.x, minPosY);

        Img_Tip.sprite = freeMode;
        Img_Tip.SetNativeSize();

        // ���� ī�޶� �̵� //
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(horizontalInput, verticalInput);
        Vector2 velocity = direction * freeMoveSpeed * Time.deltaTime;

        transform.Translate(velocity);

        // ���� ī�޶� �� //
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float zoomAmount = scrollInput * freeZoomSpeed * Time.deltaTime;

        targetZoom -= zoomAmount;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomSmoothness * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }
}