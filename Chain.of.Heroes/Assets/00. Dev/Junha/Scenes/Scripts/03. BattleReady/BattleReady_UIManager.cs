using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using System;

public class BattleReady_UIManager : MonoBehaviour
{
    #region instance화 :: Awake()함수 포함
    [Header("===== [instance 전용 property] =====")]
    [Header("[유닛 슬롯] 오브젝트")] public GameObject[] slot;

    public static BattleReady_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public int Max_Value;
    private Talk TextBox;

    [Header("============================\n\n[UI Canvas] 오브젝트")]
    [SerializeField] private GameObject _Menu;
    [SerializeField] private GameObject _UnitFormation;
    [SerializeField] private GameObject _ChangeFormation;

    [SerializeField] private bool ischange_formationCamera;

    public event EventHandler OnCharacterChangeFormation;

    private enum STATE { MENU, UNIT_FORMATION, CHANGE_FORMATION }
    private STATE state = STATE.MENU;

    [SerializeField, Header("현재 편성 유닛 수")] private TMP_Text _Current; // "n"
    [SerializeField, Header("총 조우한 유닛 수")] private TMP_Text _Max; // "/ n"

    [SerializeField, Header("[편성 / 스킬 확인] 오브젝트")] private GameObject menuSelected;

    [Header("[캐릭터 정보] 텍스트")]
    [SerializeField] private TextMeshProUGUI character_Name;
    [SerializeField] private TextMeshProUGUI character_Class;
    [SerializeField] private TextMeshProUGUI character_Level;
    [SerializeField] private TextMeshProUGUI character_HP;
    [SerializeField] private TextMeshProUGUI character_AttackPower;
    [SerializeField] private TextMeshProUGUI character_ChainAttackPower;
    [SerializeField] private TextMeshProUGUI character_DefensePower;
    private CharacterDataManager data;
    private BattleReady_FormationState form_State;
    private Sprite no_Skill;
    [Header("캐릭터 스킬 이미지")]
    [SerializeField] private Image[] character_Skill_Image;
    [Header("캐릭터 스킬 이름")]
    [SerializeField] private TextMeshProUGUI[] character_Skill_Name;
    [Header("캐릭터 이미지")]
    [SerializeField] private Image character_Background;
    [SerializeField] private Image character_Image;
    private RectTransform rt;

    private string path = "Character/Skill/";
    private string skill_Content_1;
    private string skill_Content_2;
    private string skill_Content_3;

    [Header("스킬 확인 정보")]
    [SerializeField] private Image skill_Image;
    [SerializeField] private TextMeshProUGUI skill_Name;
    [SerializeField] private TextMeshProUGUI skill_Content;

    [SerializeField, Header("[챕터 장] 텍스트")] private TMP_Text Txt_chapterNum;
    [SerializeField, Header("[챕터명] 텍스트")] private TMP_Text Txt_chapterName;

    [SerializeField, Header("[스킬 커서] 오브젝트")] private GameObject Skill_Cursor;
    [SerializeField, Header("[스킬 설명] 오브젝트")] private GameObject Skill_Detail;

    private void Start()
    {
        Max_Value = MapManager.Instance.mapData[MapManager.Instance.stageNum].Count_Unlock;
        Set_ChapterNumName();

        StartCoroutine(TalkStart());
    }

    private IEnumerator TalkStart()
    {
        _Menu.SetActive(false);
        TextBox = Instantiate(Resources.Load<GameObject>("TextBox")).GetComponent<Talk>();

        TextBox.Initialize(MapManager.Instance.stageNum);
        yield return new WaitUntil(() => TextBox.IsEnd);

        _Menu.SetActive(true);

        Destroy(TextBox.gameObject);
    }

    private void Update()
    {
        UI_STATE();
    }

    private void UI_STATE()
    {
        switch (state)
        {
            case STATE.MENU:



                break;

            case STATE.UNIT_FORMATION:

                OffUnitFormation();

                Update_Formation();
                Update_Data();

                break;

            case STATE.CHANGE_FORMATION:

                OffChangeFormation();

                break;
        }
    }

    #region [유닛 편성]
    public void OnUnitFormation()
    {
        _Menu.SetActive(false);
        _UnitFormation.SetActive(true);

        state = STATE.UNIT_FORMATION;
    }
    private void OffUnitFormation()
    {
        if (!BattleReady_UnitFormationCursor.isOnMenuSelect && Input.GetKeyDown(KeyCode.Escape))
        {
            _UnitFormation.SetActive(false);
            _Menu.SetActive(true);

            state = STATE.MENU;
        }
    }
    #endregion

    #region [배치 변경]
    public void OnChangeFormation()
    {
        _Menu.SetActive(false);
        _ChangeFormation.SetActive(true);
        OnCharacterChangeFormation?.Invoke(this, EventArgs.Empty);

        state = STATE.CHANGE_FORMATION;
    }
    private void OffChangeFormation()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _ChangeFormation.SetActive(false);
            _Menu.SetActive(true);
            ischange_formationCamera = false;

            state = STATE.MENU;
        }
        else
        {
            ischange_formationCamera = true;
        }
    }
    #endregion



    #region [현재 편성된 유닛 수 / 총 조우한 유닛 수]
    private void Update_Formation()
    {
        _Current.text = BattleReady_UnitFormationCursor.count.ToString();
        _Max.text = "/ " + Max_Value; // 총 캐릭터 수가 9명임.
    }
    #endregion

    #region [편성 / 스킬 확인]
    public void OnMenuSelected()
    {
        SoundManager.instance.Sound_SelectMenu();
        menuSelected.SetActive(true);
    }
    public void OffMenuSelected()
    {
        SoundManager.instance.Sound_SelectMenu();
        menuSelected.SetActive(false);
    }
    #endregion

    #region 미편성 / 편성 완료
    public void UnlockCharacter(GameObject obj)
    {
        //캐릭터 언락 상태에 따른 캐릭터 스프라이트 명암 변경
        ChangeColor(obj, 0f, 0f, 0f, 255f);

        GameObject child = obj.transform.GetChild(1).gameObject;
        if (!obj.GetComponent<BattleReady_FormationState>().isUnlock)
        {
            child.SetActive(false);
        }
    }
    public void Formation(GameObject obj)
    {
        GameObject child = obj.transform.GetChild(1).gameObject;
        child.SetActive(true);

        Image img = child.GetComponent<Image>();

        RectTransform childRT = child.GetComponent<RectTransform>();
        if (obj.GetComponent<BattleReady_FormationState>().isFormationState)
        {
            //편성 상태에 따른 캐릭터 스프라이트 명암 변경
            ChangeColor(obj, 255f, 255f, 255f, 255f);

            //편성 스프라이트 변경 및 포지션 변경
            img.sprite = Resources.Load<Sprite>("team_forming");
            childRT.anchoredPosition = new Vector2(-59.5f, 19f);

            ChangeFormationSystem.Instance.AnyDestroyCharacterUI();
        }
        else //!obj.GetComponent<BattleReady_FormationState>().isFormationState
        {
            //편성 상태에 따른 캐릭터 스프라이트 명암 변경
            ChangeColor(obj, 48f, 48f, 48f, 255f);

            //편성 스프라이트 변경 및 포지션 변경
            img.sprite = Resources.Load<Sprite>("team_unforming");
            childRT.anchoredPosition = new Vector2(-41.5f, 18.5f);
        }
        img.SetNativeSize();
    }

    private void ChangeColor(GameObject obj, float r, float g, float b, float a)
    {
        GameObject child = obj.transform.GetChild(0).gameObject;
        Image img = child.GetComponent<Image>();
        img.color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
    }
    #endregion

    #region 유닛 편성: 캐릭터 정보 갱신
    private void Update_Data()
    {
        GameObject obj = BattleReady_UnitFormationCursor.currentSelected;
        if (obj.GetComponentInChildren<CharacterDataManager>() == null)
        {
            return;
        }
        data = obj.GetComponentInChildren<CharacterDataManager>();

        form_State = obj.GetComponentInChildren<BattleReady_FormationState>();
        no_Skill = Resources.Load<Sprite>("slot_image");
        rt = character_Image.gameObject.GetComponent<RectTransform>();
        if (!form_State.isUnlock)
        {
            character_Name.text = "???";
            character_Class.text = "???/???";
            character_Level.text = "Lv. ???";
            character_HP.text = "???";
            character_AttackPower.text = "???";
            character_ChainAttackPower.text = "???";
            character_DefensePower.text = "???";

            character_Background.sprite = Resources.Load<Sprite>(data.m_back_resourcePath);
            character_Background.color = new Color(64 / 255f, 64 / 255f, 64 / 255f, 255 / 255f);

            character_Image.sprite = Resources.Load<Sprite>(data.m_resourcePath);
            character_Image.color = Color.black;

            for (int i = 0; i < character_Skill_Image.Length; ++i)
            {
                character_Skill_Image[i].sprite = no_Skill;
                character_Skill_Name[i].text = "???";
            }
        }
        else // form_State.isUnlock
        {
            Set_NameAndImage();

            character_Name.text = data.m_name.ToString();
            character_Class.text = data.m_class.ToString();
            character_Level.text = "Lv. " + data.m_level.ToString();
            character_HP.text = data.m_hp.ToString();
            character_AttackPower.text = data.m_attackPower.ToString();
            character_ChainAttackPower.text = data.m_chainAttackPower.ToString();
            character_DefensePower.text = data.m_defensePower.ToString();

            character_Image.color = character_Background.color = Color.white;
        }
    }
    #endregion

    #region 유닛 편성: 캐릭터 스킬 관련
    private void Set_NameAndImage()
    {
        character_Background.sprite = Resources.Load<Sprite>(data.m_back_resourcePath);
        character_Image.sprite = Resources.Load<Sprite>(data.m_resourcePath);

        switch (data.m_name)
        {
            #region No.1 아카메, SwordWoman
            case "아카메":
                rt.anchoredPosition = new Vector2(480f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill01_Sww");
                character_Skill_Name[0].text = "신검합일";
                skill_Content_1 = "검과 하나가 되어 치명적인 공격 능력을 향상시킨다.\n\n" +
                    "치명타 확률 +<color=#ff7f00>15</color>%\n" +
                    "치명타 데미지 +<color=#ff7f00>20</color>%";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill02_Sww");
                character_Skill_Name[1].text = "일섬";
                skill_Content_2 = "빠른 속도로 전방의 적 한명을 캐릭터 공격력의 <color=#ff7f00>150</color>% 만큼의 데미지로 <color=#ff7f00>1</color>회 벤다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill03_Sww");
                character_Skill_Name[2].text = "섬광";
                skill_Content_3 = "전방을 향해 검기를 빠르게 날려 범위 내의 적을 캐릭터 공격력의 <color=#ff7f00>100</color>% 만큼의 데미지로 <color=#ff7f00>4</color>회 공격한다.";

                break;
            #endregion

            #region No.2 크리스, Knight
            case "크리스":
                rt.anchoredPosition = new Vector2(447f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Knight/Skill01_Kni");
                character_Skill_Name[0].text = "신성의 힘";
                skill_Content_1 = "신성한 힘을 통해 자신의 공격력을 <color=#ff7f00>15</color>% 만큼 증가시킨다.";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Knight/Skill02_Kni");
                character_Skill_Name[1].text = "신성 강타";
                skill_Content_2 = "신성의 힘이 깃든 검으로 대상을 강타하여 캐릭터 공격력의 <color=#ff7f00>100</color>% 만큼의 데미지로 <color=#ff7f00>1</color>회 공격하고 대상을 <color=#ff7f00>2</color>턴간 기절 상태로 만든다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Knight/Skill03_Kni");
                character_Skill_Name[2].text = "홀리 오라";
                skill_Content_3 = "신성한 오라를 내뿜어 주변 아군의 공격력을 <color=#ff7f00>30</color>% 만큼 증가시킨다.";

                break;
            #endregion

            #region No.3 카미나, Samurai
            case "카미나":
                rt.anchoredPosition = new Vector2(383f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Samurai/Skill01_Sam");
                character_Skill_Name[0].text = "명경지수";
                skill_Content_1 = "맑은 거울과 고요한 물과 같은 마음가짐을 통해 검의 경지에 다다랐다.\n\n" +
                    "치명타 확률 +<color=#ff7f00>20</color>%\n" +
                    "치명타 데미지 +<color=#ff7f00>30</color>%\n" +
                    "스킬 행동력 소모값 -<color=#ff7f00>1</color>";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Samurai/Skill02_Sam");
                character_Skill_Name[1].text = "제비참";
                skill_Content_2 = "섬전과 같은 속도로 전방의 대상을 캐릭터 공격력의 <color=#ff7f00>150</color>% 만큼의 데미지로 <color=#ff7f00>3</color>회 공격한다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Samurai/Skill03_Sam");
                character_Skill_Name[2].text = "반월섬";
                skill_Content_3 = "회전하며 강한 힘으로 발도하여 전방의 대상을 캐릭터 공격력의 <color=#ff7f00>500</color>% 만큼의 데미지로 <color=#ff7f00>1</color>회 공격한다.";

                break;
            #endregion

            #region No.4 멜리사, Archer
            case "멜리사":
                rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Archer/Skill01_Arc");
                character_Skill_Name[0].text = "약점 포착";
                skill_Content_1 = "상대의 약점을 포착해서 좀 더 치명적인 공격을 가할 수 있다.\n\n" +
                    "치명타 확률 + <color=#ff7f00>30</color>%\n" +
                    "치명타 데미지 + <color=#ff7f00>40</color>%";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Archer/Skill02_Arc");
                character_Skill_Name[1].text = "애로우 블로우";
                skill_Content_2 = "한 명의 적에게 여러 발의\n" +
                    "화살을 발사하여 캐릭터 공격력의 <color=#ff7f00>50</color>% 만큼의 데미지로 <color=#ff7f00>4</color>회 공격한다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Archer/Skill03_Arc");
                character_Skill_Name[2].text = "스나이핑";
                skill_Content_3 = "상대의 급소를 정확히 노리는 화살을 발사하여 캐릭터 공격력의 <color=#ff7f00>1000</color>% 만큼의 데미지로 <color=#ff7f00>1</color>회 공격한다.";

                break;
            #endregion

            #region No.5 플라틴, Guardian
            case "플라틴":
                rt.anchoredPosition = new Vector2(480f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Guardian/Skill01_Gur");
                character_Skill_Name[0].text = "굳건한 의지";
                skill_Content_1 = "수호 기사단의 일원인 가디언의 의지는 단단하여 쉽게 깨트릴 수 없다. 자신이 받는 데미지를 <color=#ff7f00>15</color>% 감소시킨다.";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Guardian/Skill02_Gur");
                character_Skill_Name[1].text = "파워 브레이크";
                skill_Content_2 = "전방으로 대검을 휘둘러 캐릭터 공격력의 <color=#ff7f00>150</color>% 만큼의 데미지로 <color=#ff7f00>1</color>회 공격한다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Guardian/Skill03_Gur");
                character_Skill_Name[2].text = "증오의 함성";
                skill_Content_3 = "강하게 포효하여 주변 범위 모든 적의 공격력을 <color=#ff7f00>2</color>턴간 <color=#ff7f00>20</color>% 감소시키고 <color=#ff7f00>2</color>턴간 도발한다.";

                break;
            #endregion

            #region No.6 아이네, Priest
            case "아이네":
                rt.anchoredPosition = new Vector2(408f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Priest/Skill01_Pri");
                character_Skill_Name[0].text = "빛의 은총";
                skill_Content_1 = "빛의 은총을 받아 회복 계열 스킬의 회복량이 <color=#ff7f00>20</color>% 증가한다.";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Priest/Skill02_Pri");
                character_Skill_Name[1].text = "재생의 빛";
                skill_Content_2 = "아군에게 재생의 빛을 쏘아 대상의 체력을 <color=#ff7f00>40</color>% 회복시킨다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Priest/Skill03_Pri");
                character_Skill_Name[2].text = "대천사의 축복";
                skill_Content_3 = "대천사의 힘으로 범위 내 아군 유닛의 체력을 <color=#ff7f00>20</color>% 회복시킨다.";

                break;
            #endregion

            #region No.7 제이브, Wizard
            case "제이브":
                rt.anchoredPosition = new Vector2(408f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Wizard/Skill01_Wiz");
                character_Skill_Name[0].text = "마나 폭주";
                skill_Content_1 = "마나의 흐름을 폭주시켜 낮은 확률로 매우 강력한 공격을 시전한다.\n\n" +
                    "치명타 확률 - <color=#ff7f00>12</color>%\n" +
                    "치명타 데미지 + <color=#ff7f00>300</color>%\n" +
                    "모든 행동력 소모값 -<color=#ff7f00>1</color>";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Wizard/Skill02_Wiz");
                character_Skill_Name[1].text = "독병 투척";
                skill_Content_2 = "전방으로 독약을 던져 범위 내 모든 적의 방어력을 <color=#ff7f00>2</color>턴간 <color=#ff7f00>10</color>% 감소시킨다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Wizard/Skill03_Wiz");
                character_Skill_Name[2].text = "메테오";
                skill_Content_3 = "거대한 메테오를 소환하여 캐릭터 공격력의 <color=#ff7f00>300</color>% 만큼의 데미지로 범위 내 적을 <color=#ff7f00>2</color>회 공격한다.";

                break;
            #endregion

            #region No.8 바네사, Valkyrie
            case "바네사":
                rt.anchoredPosition = new Vector2(408f, -336f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill01_Val");
                character_Skill_Name[0].text = "승리의 여신";
                skill_Content_1 = "승리의 여신 발키리는 그 존재만으로도 아군의 사기를 진작합니다.\n\n" +
                    "인접한 주변 아군 공격력 +<color=#ff7f00>5</color>%";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill02_Val");
                character_Skill_Name[1].text = "찌르기";
                skill_Content_2 = "전방의 적을 강하게 찔러 캐릭터 공격력의 <color=#ff7f00>200</color>% 만큼의 데미지로 <color=#ff7f00>1</color>회 공격한다.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill03_Val");
                character_Skill_Name[2].text = "심판";
                skill_Content_3 = "신의 힘이 담긴 번개를 사용하여 캐릭터 공격력의 <color=#ff7f00>300</color>% 만큼의 데미지로 범위 내 적을 <color=#ff7f00>1</color>회 공격하고 대상을 <color=#ff7f00>2</color>턴간 기절 상태로 만든다.";

                break;
                #endregion
        }
    }

    public void Set_SkillContent(string _num)
    {
        switch (_num)
        {
            case "Skill_1":
                skill_Image.sprite = character_Skill_Image[0].sprite;
                skill_Name.text = character_Skill_Name[0].text;
                skill_Content.text = skill_Content_1;
                break;

            case "Skill_2":
                skill_Image.sprite = character_Skill_Image[1].sprite;
                skill_Name.text = character_Skill_Name[1].text;
                skill_Content.text = skill_Content_2;
                break;

            case "Skill_3":
                skill_Image.sprite = character_Skill_Image[2].sprite;
                skill_Name.text = character_Skill_Name[2].text;
                skill_Content.text = skill_Content_3;
                break;
        }
    }
    #endregion

    #region 유닛 편성: 스킬 확인
    public void OnSkillCursor()
    {
        Skill_Cursor.SetActive(true);
    }
    public void OffSkillCursor()
    {
        Skill_Cursor.SetActive(false);
    }

    public void OnSkillDetail()
    {
        Skill_Detail.SetActive(true);
    }
    public void OffSkillDetail()
    {
        Skill_Detail.SetActive(false);
    }
    #endregion

    #region 챕터명
    private void Set_ChapterNumName()
    {
        Txt_chapterNum.text = "제 " + StageManager.instance.m_chapterNum.ToString() + "장";
        Txt_chapterName.text = StageManager.instance.m_chapterName.ToString();
    }
    #endregion

    public bool GetChange_FormationCamera()
    {
        return ischange_formationCamera;
    }
}