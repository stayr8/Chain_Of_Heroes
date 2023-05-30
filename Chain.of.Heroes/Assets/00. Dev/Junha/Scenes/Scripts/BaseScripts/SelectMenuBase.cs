using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SelectMenuBase : MonoBehaviour
{
    private RectTransform rt;
    private Image image;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    protected virtual void Select(float m_x)
    {
        rt.anchoredPosition = new Vector2(m_x, rt.anchoredPosition.y);

        SetSize();
    }
    protected virtual void DeSelect(float m_x)
    {
        rt.anchoredPosition = new Vector2(m_x, rt.anchoredPosition.y);

        SetSize();
    }

    protected virtual void Select(GameObject _obj,
                                string m1, float m1_x,
                                string m2, float m2_x,
                                string m3, float m3_x,
                                string m4, float m4_x)
    {
        if (_obj.name == m1)
        {
            rt.anchoredPosition = new Vector2(m1_x, rt.anchoredPosition.y);
        }
        else if (_obj.name == m2)
        {
            rt.anchoredPosition = new Vector2(m2_x, rt.anchoredPosition.y);
        }
        else if (_obj.name == m3)
        {
            rt.anchoredPosition = new Vector2(m3_x, rt.anchoredPosition.y);
        }
        else if (_obj.name == m4)
        {
            rt.anchoredPosition = new Vector2(m4_x, rt.anchoredPosition.y);
        }

        SetSize();
    }

    protected virtual void SetSize()
    {
        image.SetNativeSize();
    }
}