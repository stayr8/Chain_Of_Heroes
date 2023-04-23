using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleReady_SelectedMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField, Header("선택 스프라이트")] private Sprite Selected;
    [SerializeField, Header("미선택 스프라이트")] private Sprite notSelected;

    private RectTransform rt;
    private Image image;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (gameObject.name == "_BattleStart")
        {
            Select();
        }
    }
    private void OnDisable()
    {
        notSelect();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Select();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        notSelect();
    }

    private void Select()
    {
        image.sprite = Selected;

        if (gameObject.name == "_BattleStart")
        {
            setPos(55f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_UnitFormation")
        {
            setPos(45f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Maintenance")
        {
            setPos(35f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_ChangeFormation")
        {
            setPos(25f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Save")
        {
            setPos(15f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Back")
        {
            setPos(5f, rt.anchoredPosition.y);
        }
        rt.sizeDelta = new Vector2(557f, rt.sizeDelta.y);
    }
    private void notSelect()
    {
        image.sprite = notSelected;

        if (gameObject.name == "_BattleStart")
        {
            setPos(-45f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_UnitFormation")
        {
            setPos(-55f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Maintenance")
        {
            setPos(-65f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_ChangeFormation")
        {
            setPos(-75f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Save")
        {
            setPos(-85f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Back")
        {
            setPos(-95f, rt.anchoredPosition.y);
        }
        rt.sizeDelta = new Vector2(335f, rt.sizeDelta.y);
    }

    private void setPos(float posX, float posY)
    {
        rt.anchoredPosition = new Vector2(posX, posY);
    }
}