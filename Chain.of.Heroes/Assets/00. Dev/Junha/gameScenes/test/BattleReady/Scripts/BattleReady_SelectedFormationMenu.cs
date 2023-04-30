using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleReady_SelectedFormationMenu : MonoBehaviour, ISelectHandler, IDeselectHandler
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
        if (gameObject.name == "_Formation")
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

        setSize(452f, 79f);
    }
    private void notSelect()
    {
        image.sprite = notSelected;

        setSize(436f, 60f);
    }

    private void setSize(float sizeX, float sizeY)
    {
        rt.sizeDelta = new Vector2(sizeX, sizeY);
    }
}
