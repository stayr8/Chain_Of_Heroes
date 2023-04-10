using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideUI : MonoBehaviour
{

    [SerializeField] private Image Panel;

    private float time = 0f;

    private float F_time = 2f;


    private void Start()
    {
        if(Panel != null)
            Fade();
    }

    public void Fade()
    {
        StartCoroutine(Fadeinout());
    }

    IEnumerator Fadeinout()
    {
        yield return new WaitForSeconds(0.2f);

        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }

        time = 0f;

        Invoke("Hide", 0.5f);

        yield return null;
    }

    void Hide()
    {
        this.gameObject.SetActive(false);;
    }
}
