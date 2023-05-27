using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeOut03 : MonoBehaviour
{
    [SerializeField] private float F_time = 0.2f;
    [SerializeField] private TMP_Text TalkText;
    private float time = 0f;

    IEnumerator FadeOut()
    {
        TalkText.gameObject.SetActive(true);
        time = 0f;
        Color alpha = TalkText.color;

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            TalkText.color = alpha;
            yield return null;
        }

        yield return null;
    }
    public void Fade()
    {
        StartCoroutine(FadeOut());
    }
}
