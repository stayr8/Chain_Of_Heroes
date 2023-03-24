using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testController : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    CharacterController _controller;

    public float speed = 5f;
    public float runSpeed = 8f;
    public float finalSpeed; 
    public bool toggleCameraRotation;
    public bool run;

    public float smoothness = 10f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; // �ѷ����� Ȱ��ȭ
        }
        else
        {
            toggleCameraRotation = false; // �ѷ����� ��Ȱ��ȭ
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }

        InputMovement();
    }

    private void LateUpdate()
    {
        if (toggleCameraRotation != true)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    private void InputMovement()
    {
        finalSpeed = (run) ? runSpeed : speed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);

        float percent = ((run) ? 1 : 0.5f) * moveDirection.magnitude;
        Debug.Log(percent);
        _animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}