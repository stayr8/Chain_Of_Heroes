using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

using UnityEngine.UI;

public class CursorBase : MonoBehaviour
{
    protected virtual void Init(RectTransform _rt, float _x, float _y, ref GameObject _currentSelected, string Object_Name)
    {
        _rt.anchoredPosition = new Vector2(_x, _y);
        _currentSelected = GameObject.Find(Object_Name);
        _currentSelected.GetComponent<Selectable>().Select();
    }

    protected virtual void Movement(RectTransform _rt, ref GameObject _currentSelected, float _moveDistance, float _minY, float _maxY)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

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
            SoundManager.instance.Sound_SelectMenu();

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

    /// <summary>
    /// WorldMap_MenuSelectCursor 전용 함수
    /// </summary>
    protected virtual void Movement_forWorld(RectTransform _rt, ref GameObject _currentSelected, float _moveDistance, float _minX, float _maxX, float _minY, float _maxY)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition += new Vector2(10f, _moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
            Select(_currentSelected);

            if(_rt.anchoredPosition.x > _maxX)
            {
                _rt.anchoredPosition = new Vector2(_minX, _rt.anchoredPosition.y);
            }
            if (_rt.anchoredPosition.y > _maxY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _minY);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition += new Vector2(-10f, -_moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
            Select(_currentSelected);

            if (_rt.anchoredPosition.x < _minX)
            {
                _rt.anchoredPosition = new Vector2(_maxX, _rt.anchoredPosition.y);
            }
            if (_rt.anchoredPosition.y < _minY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _maxY);
            }
        }
    }
}