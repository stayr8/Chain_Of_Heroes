using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    #region instance ȭ
    public static SaveManager Instance = null;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        if(Instance == null)
        {
            GameObject Entity = new GameObject("SaveManager");

            Instance = Entity.AddComponent<SaveManager>();

            DontDestroyOnLoad(Entity.gameObject);
        }
    }
    #endregion

    public void Save(SimpleJSON.JSONNode Node, string Key, int SlotIndex)
    {
        byte[] Bytes = System.Text.Encoding.UTF8.GetBytes(Node.ToString());

        ES3.SaveRaw(Bytes, Key + SlotIndex.ToString());
    }

    public SimpleJSON.JSONNode Load(string Key, int SlotIndex)
    {
        byte[] Bytes = ES3.LoadRawBytes(Key + SlotIndex);
        string JsonString = System.Text.Encoding.UTF8.GetString(Bytes);

        return SimpleJSON.JSONNode.Parse(JsonString);
    }
}
