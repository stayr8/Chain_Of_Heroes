using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReady_ChangeFormationCursor : CursorBase
{
    private RectTransform rt;
    private GameObject BeforeCurrentSelected;
    [SerializeField] private GameObject AfterCurrentSelect;

    private bool _seleted;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        _seleted = false;
    }

    private const float INIT_X = 50f;
    private const float INIT_Y = -50f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref BeforeCurrentSelected, "_04");
    }

    private const float MOVE_DISTANCE_X = 125f; private const float MOVE_DISTANCE_Y = 125f;
    private const float MAX_POSITION_X = 425f; private const float MAX_POSITION_Y = -50f;
    private const float MIN_POSITION_X = 50f; private const float MIN_POSITION_Y = -300f;
    private void Update()
    {
        Movement(rt, ref BeforeCurrentSelected, MOVE_DISTANCE_X, MOVE_DISTANCE_Y, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);

        MenuFunction();
    }

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(_seleted)
            {
                _seleted = false;
                cursorSeleted();
            }
            else
            {
                _seleted = true;
                cursorSeleted();
            }

            switch (BeforeCurrentSelected.name)
            {
                case "_04":
                    Debug.Log("x: 0, y: 4 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 8);

                    break;
                case "_24":
                    Debug.Log("x: 2, y: 4 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 9);
                    break;
                case "_44":
                    Debug.Log("x: 4, y: 4 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 10);
                    break;
                case "_64":
                    Debug.Log("x: 6, y: 4 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 11);
                    break;

                case "_02":
                    Debug.Log("x: 0, y: 2 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 4);
                    break;
                case "_22":
                    Debug.Log("x: 2, y: 2 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 5);
                    break;
                case "_42":
                    Debug.Log("x: 4, y: 2 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 6);
                    break;
                case "_62":
                    Debug.Log("x: 6, y: 2 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 7);
                    break;

                case "_00":
                    Debug.Log("x: 0, y: 0 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 0);
                    break;
                case "_20":
                    Debug.Log("x: 2, y: 0 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 1);
                    break;
                case "_40":
                    Debug.Log("x: 4, y: 0 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 2);
                    break;
                case "_60":
                    Debug.Log("x: 6, y: 0 ΩΩ∑‘ º±≈√");
                    ChangePosAtSeleted(1, 3);
                    break;
            }
        }
    }

    private void ChangePosAtSeleted(int character, int pos)
    {
        if(_seleted)
        {
            
        }
        else
        {
            Debug.Log("ø©±‚¥¬ µÈæÓø¿¡ˆ?");
            ChangeFormationSystem.Instance.CreateCharacterUI(character, pos);
        }
    }

    private void cursorSeleted()
    {
        if (_seleted)
        {
            AfterCurrentSelect.SetActive(_seleted);
        }
        else
        {
            AfterCurrentSelect.SetActive(_seleted);
        }
    }
}