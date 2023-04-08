using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class UIManager_WorldMap : MonoBehaviour
{
    [SerializeField, Header("Menu UI")] private GameObject UI_Menu;
    public static bool isOnMenu = false;

    [SerializeField, Header("ChapterInfo UI")] private GameObject UI_ChapterInfo;
    [SerializeField, Header("ChapterInfo ¹è°æ")] private GameObject ChapterInfo_Background;
    private RectTransform ChapterInfo_rt;

    public static TextMeshProUGUI _tmp;

    private void Awake()
    {
        ChapterInfo_rt = ChapterInfo_Background.GetComponent<RectTransform>();
        //_tmp = GameObject.Find("ChapterName_tmp").GetComponent<TextMeshProUGUI>();

    }

    private void Update()
    {
        Function();
    }

    private void Function()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (!isOnMenu && Input.GetKeyDown(KeyCode.Return)))
        {
            WorldMapMenu();
        }
    }

    private void WorldMapMenu()
    {
        if (!isOnMenu)
        {
            UI_Menu.SetActive(true);
            isOnMenu = true;

            ChapterInfo_rt.anchoredPosition = new Vector2(ChapterInfo_rt.anchoredPosition.x, 406f);
        }
        else if (isOnMenu)
        {
            UI_Menu.SetActive(false);
            isOnMenu = false;

            ChapterInfo_rt.anchoredPosition = new Vector2(ChapterInfo_rt.anchoredPosition.x, 60f);
        }
    }
}