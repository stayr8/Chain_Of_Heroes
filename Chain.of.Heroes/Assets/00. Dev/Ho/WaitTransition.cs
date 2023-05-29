using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class WaitTransition : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(goToScene());
    }

    private IEnumerator goToScene()
    {
        yield return new WaitForSeconds(31f);
        LoadingSceneController.LoadScene("WorldMapScene"); // 튜토리얼 씬으로 연결해야 함
    }
}