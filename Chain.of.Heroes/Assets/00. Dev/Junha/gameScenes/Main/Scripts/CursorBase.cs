using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorBase : MonoBehaviour
{
    protected virtual void Init(RectTransform _rt, float _x, float _y, ref GameObject _currentSelected, string Object_Name)
    {
        _rt.anchoredPosition = new Vector2(_x, _y);
        _currentSelected = GameObject.Find(Object_Name);
    }

    protected virtual void Movement(RectTransform _rt, ref GameObject _currentSelected, float _moveDistance, float _minY, float _maxY)
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            _rt.anchoredPosition += new Vector2(0, _moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
            Select(_currentSelected);

            if (_rt.anchoredPosition.y > _maxY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _minY);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _rt.anchoredPosition += new Vector2(0, -_moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
            Select(_currentSelected);

            if (_rt.anchoredPosition.y < _minY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _maxY);
            }
        }
    }
    private void Select(GameObject _currentSelected)
    {
        _currentSelected.GetComponent<Selectable>().Select();
    }
}