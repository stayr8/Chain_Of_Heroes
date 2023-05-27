using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    private string JsonFileName;

    [SerializeField, Header("[�� �ƾƵ�]")] private int MapID;
    private List<SpeechBubble> Speeches = new List<SpeechBubble>();

    [Header("���ȭ��")]
    [SerializeField] private Image Img_Background; // ���ȭ��

    [Header("�ؽ�Ʈ �ڽ�")]
    [SerializeField] private TMP_Text Txt_Name; // �̸�
    [SerializeField] private TMP_Text Txt_Text; // ����
    [SerializeField] private Image Txt_Image; // �̹���

    private Coroutine TalkCoroutine;
    private int CurrentIndex;

    private bool PressedEnter = false;

    private bool Initialized = false;

    public bool IsEnd { get; private set; } = false;

    public void Initialize(int _MapID)
    {
        MapID = _MapID;

        JsonFileName = "SpeechBubble";
        Txt_Text.text = "";

        string json = Resources.Load<TextAsset>(JsonFileName).text;
        var Node = SimpleJSON.JSON.Parse(json);

        for(int i = 0; i < Node.Count; i ++)
        {
            SpeechBubble bubble = new SpeechBubble();
            bubble.Parse(Node[i]);

            if(bubble.MapID == MapID)
            {
                Speeches.Add(bubble);
            }
        }

        CurrentIndex = 0;

        string path = Speeches[CurrentIndex].BResourcePath;
        Img_Background.sprite = Resources.Load<Sprite>(path);

        TalkCoroutine = StartCoroutine(Talking(Speeches[CurrentIndex].StringKR));

        Initialized = true;
    }

    private void Update()
    {
        if(Initialized == false || IsEnd) { return; }

        if(Speeches.Count == CurrentIndex)
        {
            IsEnd = true;

            return;
        }

        if(TalkCoroutine == null)
        {
            TalkCoroutine = StartCoroutine(Talking(Speeches[CurrentIndex].StringKR));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(TalkCoroutine != null)
            {
                PressedEnter = true;
            }
        }
    }

    IEnumerator Talking(string talk)
    {
        Txt_Text.text = "";
        Txt_Name.text = Speeches[CurrentIndex].Speaker;

        string path = Speeches[CurrentIndex].ResourcePath;
        Txt_Image.sprite = Resources.Load<Sprite>(path);

        foreach (var ch in talk)
        {
            Txt_Text.text += ch;
            yield return new WaitForSeconds(Speeches[CurrentIndex].TextSpeed); // ���ϴ� �ӵ�
            if(PressedEnter)
            {
                break;
            }
        }
        Txt_Text.text = talk;
        yield return new WaitForSeconds(Speeches[CurrentIndex].TalkSpeed); // �� �Ѿ�� �ӵ�

        CurrentIndex++;
        Txt_Text.text = "";
        TalkCoroutine = null;
        yield return null;

        PressedEnter = false;
    }
}