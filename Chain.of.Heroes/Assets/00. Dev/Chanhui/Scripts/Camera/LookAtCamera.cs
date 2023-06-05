using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool invert;

    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if(invert)
        {
            transform.LookAt(transform.position + cameraTransform.transform.rotation * Vector3.back,
                cameraTransform.transform.rotation * Vector3.up);
        }
        else
        {
            //transform.LookAt(cameraTransform);
            transform.LookAt(transform.position + cameraTransform.transform.rotation * Vector3.back,
                cameraTransform.transform.rotation * Vector3.up);
        }
    }
}
