using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class WorldMap_PlayerController : MonoBehaviour
{
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;

    private GameObject currentSelected;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        currentSelected = GameObject.Find("_1");
        transform.position = new Vector2(currentSelected.transform.position.x,
                                        currentSelected.transform.position.y + 0.4f);
    }

    private void Update()
    {
        Move();
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

            var selectable = currentSelected.GetComponent<Selectable>();
            if (selectable != null)
            {
                var selectableOnLeft = selectable.FindSelectableOnLeft();
                if (selectableOnLeft != null)
                {
                    currentSelected = selectableOnLeft.gameObject;
                    currentSelected.GetComponent<Selectable>().Select();

                    StartCoroutine(MoveObject(currentSelected.transform.position));
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _spriteRenderer.flipX = true;

            var selectable = currentSelected.GetComponent<Selectable>();
            if (selectable != null)
            {
                var selectableOnRight = selectable.FindSelectableOnRight();
                if (selectableOnRight != null)
                {
                    currentSelected = selectableOnRight.gameObject;
                    currentSelected.GetComponent<Selectable>().Select();

                    StartCoroutine(MoveObject(currentSelected.transform.position));
                }
            }
        }
    }

    #region 움직임 애니메이션
    private bool isMoving = false; // 움직임 상태를 나타내는 플래그
    private float moveDuration = 1.0f; // 움직임의 지속 시간, 클수록 느려짐
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
