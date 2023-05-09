using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewData", menuName = "Data")]
public class MyData : ScriptableObject
{
    public int value;
    
    public void Initialize(int initialValue)
    {
        value = initialValue;
    }
}

public class Testing : MonoBehaviour
{
    private void Start()
    {
        CreateData();
    }

    public void CreateData()
    {
        Debug.Log("����");
        MyData newData = ScriptableObject.CreateInstance<MyData>();
        
        newData.Initialize(10);
        // ������ ScriptableObject�� ���� ����� �� �ֽ��ϴ�.
    }
}
