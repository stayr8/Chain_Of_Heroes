using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class WorldMap_PlayerController : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;

    private GameObject currentSelected;
    private Selectable selectable;
    private Selectable selectableOnLeft;
    public static Selectable selectableOnRight;

    public static bool isCan = true;

    public static bool isMoving = false; // 움직임 상태를 나타내는 플래그
    private float moveDuration = 1.0f; // 움직임의 지속 시간, 클수록 느려짐

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        currentSelected = GameObject.Find("_" + 
            (StageManager.instance.m_chapterNum == 0 ? "1" : StageManager.instance.m_chapterNum.ToString()));

        selectable = currentSelected.GetComponent<Selectable>();
        selectableOnLeft = selectable.FindSelectableOnLeft();
        selectableOnRight = selectable.FindSelectableOnRight();

        transform.position = new Vector2(currentSelected.transform.position.x,
                                         currentSelected.transform.position.y + 0.4f);
    }

    private void Update()
    {
        if (!PlayerCamera.isFree && (!WorldMap_UIManager.instance.GetBool("isMenuState") && !WorldMap_UIManager.instance.GetBool("isOnTip")))
        {
            Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Chapter"))
        {
            selectable = currentSelected.GetComponent<Selectable>();
            if (selectable != null)
            {
                selectableOnLeft = selectable.FindSelectableOnLeft();
                selectableOnRight = selectable.FindSelectableOnRight();
            }
        }
    }

    private void Move()
    {
        if (isMoving)
        {
            return; // 이미 움직이는 중이면 무시
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _spriteRenderer.flipX = false;

            if (selectable != null)
            {
                if (selectableOnLeft != null)
                {
                    currentSelected = selectableOnLeft.gameObject;
                    currentSelected.GetComponent<Selectable>().Select();
                    StageManager.instance.num--;
                    MapManager.Instance.stageNum--;

                    StartCoroutine(MoveObject(currentSelected.transform.position));
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(isCan) // 다음 스테이지가 해금되어 움직일 수 있다면
            {
                _spriteRenderer.flipX = true;

                if (selectable != null)
                {
                    if (selectableOnRight != null)
                    {
                        currentSelected = selectableOnRight.gameObject;
                        currentSelected.GetComponent<Selectable>().Select();
                        StageManager.instance.num++;
                        MapManager.Instance.stageNum++;

                        StartCoroutine(MoveObject(currentSelected.transform.position));
                    }
                }
            }
            else
            {
                return;
            }
        }
    }
    #region 움직임 애니메이션
    private IEnumerator MoveObject(Vector3 targetPosition)
    {
        isMoving = true;
        _anim.SetBool("isMove", true);

        Vector2 startPosition = transform.position;
        float elapsedTime = 0f;

        Vector2 temp = new Vector3(targetPosition.x, targetPosition.y + 0.4f);
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration); // 보간 시간 계산

            // 보간된 위치 계산 및 적용
            transform.position = Vector3.Lerp(startPosition, temp, t);

            yield return null;
        }

        transform.position = temp; // 목표 위치에 정확히 배치

        isMoving = false;
        _anim.SetBool("isMove", false);
    }
    #endregion

    public static GameObject GetRightChapter()
    {
        if(selectableOnRight != null)
        {
            return selectableOnRight.gameObject;
        }
        return null;
    }
}
