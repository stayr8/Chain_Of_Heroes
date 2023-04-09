using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap_UIManager : MonoBehaviour
{
    [SerializeField, Header("Select Menu UI")] private GameObject UI_SelectMenu;
    [SerializeField, Header("\nChapterInfo UI")] private GameObject UI_ChapterInfo;
    [SerializeField, Header("\nChapterInfo ¹è°æ")] private GameObject ChapterInfo_Background;
    private RectTransform rt_ChapterInfo;

    private void Awake()
    {
        rt_ChapterInfo = ChapterInfo_Background.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Controller_WorldMapMenu();

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log(WorldMap_MenuSelectCursor.isChapterStart);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log(WorldMap_MenuSelectCursor.isBaseCamp);
        }
    }

    private bool isOnWorldMapMenu = false;
    private void Controller_WorldMapMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (!isOnWorldMapMenu && Input.GetKeyDown(KeyCode.Return)))
        {
            if (!isOnWorldMapMenu)
            {
                isOnWorldMapMenu = true;
                UI_SelectMenu.gameObject.SetActive(true);

                rt_ChapterInfo.anchoredPosition = new Vector2(rt_ChapterInfo.anchoredPosition.x, 0f);
            }
            else if (isOnWorldMapMenu)
            {
                if (WorldMap_MenuSelectCursor.isChapterStart || WorldMap_MenuSelectCursor.isBaseCamp)
                {
                    return;
                }

                isOnWorldMapMenu = false;
                UI_SelectMenu.gameObject.SetActive(false);

                rt_ChapterInfo.anchoredPosition = new Vector2(rt_ChapterInfo.anchoredPosition.x, -315f);
            }
        }
    }
}