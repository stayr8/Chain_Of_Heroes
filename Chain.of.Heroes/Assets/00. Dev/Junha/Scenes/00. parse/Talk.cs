using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    private string JsonFileName = "SpeechBubble";

    [SerializeField, Header("[맵 아아디]")] private int MapID;
    private List<SpeechBubble> Speeches = new List<SpeechBubble>();
    [Header("배경화면")]
    [SerializeField] private Image Img_Background; // 배경화면
    [Header("텍스트 박스")]
    [SerializeField] private GameObject _Parent;
    [SerializeField] private TMP_Text Txt_Name; // 이름
    [SerializeField] private TMP_Text Txt_Text; // 내용
    [SerializeField] private Image Txt_Image; // 이미지

    private int CurrentIndex;
    private Coroutine TalkCoroutine;
    private bool Initialized = false;

    private bool PressedEnter = false;

    [Header("챕터 오프닝")]
    [SerializeField] private Image _Panel;
    [SerializeField] private Image _Background;
    [SerializeField] private TMP_Text _chapterName;

    private float duration = 1f;

    public bool IsEnd { get; private set; } = false;

    private bool isShow = false;

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

        isShow = false;

        int temp = StageManager.instance.m_chapterNum;
        string str = StageManager.instance.m_chapterName;

        _chapterName.text = temp < 10 ? "Chapter. 0" + temp + "\n" + str :
                                        "Chapter. " + temp + "\n" + str;

        StartCoroutine(Opening());

        Initialized = true;
    }

    private IEnumerator Opening()
    {
        // 패널 FadeOut (255f → 192f)
        yield return new WaitForSeconds(0.5f);

        float elapsedTime = 0f;
        float targetAlpha = 192f;
        Color originalColor1 = _Panel.color;
        Color targetColor1 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            _Panel.color = Color.Lerp(originalColor1, targetColor1, t);

            yield return null;
        }

        // 챕터 배경화면, 챕터명 (0f → 255f)
        yield return new WaitForSeconds(0.5f);

        elapsedTime = 0f;
        targetAlpha = 255f;
        Color originalColor2 = _Background.color;
        Color targetColor2 = new Color(originalColor2.r, originalColor2.g, originalColor2.b, targetAlpha / 255f);

        Color originalColor3 = _chapterName.color;
        Color targetColor3 = new Color(originalColor3.r, originalColor3.g, originalColor3.b, targetAlpha / 255f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            _Background.color = Color.Lerp(originalColor2, targetColor2, t);
            _chapterName.color = Color.Lerp(originalColor3, targetColor3, t);

            yield return null;
        }

        // 전체 페이드 아웃
        // 패널 (192f → 0f)
        // 챕터 배경화면, 챕터명 (255f → 0f)
        yield return new WaitForSeconds(1f);

        elapsedTime = 0f;
        targetAlpha = 0f;

        originalColor1 = _Panel.color;
        targetColor1 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        originalColor2 = _Background.color;
        targetColor2 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        originalColor3 = _chapterName.color;
        targetColor3 = new Color(originalColor1.r, originalColor1.g, originalColor1.b, targetAlpha / 255f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);

            _Panel.color = Color.Lerp(originalColor1, targetColor1, t);
            _Background.color = Color.Lerp(originalColor2, targetColor2, t);
            _chapterName.color = Color.Lerp(originalColor3, targetColor3, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        _Parent.SetActive(true);

        isShow = true;
    }

    private void Update()
    {
        if(!isShow) { return; }

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
        yield return new WaitUntil(() => isShow);

        Txt_Text.text = "";
        Txt_Name.text = Speeches[CurrentIndex].Speaker;

        string path = Speeches[CurrentIndex].ResourcePath;
        Txt_Image.sprite = Resources.Load<Sprite>(path);

        foreach (var ch in talk)
        {
            Txt_Text.text += ch;
            yield return new WaitForSeconds(Speeches[CurrentIndex].TextSpeed); // 말하는 속도
            if (PressedEnter)
            {
                break;
            }
        }
        Txt_Text.text = talk;
        yield return new WaitForSeconds(Speeches[CurrentIndex].TalkSpeed); // 글 넘어가는 속도

        CurrentIndex++;
        Txt_Text.text = "";
        TalkCoroutine = null;
        yield return null;

        PressedEnter = false;
    }
}