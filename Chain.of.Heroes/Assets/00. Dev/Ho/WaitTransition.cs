using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitTransition : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(goToScene());
    }

    private IEnumerator goToScene()
    {
        yield return new WaitForSeconds(31f);
        //yield return new WaitForSeconds(5f);
        LoadingSceneController.LoadScene("Ch_Tutorial");
    }

    private void Update()
    {
        Skip();
    }

    private void Skip()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            MapManager.Instance.stageNum = StageManager.instance.num = 1;
            LoadingSceneController.LoadScene("WorldMapScene");
        }
    }
}