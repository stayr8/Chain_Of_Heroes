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
        _currentSelected.GetComponent<Selectable>().Select();
    }

    /// <summary>
    /// Main_MenuSelectCursor 사용
    /// Main_SlotSelectCursor 사용
    /// </summary>
    protected virtual void Movement(RectTransform _rt, ref GameObject _currentSelected, float _moveDistance, float _minY, float _maxY)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition += new Vector2(0, _moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

            if (_rt.anchoredPosition.y > _maxY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _minY);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition -= new Vector2(0, _moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

            if (_rt.anchoredPosition.y < _minY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _maxY);
            }
        }
    }

    /// <summary>
    /// WorldMap_MenuSelectCursor 사용
    /// BattleReady_MenuSelectCursor 사용
    /// </summary>
    protected virtual void Movement(RectTransform _rt, ref GameObject _currentSelected, float _moveDistance, float _minX, float _maxX, float _minY, float _maxY)
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition += new Vector2(10f, _moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

            if (_rt.anchoredPosition.x > _maxX)
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

            _rt.anchoredPosition -= new Vector2(10f, _moveDistance);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

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

    /// <summary>
    /// BattleReady_UnitFormationCursor 사용
    /// </summary>
    protected virtual void Movement(RectTransform _rt, ref GameObject _currentSelected, float _moveDistance_X, float _moveDistance_Y, float _minX, float _maxX, float _minY, float _maxY)
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition -= new Vector2(_moveDistance_X, 0f);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnLeft().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

            if (_rt.anchoredPosition.x < _minX)
            {
                _rt.anchoredPosition = new Vector2(_maxX, _rt.anchoredPosition.y);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition += new Vector2(_moveDistance_X, 0f);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnRight().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

            if (_rt.anchoredPosition.x > _maxX)
            {
                _rt.anchoredPosition = new Vector2(_minX, _rt.anchoredPosition.y);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition += new Vector2(0f, _moveDistance_Y);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnUp().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

            if (_rt.anchoredPosition.y > _maxY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _minY);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SoundManager.instance.Sound_SelectMenu();

            _rt.anchoredPosition -= new Vector2(0f, _moveDistance_Y);

            _currentSelected = _currentSelected.GetComponent<Selectable>().FindSelectableOnDown().gameObject;
            _currentSelected.GetComponent<Selectable>().Select();

            if (_rt.anchoredPosition.y < _minY)
            {
                _rt.anchoredPosition = new Vector2(_rt.anchoredPosition.x, _maxY);
            }
        }
    }
}