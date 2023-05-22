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
            return; // �̹� �����̴� ���̸� ����
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

    #region ������ �ִϸ��̼�
    private bool isMoving = false; // ������ ���¸� ��Ÿ���� �÷���
    private float moveDuration = 1.0f; // �������� ���� �ð�, Ŭ���� ������
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
            float t = Mathf.Clamp01(elapsedTime / moveDuration); // ���� �ð� ���

            // ������ ��ġ ��� �� ����
            transform.position = Vector3.Lerp(startPosition, temp, t);

            yield return null;
        }

        transform.position = temp; // ��ǥ ��ġ�� ��Ȯ�� ��ġ

        isMoving = false;
        _anim.SetBool("isMove", false);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
