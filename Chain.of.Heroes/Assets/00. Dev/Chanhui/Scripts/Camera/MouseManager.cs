using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private static MouseManager instance = null;

    [SerializeField] Texture2D cursorImg;

    private void Awake()
    {
        if(null == instance)
        {
            // 씬 시작될 때 인스턴스 초기화, 씬을 넘어갈때도 유지되기위한 처리
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // instance가, GameManager가 존재한다면 GameObject 제거
            Destroy(gameObject);
        }
    }

    public static MouseManager Instance
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    private void Start()
    {
        Cursor.visible = true;
        //Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.ForceSoftware);
    }
   
}
