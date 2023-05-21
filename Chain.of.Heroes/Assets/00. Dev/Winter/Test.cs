using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private IEnumerator Start()
    {
        SimpleJSON.JSONObject SaveData = new SimpleJSON.JSONObject();
        SaveData.Add("ID", 1);
        SaveData.Add("StageNumber", 2);

        SaveManager.Instance.Save(SaveData, "SaveData", 0);
        
        yield return new WaitForSeconds(1f);

        if(ES3.FileExists("SaveData0"))
        {
            var LoadData = SaveManager.Instance.Load("SaveData", 0);

            int ID = LoadData["ID"].AsInt;
            int StageNumber = LoadData["StageNumber"].AsInt;

            //UI.Text = "{ID}번 파일 존재";

            Debug.Log(ID);
            Debug.Log(StageNumber);
        }
    }
}
