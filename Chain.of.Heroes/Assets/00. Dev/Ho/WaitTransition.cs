using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTransition : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(goToScene());
    }

    private IEnumerator goToScene()
    {
        //yield return new WaitForSeconds(31f);
        yield return new WaitForSeconds(1f);
        LoadingSceneController.LoadScene("Ch_Tutorial"); // 튜토리얼 씬으로 연결해야 함
    }
}