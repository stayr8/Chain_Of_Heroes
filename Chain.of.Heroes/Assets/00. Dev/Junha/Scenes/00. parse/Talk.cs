using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    private string JsonFileName = "SpeechBubble";

    [SerializeField, Header("[�� �ƾƵ�]")] private int MapID;
    private List<SpeechBubble> Speeches = new List<SpeechBubble>();
    [Header("���ȭ��")]
    [SerializeField] private Image Img_Background; // ���ȭ��
    [Header("�ؽ�Ʈ �ڽ�")]
    [SerializeField] private TMP_Text Txt_Name; // �̸�
    [SerializeField] private TMP_Text Txt_Text; // ����
    [SerializeField] private Image Txt_Image; // �̹���

    private int CurrentIndex;
    private Coroutine TalkCoroutine;
    private bool Initialized = false;

    private bool PressedEnter = false;

    [Header("é�� ������")]
    [SerializeField] private GameObject _Parent;
    [SerializeField] private GameObject _Panel;
    [SerializeField] private GameObject _Background;
    [SerializeField] private GameObject _chapterName;

    private float duration = 1f;

    public bool IsEnd { get; private set; } = false;

    public void Initialize(int _MapID)
    {
        MapID = _MapID;

        Txt_Text.text = "";

        string json = Resources.Load<TextAsset>(JsonFileName).text;
        var Node = SimpleJSON.JSON.Parse(json);

        for (int i = 0; i < Node.Count; i++)
        {
            SpeechBubble bubble = new SpeechBubble();
            bubble.Parse(Node[i]);

            if (bubble.MapID == MapID)
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

    private void Start()
    {
        StartCoroutine(Opening());
    }

    private IEnumerator Opening()
    {
        // �г� FadeOut (255f �� 192f)
        yield return new WaitForSeconds(0.5f);
        Image panel = _Panel.GetComponent<Image>();

        float elapsedTime = 0f;
        float targetAlpha = 192f;
        Color originalColor1 = panel.color;
        Color targetColor1 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            panel.color = Color.Lerp(originalColor1, targetColor1, t);

            yield return null;
        }

        // é�� ���ȭ��, é�͸� (0f �� 255f)
        yield return new WaitForSeconds(0.5f);
        Image background = _Background.GetComponent<Image>();
        TMP_Text name = _chapterName.GetComponent<TMP_Text>();

        elapsedTime = 0f;
        targetAlpha = 255f;
        Color originalColor2 = background.color;
        Color targetColor2 = new Color(originalColor2.r, originalColor2.g, originalColor2.b, targetAlpha / 255f);

        Color originalColor3 = name.color;
        Color targetColor3 = new Color(originalColor3.r, originalColor3.g, originalColor3.b, targetAlpha / 255f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            background.color = Color.Lerp(originalColor2, targetColor2, t);
            name.color = Color.Lerp(originalColor3, targetColor3, t);

            yield return null;
        }

        // ��ü ���̵� �ƿ�
        // �г� (192f �� 0f)
        // é�� ���ȭ��, é�͸� (255f �� 0f)
        yield return new WaitForSeconds(1f);

        elapsedTime = 0f;
        targetAlpha = 0f;

        originalColor1 = panel.color;
        targetColor1 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        originalColor2 = background.color;
        targetColor2 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        originalColor3 = name.color;
        targetColor3 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            panel.color = Color.Lerp(originalColor1, targetColor1, t);
            background.color = Color.Lerp(originalColor2, targetColor2, t);
            name.color = Color.Lerp(originalColor3, targetColor3, t);

            yield return null;
        }

        _Parent.SetActive(false);
    }

    private void Update()
    {
        if (Initialized == false || IsEnd) { return; }

        if (Speeches.Count == CurrentIndex)
        {
            IsEnd = true;

            return;
        }

        if (TalkCoroutine == null)
        {
            TalkCoroutine = StartCoroutine(Talking(Speeches[CurrentIndex].StringKR));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (TalkCoroutine != null)
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
            if (PressedEnter)
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