using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    private bool isLock = true;

    private void Update()
    {
        freeCamera();
    }

    private void freeCamera()
    {
        if(isLock)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("카메라 락 풀림");
                isLock = false;
            }

        }
        else if(!isLock)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Debug.Log("카메라 락 잠김");
                isLock = false;
            }
        }
    }
}