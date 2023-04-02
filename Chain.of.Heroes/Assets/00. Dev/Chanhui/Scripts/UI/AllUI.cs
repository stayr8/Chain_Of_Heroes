using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AllUI : MonoBehaviour
{



    public void SceneChange()
    {
        string nowbutton = EventSystem.current.currentSelectedGameObject.name;
        switch (nowbutton)
        {
            case "Stage1":
                Debug.Log("1스테이지");
                MapManager.Instance.stageNum = 0;
                break;
            case "Stage2":
                Debug.Log("2스테이지");
                MapManager.Instance.stageNum = 1;
                break;
        }

        SceneManager.LoadScene("Chan");
    }

    public void NextScene()
    {
        SceneManager.LoadScene("ChoiceScene");
    }
}
