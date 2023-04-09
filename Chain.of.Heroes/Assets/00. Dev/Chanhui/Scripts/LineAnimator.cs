using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineAnimator : MonoBehaviour
{
    public float speed = 1.0f;
    private LineRenderer lineRenderer;
    private float t = 0.0f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        t += Time.deltaTime * speed;
        if (t > 1.0f)
        {
            t = 1.0f - (t - 1.0f);
        }
        Vector3 startPos = lineRenderer.GetPosition(0);
        Vector3 endPos = lineRenderer.GetPosition(1);
        lineRenderer.SetPosition(1, Vector3.Lerp(startPos, endPos, t));
    }
}
