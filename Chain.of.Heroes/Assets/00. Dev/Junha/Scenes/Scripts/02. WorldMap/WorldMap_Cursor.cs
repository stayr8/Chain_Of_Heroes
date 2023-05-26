using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WorldMap_Cursor : CursorBase
{
    private RectTransform rt;
    private GameObject currentSelected;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private const float INIT_X = -860f;
    private const float INIT_Y = 200f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref currentSelected, "_ChapterStart");
    }

    public static bool isChapterStart = false;
    private const float MOVE_DISTANCE = 100f;
    private const float MAX_POSITION_X = -860f; private const float MAX_POSITION_Y = 200f;
    private const float MIN_POSITION_X = -880f; private const float MIN_POSITION_Y = 0f;
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

    [SerializeField, Header("nextButton")] private GameObject nextButton;
    [SerializeField, Header("nextButton Text")] private TextMeshProUGUI text_nextButton;
    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SoundManager.instance.Sound_SelectMenu();
            switch (currentSelected.name)
            {
                case "_ChapterStart":
                    isChapterStart = true;
                    NextButton(true, "_Yes");
                    text_nextButton.text = "챕터를 진행하시겠습니까?";

                    break;

                case "_Party":
                    

                    break;

                case "_Save":
                    Debug.Log("저장");
                    break;

                case "_Yes":
                    SceneManager.LoadScene("Ch_01"); // map_id에 맞게 바꿔줘야 할 듯
                    // MapManager.Instance.mapData[MapManager.Instance.stageNum].Map_Id;

                    break;
                case "_No":
                    if (isChapterStart)
                    {
                        isChapterStart = false;
                        NextButton(false, "_ChapterStart");
                    }

                    ResetCursorPosition();

                    break;
            }
        }
    }

    private void NextButton(bool isCheck, string name)
    {
        nextButton.SetActive(isCheck);

        if (isCheck)
        {
            gameObject.GetComponent<Image>().enabled = false;
        }
        else if (!isCheck)
        {
            gameObject.GetComponent<Image>().enabled = true;
        }

        currentSelected = GameObject.Find(name);
        currentSelected.GetComponent<Selectable>().Select();
    }

    private void ResetCursorPosition()
    {
        if (currentSelected.name == "_ChapterStart")
        {
            rt.anchoredPosition = new Vector2(-860f, MAX_POSITION_Y);
        }
    }
}