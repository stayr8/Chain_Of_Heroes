using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterInfo : MonoBehaviour
{
    [Serializable]
    public class InfoData
    {
        public string ChapterNumAndName;
        public bool isUnLock;
        public bool isClear;

        //public Image Img_reward1;
        //public string Name_reward1;

        //public Image Img_reward2;
        //public string Name_reward2;
    }
    //public InfoData Info = new InfoData();

    private void Start()
    {
        //Info.ChapterNumAndName = gameObject.name;
        //Info.isUnLock = false;
        //Info.isClear = false;
    }
}
