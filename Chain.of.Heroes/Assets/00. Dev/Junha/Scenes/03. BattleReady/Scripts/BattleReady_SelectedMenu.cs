using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleReady_SelectedMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField, Header("���� ��������Ʈ")] private Sprite Selected;
    [SerializeField, Header("�̼��� ��������Ʈ")] private Sprite notSelected;

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
        else if (gameObject.name == "_ChangeFormation")
        {
            setPos(35f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Back")
        {
            setPos(25f, rt.anchoredPosition.y);
        }

        image.SetNativeSize();
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
        else if (gameObject.name == "_ChangeFormation")
        {
            setPos(-65f, rt.anchoredPosition.y);
        }
        else if (gameObject.name == "_Back")
        {
            setPos(-75f, rt.anchoredPosition.y);
        }

        image.SetNativeSize();
    }

    private void setPos(float posX, float posY)
    {
        rt.anchoredPosition = new Vector2(posX, posY);
    }
}