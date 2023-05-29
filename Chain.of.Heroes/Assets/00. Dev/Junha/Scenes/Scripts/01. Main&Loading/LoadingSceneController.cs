using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    private static string nextScene;

    private void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private IEnumerator LoadSceneProcess()
    {
        yield return new WaitForSeconds(1); // ���Ƿ� 1�� ���

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            yield return null;

            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("�ε� ���൵: " + (progress * 100) + "%");

            if(progress >= 1f)
            {
                asyncLoad.allowSceneActivation = true;
                yield break;
            }
        }
    }
}