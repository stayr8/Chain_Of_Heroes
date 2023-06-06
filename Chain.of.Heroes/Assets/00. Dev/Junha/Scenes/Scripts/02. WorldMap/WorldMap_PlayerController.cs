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

    public static bool isMoving = false; // ������ ���¸� ��Ÿ���� �÷���
    private float moveDuration = 1.0f; // �������� ���� �ð�, Ŭ���� ������

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
            return; // �̹� �����̴� ���̸� ����
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
            if(isCan) // ���� ���������� �رݵǾ� ������ �� �ִٸ�
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
    #region ������ �ִϸ��̼�
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

    public static GameObject GetRightChapter()
    {
        if(selectableOnRight != null)
        {
            return selectableOnRight.gameObject;
        }
        return null;
    }
}
