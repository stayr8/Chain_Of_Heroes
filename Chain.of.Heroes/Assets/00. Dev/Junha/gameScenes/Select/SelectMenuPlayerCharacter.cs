using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SelectMenuPlayerCharacter : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public static bool isMove = false;

    #region Property
    public GameObject[] targetObjects;
    private int currentTargetIndex = 0;
    private GameObject CurrentTarget
    {
        get
        {
            return targetObjects[currentTargetIndex];
        }
    }

    private Transform CurrentTargetTransform
    {
        get
        {
            return targetObjects[currentTargetIndex].transform;
        }
    }

    private Vector3 NewPosition
    {
        get
        {
            Vector3 newPosition = targetObjects[currentTargetIndex].transform.position;
            newPosition.y += 0.5f;

            return newPosition;
        }
    }
    private Vector2 Direction
    {
        get
        {
            return NewPosition - transform.position;
        }
    }
    private Vector3 DirectionNormalized
    {
        get
        {
            return Direction.normalized;
        }
    }
    #endregion

    private void Start()
    {
        transform.position = new Vector3(CurrentTargetTransform.position.x, CurrentTargetTransform.position.y, -2f);
    }

    private void Update()
    {
        if (!UIManager_WorldMap.isOnMenu)
        {
            Movement();

            Search();
        }
    }

    private void Movement()
    {
        if (CurrentTarget != null)
        {
            transform.position += DirectionNormalized * moveSpeed * Time.deltaTime;
        }
    }

    private void Search()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentTargetIndex = (currentTargetIndex - 1 + targetObjects.Length) % targetObjects.Length;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetObjects.Length;
        }

        if (CurrentTarget != null)
        {
            Vector3 Dir = Direction;
            Dir.y = 0.0f;

            if (Dir != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Dir);
            }
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Chapter"))
        {
            if(other.gameObject.name == ("Chapter.01"))
            {
                UIManager_WorldMap._tmp.text = "Chapter.01\n졸업작품마이너갤러리";
            }
            else if (other.gameObject.name == ("Chapter.02"))
            {
                UIManager_WorldMap._tmp.text = "Chapter.02\n차라리날죽여";
            }
            else if (other.gameObject.name == ("Chapter.03"))
            {
                UIManager_WorldMap._tmp.text = "Chapter.03\n잘작동하니?";
            }
            else if (other.gameObject.name == ("Chapter.04"))
            {
                UIManager_WorldMap._tmp.text = "Chapter.04\n브라보졸마갤";
            }
        }
    }
}