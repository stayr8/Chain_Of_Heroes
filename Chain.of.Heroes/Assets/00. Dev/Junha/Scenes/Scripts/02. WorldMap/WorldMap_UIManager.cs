using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap_UIManager : MonoBehaviour
{
    [SerializeField, Header("Select Menu UI")] private GameObject UI_SelectMenu;
    [SerializeField, Header("ChapterInfo UI")] private GameObject UI_ChapterInfo;
    [SerializeField, Header("ChapterInfo ¹è°æ")] private GameObject ChapterInfo_Background;
    private RectTransform rt_ChapterInfo;

    private void Awake()
    {
        rt_ChapterInfo = ChapterInfo_Background.GetComponent<RectTransform>();
    }

    private void Update()
    {
        Controller_WorldMapMenu();
    }

    public static bool isOnWorldMapMenu = false;
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

            SoundManager.instance.Sound_WorldMapUIOpen();
        }
    }
}