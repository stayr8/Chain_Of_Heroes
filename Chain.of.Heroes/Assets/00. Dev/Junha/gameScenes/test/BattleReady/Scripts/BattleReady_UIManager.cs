using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class BattleReady_UIManager : MonoBehaviour
{
    public event EventHandler OnCharacterChangeFormation;

    [SerializeField, Header("[메뉴] 오브젝트")] private GameObject UI_Menu;
    [SerializeField, Header("[유닛 편성] 오브젝트")] private GameObject UI_UnitFormation;
    [SerializeField, Header("[배치 변경] 오브젝트")] private GameObject UI_ChangeFormation;

    #region instance화 :: Awake()함수 포함
    public static BattleReady_UIManager instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    private void Update()
    {
        UI_STATE();
    }

    private enum STATE { MENU, UNIT_FORMATION, CHANGE_FORMATION }
    private STATE state = STATE.MENU;
    private void UI_STATE()
    {
        switch (state)
        {
            case STATE.MENU:
                break;

            case STATE.UNIT_FORMATION:
                if (!BattleReady_UnitFormationCursor.isOnMenuSelect && Input.GetKeyDown(KeyCode.Escape))
                {
                    UI_UnitFormation.SetActive(false);
                    UI_Menu.SetActive(true);

                    state = STATE.MENU;
                }
                Update_Formation();
                Update_Data();
                break;

            case STATE.CHANGE_FORMATION:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    UI_ChangeFormation.SetActive(false);
                    UI_Menu.SetActive(true);
                    //ChangeFormationSystem.Instance.DestroyCharacterUI();

                    state = STATE.MENU;
                }
                break;
        }
    }

    public void OnUnitFormation()
    {
        state = STATE.UNIT_FORMATION;

        UI_Menu.SetActive(false);
        UI_UnitFormation.SetActive(true);
    }

    public void OnChangeFormation()
    {
        state = STATE.CHANGE_FORMATION;

        UI_Menu.SetActive(false);
        UI_ChangeFormation.SetActive(true);
        OnCharacterChangeFormation?.Invoke(this, EventArgs.Empty);
    }

    public void OnChangeUICreate()
    {
        OnCharacterChangeFormation?.Invoke(this, EventArgs.Empty);
    }

    #region 편성 / 스킬 확인
    [SerializeField, Header("[편성 / 스킬 확인] 오브젝트")] private GameObject menuSelected;
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
    [SerializeField, Header("[미편성] 스프라이트")] private Sprite Img_OffFormation;
    public void OffFormation(GameObject obj)
    {
        // 스프라이트 변경
        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OffFormation;

        // 스프라이트 변경에 따른 포지션 변경
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-41.5f, 18.5f);
        childRT.sizeDelta = new Vector2(83f, 37f);
    }
    [SerializeField, Header("[편성 완료] 스프라이트")] private Sprite Img_OnFormation;
    public void OnFormation(GameObject obj)
    {
        // 스프라이트 변경
        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OnFormation;

        // 스프라이트 변경에 따른 포지션 변경
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-59.5f, 19f);
        childRT.sizeDelta = new Vector2(119f, 38f);
    }
    #endregion

    #region 유닛 편성: 캐릭터 정보 갱신
    [Header("[유닛 편성 수] 텍스트")]
    [SerializeField] private TextMeshProUGUI _Current; // "n"
    [SerializeField] private TextMeshProUGUI _Max; // "/ n"
    private void Update_Formation()
    {
        _Current.text = BattleReady_UnitFormationCursor.count.ToString();
        _Max.text = "/ " + "9"; // 총 캐릭터 수가 9명임.
    }

    [Header("[캐릭터 정보] 텍스트")]
    [SerializeField] private TextMeshProUGUI character_Name;
    [SerializeField] private TextMeshProUGUI character_Class;
    [SerializeField] private TextMeshProUGUI character_Level;
    [SerializeField] private TextMeshProUGUI character_HP;
    [SerializeField] private TextMeshProUGUI character_AttackPower;
    [SerializeField] private TextMeshProUGUI character_ChainAttackPower;
    [SerializeField] private TextMeshProUGUI character_DefensePower;
    private CharacterDataManager data;
    private void Update_Data()
    {
        GameObject obj = BattleReady_UnitFormationCursor.currentSelected;
        if (obj.GetComponentInChildren<CharacterDataManager>() == null)
        {
            return;
        }
        data = obj.GetComponentInChildren<CharacterDataManager>();

        Set_NameAndImage();
        //character_Class.text = data.m_class.ToString();
        character_Level.text = "Lv. " + data.m_level.ToString();
        character_HP.text = data.m_hp.ToString();
        character_AttackPower.text = data.m_attackPower.ToString();
        character_ChainAttackPower.text = data.m_chainAttackPower.ToString();
        character_DefensePower.text = data.m_defensePower.ToString();
    }
    private RectTransform rt;
    [SerializeField] private TextMeshProUGUI character_Skill_1;
    [SerializeField] private TextMeshProUGUI character_Skill_2;
    [SerializeField] private TextMeshProUGUI character_Skill_3;
    private string skill_Content_1;
    private string skill_Content_2;
    private string skill_Content_3;
    private Sprite skill_Image_1;
    private Sprite skill_Image_2;
    private Sprite skill_Image_3;
    [SerializeField] private Image character_Image;
    private void Set_NameAndImage()
    {
        rt = character_Image.gameObject.GetComponent<RectTransform>();

        switch (data.m_name)
        {
            case "Akame": // _1
                character_Name.text = "아카메";
                rt.anchoredPosition = new Vector2(480f, -415f);

                character_Skill_1.text = "신검합일";
                skill_Content_1 = "검과 하나가 되어 치명적인\n" +
                    "공격 능력을 향상시킨다.\n\n" +
                    "치명타 확률 +<color=#ff7f00> 15</color>%\r\n" +
                    "치명타 데미지 +<color=#ff7f00> 20</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "일섬";
                skill_Content_2 = "빠른 속도로 전방의 적 한명을 캐릭터 공격력의 <color=#ff7f00>150</color>% 만큼의 데미지로 1회 벤다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "섬광";
                skill_Content_3 = "전방을 향해 검기를 빠르게 날려 범위 내의 적을 캐릭터 공격력의 <color=#ff7f00>100</color>% 만큼의\n" +
                                  "데미지로 4회 공격한다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Kris": // _2
                character_Name.text = "크리스";
                rt.anchoredPosition = new Vector2(447f, -415f);

                character_Skill_1.text = "신성의 힘";
                skill_Content_1 = "신성한 힘을 통해\n" +
                    "자신의 공격력을 <color=#ff7f00>15</color>% 만큼 증가시킨다.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "신성 강타";
                skill_Content_2 = "신성의 힘이 깃든 검으로\n" +
                    "대상을 강타하여 캐릭터 공격력의 <color=#ff7f00>100</color>% 만큼의 데미지로 1회 공격하고 대상을 2턴간 기절 상태로 만든다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "홀리 오라";
                skill_Content_3 = "신성한 오라를 내뿜어 주변 아군의 공격력을 <color=#ff7f00>30</color>% 만큼 증가시킨다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Teo": // _3
                character_Name.text = "태오";
                rt.anchoredPosition = new Vector2(383f, -415f);

                character_Skill_1.text = "명경지수";
                skill_Content_1 = "맑은 거울과 고요한 물과\n" +
                    "같은 마음가짐을 통해 검의 경지에 다다랐다.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "제비참";
                skill_Content_2 = "섬전과 같은 속도로 전방의 대상을 캐릭터 공격력의 <color=#ff7f00>150</color>% 만큼의 데미지로 3회 공격한다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "반월섬";
                skill_Content_3 = "회전하며 강한 힘으로 발도하여\n" +
                    "전방의 대상을 캐릭터 공격력의 <color=#ff7f00>500</color>% 만큼의 데미지로 1회 공격한다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Melia": // _4
                character_Name.text = "멜리아";
                rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "약점 포착";
                skill_Content_1 = "상대의 약점을 포착해서 좀 더 치명적인 공격을 가할 수 있다.\n\n" +
                    "치명타 확률 + <color=#ff7f00>30</color>%\n" +
                    "치명타 데미지 + <color=#ff7f00>40</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "애로우 블로우";
                skill_Content_2 = "한 명의 적에게 여러 발의\n" +
                    "화살을 발사하여 캐릭터 공격력의 <color=#ff7f00>50</color>% 만큼의 데미지로 4회 공격한다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "스나이핑";
                skill_Content_3 = "상대의 급소를 정확히 노리는 화살을 발사하여 캐릭터 공격력의 <color=#ff7f00>1000</color>% 만큼의 데미지로 1회 공격한다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Platin": // _5
                character_Name.text = "플라틴";
                //rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "굳건한 의지";
                skill_Content_1 = "수호 기사단의 일원인 가디언의 의지는 단단하여 쉽게 깨트릴 수 없다. 자신이 받는 데미지를 <color=#ff7f00>15</color>% 감소시킨다.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "파워 브레이크";
                skill_Content_2 = "전방으로 대검을 휘둘러 캐릭터 공격력의 <color=#ff7f00>150</color>% 만큼의 데미지로 1회 공격한다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "증오의 함성";
                skill_Content_3 = "강하게 포효하여 주변 범위 모든 적의 공격력을 2턴간 <color=#ff7f00>20</color>% 감소시키고 2턴간 도발한다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Raiden": // _6
                character_Name.text = "라이덴";
                //rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "이기어검술";
                skill_Content_1 = "신검의 경지에 이르러 7자루의 검을 자유자재로 다룬다.\n\n" +
                    "치명타 확률 + <color=#ff7f00>10</color>%\n" +
                    "치명타 데미지 + <color=#ff7f00>10</color>%\n" +
                    "캐릭터 공격력 + <color=#ff7f00>10</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "어검 사출";
                skill_Content_2 = "2자루의 검을 사출하여 캐릭터 공격력의 <color=#ff7f00>125</color>% 만큼의 데미지로 2회 공격한다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "섬단";
                skill_Content_3 = "7자루의 검을 합쳐 캐릭터 공격력의 <color=#ff7f00>400</color>% 만큼의 데미지로 1회 공격한다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Eileene": // _7
                character_Name.text = "아일린";
                rt.anchoredPosition = new Vector2(408f, -415f);

                character_Skill_1.text = "빛의 은총";
                skill_Content_1 = "빛의 은총을 받아 회복 계열 스킬의 회복량이 <color=#ff7f00>20</color>% 증가한다.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "재생의 빛";
                skill_Content_2 = "아군에게 재생의 빛을 쏘아 대상의 체력을 <color=#ff7f00>40</color>% 회복시킨다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "대천사의 축복";
                skill_Content_3 = "대천사의 힘으로 범위 내 아군 유닛의 체력을 <color=#ff7f00>20</color>% 회복시킨다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Jave": // _8
                character_Name.text = "제이브";
                //rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "마나 폭주";
                skill_Content_1 = "마나의 흐름을 폭주시켜 낮은 확률로 매우 강력한 공격을 시전한다.\n\n" +
                    "치명타 확률 - <color=#ff7f00>12</color>%\n" +
                    "치명타 데미지 + <color=#ff7f00>300</color>%\n" +
                    "모든 행동력 소모값 -<color=#ff7f00>1</color>";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "독병 투척";
                skill_Content_2 = "전방으로 독약을 던져 범위 내 모든 적의 방어력을 2턴간 <color=#ff7f00>10</color>% 감소시킨다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "메테오";
                skill_Content_3 = "거대한 메테오를 소환하여 캐릭터 공격력의 <color=#ff7f00>300</color>% 만큼의 데미지로 범위 내 적을 2회 공격한다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Vanessa": // _9
                character_Name.text = "바네사";
                rt.anchoredPosition = new Vector2(408f, -336f);

                character_Skill_1.text = "승리의 여신";
                skill_Content_1 = "승리의 여신 발키리는 그 존재만으로도 아군의 사기를 진작합니다.\n\n" +
                    "인접한 주변 아군 공격력 + <color=#ff7f00>5</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "찌르기";
                skill_Content_2 = "전방의 적을 강하게 찔러 캐릭터 공격력의 <color=#ff7f00>200</color>% 만큼의 데미지로 1회 공격한다.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "심판";
                skill_Content_3 = "신의 힘이 담긴 번개를 사용하여 캐릭터 공격력의 <color=#ff7f00>300</color>% 만큼의 데미지로 범위 내 적을 1회 공격하고 대상을 2턴간 기절 상태로 만든다.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;
        }
        character_Image.sprite = Resources.Load<Sprite>("Character/Illustration/" + data.m_name);
    }
    [SerializeField] private Image skill_Image;
    [SerializeField] private TextMeshProUGUI skill_Name;
    [SerializeField] private TextMeshProUGUI skill_Content;
    public void Set_SkillContent(string _num)
    {
        switch (_num)
        {
            case "Skill_1":
                skill_Image.sprite = skill_Image_1;
                skill_Name.text = character_Skill_1.text;
                skill_Content.text = skill_Content_1;
                break;

            case "Skill_2":
                skill_Image.sprite = skill_Image_2;
                skill_Name.text = character_Skill_2.text;
                skill_Content.text = skill_Content_2;
                break;

            case "Skill_3":
                skill_Image.sprite = skill_Image_3;
                skill_Name.text = character_Skill_3.text;
                skill_Content.text = skill_Content_3;
                break;
        }
    }
    #endregion

    #region 유닛 편성: 스킬 확인
    [SerializeField, Header("[스킬 커서] 오브젝트")] private GameObject Skill_Cursor;
    public void OnSkillCursor()
    {
        Skill_Cursor.SetActive(true);
    }
    public void OffSkillCursor()
    {
        Skill_Cursor.SetActive(false);
    }

    [SerializeField, Header("[스킬 설명] 오브젝트")] private GameObject Skill_Detail;
    public void OnSkillDetail()
    {
        Skill_Detail.SetActive(true);
    }
    public void OffSkillDetail()
    {
        Skill_Detail.SetActive(false);
    }
    #endregion
}