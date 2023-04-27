using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dissolve : MonoBehaviour
{
    private RawImage _ri;

    private float ChangeSpeed = 6f;


    private void Start()
    {
        _ri = GetComponent<RawImage>();
        _ri.texture = ScreenManager._instance.ScreenTextuer;

        if(_ri.texture == null)
        {
            gameObject.SetActive(false);
        }

        _ri.color= Color.white;
    }

    private void Update()
    {
        _ri.color = Color.Lerp(_ri.color, new Color(1,1,1,0), ChangeSpeed * Time.deltaTime);
        if(_ri.color.a <= 0.01f)
        {
            gameObject.SetActive(false);
        }
    }
}
