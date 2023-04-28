using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager m_instance;
    public static ScreenManager _instance;


    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance= this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public Texture2D ScreenTextuer;
    public GameObject Raw;

    IEnumerator CaptuerScreen()
    {
        Debug.Log("디졸브 발동");
        // 텍스쳐 자료형 변수를 생성
        Texture2D textuer = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        // 화면의 픽셀 데이터를 읽어서 텍스쳐화 하는 과정
        textuer.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        textuer.Apply();
        ScreenTextuer= textuer;
        OnRawScreen();
    }

    public void _LoadScreenTextuer()
    {
        StartCoroutine(CaptuerScreen());
    }

    public void OnRawScreen()
    {
        Raw.SetActive(true);
    }

    public void OffRawScreen()
    {
        Raw.SetActive(false);
    }
}
