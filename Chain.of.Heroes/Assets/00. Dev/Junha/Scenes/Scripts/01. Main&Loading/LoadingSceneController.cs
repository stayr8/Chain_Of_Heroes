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
            Debug.Log("로딩 진행도: " + (progress * 100) + "%");

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
                             "LOADING...." }; // 로딩 텍스트의 각 단계를 정의합니다.
        int currentPhraseIndex = 0;

        while (true)
        {
            Txt_Loading.text = loadingArray[currentPhraseIndex];
            currentPhraseIndex = (currentPhraseIndex + 1) % loadingArray.Length; // 다음 단계로 넘어갑니다.

            yield return new WaitForSeconds(0.05f); // 각 단계마다 0.1초의 대기 시간을 줍니다.
        }
    }
}