using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterDataManager : MonoBehaviour
{
    [SerializeField, Header("√©≈Õ")] private ChapterInfo[] chapter;
    [SerializeField, Header("√©≈Õ ¡§∫∏")] private ChapterInfo.InfoData[] info;

    [SerializeField, Header("√©≈Õ ¿·±Ë")] private Sprite Img_ChapterClose;
    [SerializeField, Header("√©≈Õ ¡¯«‡¡ﬂ")] private Sprite Img_ChapterOpen;
    [SerializeField, Header("√©≈Õ ≈¨∏ÆæÓ")] private Sprite Img_ChapterClear;

    private void Start()
    {
        // ∫£¿ÃΩ∫ ƒ∑«¡ √Î±ﬁ
        info[0].isUnLock = true;
        info[0].isClear = true;

        for (int i = 0; i < chapter.Length; ++i)
        {
            info[i].ChapterNumAndName = chapter[i].gameObject.name;
        }
    }

    private void Update()
    {
        ChapterImageControl();

        ClearControl();
    }

    private void ChapterImageControl()
    {
        if (info[0].isUnLock)
        {
            if (info[0].isClear)
            {
                chapter[0].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClear;
                info[1].isUnLock = true;
            }
            else
            {
                chapter[0].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterOpen;
            }
        }
        else
        {
            chapter[0].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClose;
        }

        if (info[1].isUnLock)
        {
            if (info[1].isClear)
            {
                chapter[1].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClear;
                info[2].isUnLock = true;
            }
            else
            {
                chapter[1].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterOpen;
            }
        }
        else
        {
            chapter[1].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClose;
        }

        if (info[2].isUnLock)
        {
            if (info[2].isClear)
            {
                chapter[2].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClear;
                info[3].isUnLock = true;
            }
            else
            {
                chapter[2].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterOpen;
            }
        }
        else
        {
            chapter[2].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClose;
        }

        if (info[3].isUnLock)
        {
            if (info[3].isClear)
            {
                chapter[3].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClear;
            }
            else
            {
                chapter[3].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterOpen;
            }
        }
        else
        {
            chapter[3].gameObject.GetComponent<SpriteRenderer>().sprite = Img_ChapterClose;
        }
    }

    private void ClearControl()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            info[1].isClear = true;
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            info[2].isClear = true;
        }

        if(Input.GetKeyDown(KeyCode.F3))
        {
            info[3].isClear = true;
        }
    }
}
