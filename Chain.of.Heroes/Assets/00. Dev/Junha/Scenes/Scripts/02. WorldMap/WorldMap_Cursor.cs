using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class WorldMap_Cursor : CursorBase
{
    private RectTransform rt;
    private GameObject nextButton;
    private GameObject currentSelected;

    private const float INIT_X = -860f;
    private const float INIT_Y = 200f;

    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_X = -860f; private const float MAX_POSITION_Y = 200f;
    private const float MIN_POSITION_X = -890f; private const float MIN_POSITION_Y = -100f;

    public static bool isOnNextButton = false;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        nextButton = GameObject.Find("[ Next Button ]").transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_ChapterStart");
    }

    private void Update()
    {
        MenuFunction();

        if (!isOnNextButton)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }
        else // isOnNextButton
        {
            Movement(ref currentSelected);
        }
    }

    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0f, 0f);
    }

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.instance.Sound_SelectMenu();

            switch (currentSelected.name)
            {
                case "_ChapterStart":
                    isOnNextButton = true;
                    OnNextButton(nextButton, isOnNextButton, gameObject, ref currentSelected, "_Yes");
                    break;

                case "_Party":
                    WorldMap_UIManager.instance.OnParty();
                    break;

                case "_Save":
                    WorldMap_UIManager.instance.LieSave();
                    break;

                case "_Title":
                    LoadingSceneController.LoadScene("Main");
                    break;

                case "_Yes":
                    isOnNextButton = false;
                    LoadingSceneController.LoadScene("Ch_01");
                    break;

                case "_No":
                    isOnNextButton = false;
                    OnNextButton(nextButton, isOnNextButton, gameObject, ref currentSelected, "_ChapterStart");
                    break;
            }
        }
    }
}