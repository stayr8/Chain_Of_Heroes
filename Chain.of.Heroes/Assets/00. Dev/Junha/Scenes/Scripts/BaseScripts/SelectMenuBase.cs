using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SelectMenuBase : MonoBehaviour
{
    private RectTransform rt;
    private Image image;

    private Sprite Selected;
    private Sprite DeSelected;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        Selected = Resources.Load<Sprite>("J_SelectMenu");
        DeSelected = Resources.Load<Sprite>("J_DeSelectMenu");
    }

    /// <summary>
    /// use. InGame_SelectedMenu
    /// </summary>
    protected virtual void Select(float m_x)
    {
        image.sprite = Selected;

        rt.anchoredPosition = new Vector2(m_x, rt.anchoredPosition.y);

        image.SetNativeSize();
    }
    protected virtual void DeSelect(float m_x)
    {
        image.sprite = DeSelected;

        rt.anchoredPosition = new Vector2(m_x, rt.anchoredPosition.y);

        image.SetNativeSize();
    }

    /// <summary>
    /// use. WorldMap_SelectedMenu
    /// use. BattleReady_SelectedMEnu
    /// </summary>
    protected virtual void Select(bool isSelect, GameObject _obj, 
                                string m1, float m1_x, 
                                string m2, float m2_x, 
                                string m3, float m3_x, 
                                string m4, float m4_x)
    {
        image.sprite = isSelect ? Selected : DeSelected;

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

        image.SetNativeSize();
    }

    /// <summary>
    /// use. BattleReady_SelectedMenu
    /// </summary>
    protected virtual void Select(bool isSelect, GameObject _obj,
                                string SelectSprite, string DeSelectSprite,
                                string m1, float m1_x,
                                string m2, float m2_x,
                                string m3, float m3_x,
                                string m4, float m4_x)
    {
        image.sprite = isSelect ? Resources.Load<Sprite>(SelectSprite) : Resources.Load<Sprite>(DeSelectSprite);

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

        image.SetNativeSize();
    }
}