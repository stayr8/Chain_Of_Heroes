using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class SelectMenuBase : MonoBehaviour
{
    public Sprite Selected;
    public Sprite DeSelected;

    public RectTransform rt;
    public Image image;
    private void Awake()
    {
        Selected = Resources.Load<Sprite>("J_SelectMenu");
        DeSelected = Resources.Load<Sprite>("J_notSelectMenu");

        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
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
        if(isSelect)
        {
            image.sprite = Selected;
        }
        else // !isSelect
        {
            image.sprite = DeSelected;
        }

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