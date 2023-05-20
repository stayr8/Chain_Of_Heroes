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
        yield return new WaitForSeconds(32.5f);
        //SceneManager.LoadScene("TalkTest");
        LoadingSceneController.LoadScene("TalkTest");
    }
}