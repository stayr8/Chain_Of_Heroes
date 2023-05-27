using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class WorldMap_Cursor : CursorBase
{
    private RectTransform rt;

    private const float INIT_X = -860f;
    private const float INIT_Y = 200f;
    private GameObject currentSelected;

    public static bool isInitStart = false;

    public static bool isChapterStart = false;
    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_X = -860f; private const float MAX_POSITION_Y = 200f;
    private const float MIN_POSITION_X = -880f; private const float MIN_POSITION_Y = 0f;

    [SerializeField] GameObject nextButton;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    // private void Start() { }

    private void OnEnable()
    {
        if(!isInitStart)
        {
            Init(rt, INIT_X, INIT_Y, ref currentSelected, "_ChapterStart");
            isInitStart = true;
        }
        else // isInitStart
        {

        }
    }

    private void Update()
    {
        if (!isChapterStart)
        {
            Movement(rt, ref currentSelected, MOVE_DISTANCE, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);
        }

        MenuFunction();
    }

    private void LateUpdate()
    {
        rt.Rotate(150f * Time.deltaTime, 0f, 0f);
    }

    public static bool isOnNextButton = false;
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
                    Debug.Log("¿˙¿Â");

                    break;

                case "_Yes":
                    SceneManager.LoadScene("Ch_01");

                    break;

                case "_No":
                    isOnNextButton = false;
                    OnNextButton(nextButton, isOnNextButton, gameObject, ref currentSelected, "_ChapterStart");

                    ResetCursorPosition();

                    break;
            }
        }
    }

    private void ResetCursorPosition()
    {
        if (currentSelected.name == "_ChapterStart")
        {
            rt.anchoredPosition = new Vector2(-860f, MAX_POSITION_Y);
        }
    }
}