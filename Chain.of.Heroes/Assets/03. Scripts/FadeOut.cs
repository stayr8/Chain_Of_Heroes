using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{


    [SerializeField] private Image Panel;
    [SerializeField] private float F_time = 0.2f;
    private float time = 0f;

    IEnumerator Fadein()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;

        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Fadein());
    }
}
