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
        //yield return new WaitForSeconds(31f);
        yield return new WaitForSeconds(1f);
        LoadingSceneController.LoadScene("Ch_Tutorial"); // Ʃ�丮�� ������ �����ؾ� ��
    }
}