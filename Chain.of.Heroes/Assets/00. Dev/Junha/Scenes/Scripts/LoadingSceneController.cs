using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneController : MonoBehaviour
{
    private static string nextScene;
    private TMP_Text Txt_Loading;

    private void Start()
    {
        SoundManager.instance.Sound_ForceStop();

        Txt_Loading = GameObject.Find("[Txt] Loading").GetComponent<TMP_Text>();
        
        StartCoroutine(LoadSceneProcess());
        StartCoroutine(Loading());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private IEnumerator LoadSceneProcess()
    {
        yield return new WaitForSeconds(1f);

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

    private IEnumerator Loading()
    {
        string[] loadingArray = { "L",
                             "LO",
                             "LOA",
                             "LOAD",
                             "LOADI",
                             "LOADIN",
                             "LOADING",
                             "LOADING.",
                             "LOADING..",
                             "LOADING...",
                             "LOADING...." }; // �ε� �ؽ�Ʈ�� �� �ܰ踦 �����մϴ�.
        int currentPhraseIndex = 0;

        while (true)
        {
            Txt_Loading.text = loadingArray[currentPhraseIndex];
            currentPhraseIndex = (currentPhraseIndex + 1) % loadingArray.Length; // ���� �ܰ�� �Ѿ�ϴ�.

            yield return new WaitForSeconds(0.05f); // �� �ܰ踶�� 0.1���� ��� �ð��� �ݴϴ�.
        }
    }
}