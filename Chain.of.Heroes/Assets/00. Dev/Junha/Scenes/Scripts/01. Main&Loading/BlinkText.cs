using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class BlinkText : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();

        StartCoroutine(FadeTextToZeroAlpha());
    }

    private float minAlpha = 0.25f;
    private float maxAlpha = 1.0f;
    private IEnumerator FadeTextToFullAlpha() // 알파값 minAlpha에서 maxAlpha로 전환
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, minAlpha);
        while (tmp.color.a < maxAlpha)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToZeroAlpha());
    }

    public IEnumerator FadeTextToZeroAlpha()  // 알파값 maxAlpha에서 minAlpha로 전환
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, maxAlpha);
        while (tmp.color.a > minAlpha)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
        StartCoroutine(FadeTextToFullAlpha());
    }

}
