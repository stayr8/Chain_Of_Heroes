using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
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
        // �ؽ��� �ڷ��� ������ ����
        Texture2D textuer = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();
        // ȭ���� �ȼ� �����͸� �о �ؽ���ȭ �ϴ� ����
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
