using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaitTransition2 : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(goToScene2());
    }

    private IEnumerator goToScene2()
    {
        yield return new WaitForSeconds(23f);
        LoadingSceneController.LoadScene("Main");
    }
}