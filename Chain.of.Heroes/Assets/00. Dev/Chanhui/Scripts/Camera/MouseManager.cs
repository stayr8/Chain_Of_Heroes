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
            // �� ���۵� �� �ν��Ͻ� �ʱ�ȭ, ���� �Ѿ���� �����Ǳ����� ó��
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // instance��, GameManager�� �����Ѵٸ� GameObject ����
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
