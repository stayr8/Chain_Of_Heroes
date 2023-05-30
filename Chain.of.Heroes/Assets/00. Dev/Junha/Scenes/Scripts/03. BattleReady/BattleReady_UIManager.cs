using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;
using System;

public class BattleReady_UIManager : MonoBehaviour
{
    #region instanceȭ :: Awake()�Լ� ����
    [Header("===== [instance ���� property] =====")]
    [Header("[���� ����] ������Ʈ")] public GameObject[] slot;

    public static BattleReady_UIManager instance;
    private void Awake()
    {
        instance = this;

        Set_ChapterNumName();
    }
    #endregion

    private int Max_Value;
    private Talk TextBox;

    [Header("============================\n\n[UI Canvas] ������Ʈ")]
    [SerializeField] private GameObject _Menu;
    [SerializeField] private GameObject _UnitFormation;
    [SerializeField] private GameObject _ChangeFormation;

    private enum STATE { TALK, MENU, UNIT_FORMATION, CHANGE_FORMATION }
    private STATE state = STATE.TALK;

    [SerializeField] private bool ischange_formationCamera;

    public event EventHandler OnCharacterChangeFormation;

    [SerializeField, Header("���� �� ���� ��")] private TMP_Text _Current; // "n"
    [SerializeField, Header("�� ������ ���� ��")] private TMP_Text _Max; // "/ n"

    [SerializeField, Header("[�� / ��ų Ȯ��] ������Ʈ")] private GameObject menuSelected;

    [Header("[ĳ���� ����] �ؽ�Ʈ")]
    [SerializeField] private TMP_Text character_Name;
    [SerializeField] private TMP_Text character_Class;
    [SerializeField] private TMP_Text character_Level;
    [SerializeField] private TMP_Text character_HP;
    [SerializeField] private TMP_Text character_AttackPower;
    [SerializeField] private TMP_Text character_ChainAttackPower;
    [SerializeField] private TMP_Text character_DefensePower;
    private CharacterDataManager data;
    private BattleReady_FormationState form_State;
    private Sprite no_Skill;
    [Header("ĳ���� ��ų �̹���")]
    [SerializeField] private Image[] character_Skill_Image;
    [Header("ĳ���� ��ų �̸�")]
    [SerializeField] private TMP_Text[] character_Skill_Name;
    [Header("ĳ���� �̹���")]
    [SerializeField] private Image character_Background;
    [SerializeField] private Image character_Image;
    private RectTransform rt;

    private string path = "Character/Skill/";
    private string skill_Content_1;
    private string skill_Content_2;
    private string skill_Content_3;

    [Header("��ų Ȯ�� ����")]
    [SerializeField] private Image skill_Image;
    [SerializeField] private TMP_Text skill_Name;
    [SerializeField] private TMP_Text skill_Content;

    [SerializeField, Header("[é�� ��] �ؽ�Ʈ")] public TMP_Text Txt_chapterNum;
    [SerializeField, Header("[é�͸�] �ؽ�Ʈ")] public TMP_Text Txt_chapterName;

    [SerializeField, Header("[��ų Ŀ��] ������Ʈ")] private GameObject Skill_Cursor;
    [SerializeField, Header("[��ų ����] ������Ʈ")] private GameObject Skill_Detail;

    private bool _sceneback;

    private void Start()
    {
        Max_Value = StageManager.instance.TotalUnlock;

        StartCoroutine(TalkStart());

        _sceneback = false;
    }

    private IEnumerator TalkStart()
    {
        TextBox = Instantiate(Resources.Load<GameObject>("TextBox")).GetComponent<Talk>();
        TextBox.Initialize(MapManager.Instance.stageNum);

        if (StageManager.instance.m_chapterNum == 0)
        {
            SoundManager.instance.Sound_TutorialBGM();
            yield return new WaitUntil(() => TextBox.IsEnd);
        }
        else if (StageManager.instance.m_chapterNum == 10)
        {
            SoundManager.instance.Sound_BossStageBGM();
            yield return new WaitUntil(() => TextBox.IsEnd);
        }
        else
        {
            SoundManager.instance.Sound_TalkBGM();
            yield return new WaitUntil(() => TextBox.IsEnd);
            SoundManager.instance.Sound_ForceStop();
        }

        if (StageManager.instance.m_chapterNum == 0) // Chapter. 00
        {
            ScenesSystem.Instance.ScenesChange();

            CharacterTypeManager.Instance.GetIsCharacter()[0] = true;
            CharacterTypeManager.Instance.GetIsCharacter()[1] = true;
            UnitManager.Instance.SpawnAllPlayer();

            GridSystemVisual.Instance.HideAllGridPosition();

            MapManager.Instance.stageNum = StageManager.instance.num = 1;
        }
        else if (StageManager.instance.m_chapterNum > 0 && StageManager.instance.m_chapterNum < 10) // Chapter. 01 ~ 09
        {
            SoundManager.instance.Sound_StageBGM();
            _Menu.SetActive(true);
            state = STATE.MENU;
        }
        else if (StageManager.instance.m_chapterNum == 10) // Chapter. 10
        {
            _Menu.SetActive(true);
            state = STATE.MENU;
        }

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
            case STATE.TALK:
                break;

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

    #region [���� ��]
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

    #region [��ġ ����]
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



    #region [���� ���� ���� �� / �� ������ ���� ��]
    private void Update_Formation()
    {
        _Current.text = BattleReady_UnitFormationCursor.count.ToString();
        _Max.text = "/ " + Max_Value; // �� ĳ���� ���� 8����.
    }
    #endregion

    #region [�� / ��ų Ȯ��]
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

    #region ���� / �� �Ϸ�
    public void UnlockCharacter(GameObject obj)
    {
        //ĳ���� ��� ���¿� ���� ĳ���� ��������Ʈ ��� ����
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
            //�� ���¿� ���� ĳ���� ��������Ʈ ��� ����
            ChangeColor(obj, 255f, 255f, 255f, 255f);

            //�� ��������Ʈ ���� �� ������ ����
            img.sprite = Resources.Load<Sprite>("team_forming");
            childRT.anchoredPosition = new Vector2(-59.5f, 19f);

            ChangeFormationSystem.Instance.AnyDestroyCharacterUI();
        }
        else //!obj.GetComponent<BattleReady_FormationState>().isFormationState
        {
            //�� ���¿� ���� ĳ���� ��������Ʈ ��� ����
            ChangeColor(obj, 48f, 48f, 48f, 255f);

            //�� ��������Ʈ ���� �� ������ ����
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

    #region ���� ��: ĳ���� ���� ����
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

    #region ���� ��: ĳ���� ��ų ����
    private void Set_NameAndImage()
    {
        character_Background.sprite = Resources.Load<Sprite>(data.m_back_resourcePath);
        character_Image.sprite = Resources.Load<Sprite>(data.m_resourcePath);

        switch (data.m_name)
        {
            #region No.1 ��ī��, SwordWoman
            case "��ī��":
                rt.anchoredPosition = new Vector2(480f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill01_Sww");
                character_Skill_Name[0].text = "�Ű�����";
                skill_Content_1 = "�˰� �ϳ��� �Ǿ� ġ������ ���� �ɷ��� ����Ų��.\n\n" +
                    "ġ��Ÿ Ȯ�� +<color=#ff7f00>15</color>%\n" +
                    "ġ��Ÿ ������ +<color=#ff7f00>20</color>%";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill02_Sww");
                character_Skill_Name[1].text = "�ϼ�";
                skill_Content_2 = "���� �ӵ��� ������ �� �Ѹ��� ĳ���� ���ݷ��� <color=#ff7f00>150</color>% ��ŭ�� �������� <color=#ff7f00>1</color>ȸ ����.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "SwordWoman/Skill03_Sww");
                character_Skill_Name[2].text = "����";
                skill_Content_3 = "������ ���� �˱⸦ ������ ���� ���� ���� ���� ĳ���� ���ݷ��� <color=#ff7f00>100</color>% ��ŭ�� �������� <color=#ff7f00>4</color>ȸ �����Ѵ�.";

                break;
            #endregion

            #region No.2 ũ����, Knight
            case "ũ����":
                rt.anchoredPosition = new Vector2(447f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Knight/Skill01_Kni");
                character_Skill_Name[0].text = "�ż��� ��";
                skill_Content_1 = "�ż��� ���� ���� �ڽ��� ���ݷ��� <color=#ff7f00>15</color>% ��ŭ ������Ų��.";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Knight/Skill02_Kni");
                character_Skill_Name[1].text = "�ż� ��Ÿ";
                skill_Content_2 = "�ż��� ���� ��� ������ ����� ��Ÿ�Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>100</color>% ��ŭ�� �������� <color=#ff7f00>1</color>ȸ �����ϰ� ����� <color=#ff7f00>2</color>�ϰ� ���� ���·� �����.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Knight/Skill03_Kni");
                character_Skill_Name[2].text = "Ȧ�� ����";
                skill_Content_3 = "�ż��� ���� ���վ� �ֺ� �Ʊ��� ���ݷ��� <color=#ff7f00>30</color>% ��ŭ ������Ų��.";

                break;
            #endregion

            #region No.3 ī�̳�, Samurai
            case "ī�̳�":
                rt.anchoredPosition = new Vector2(383f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Samurai/Skill01_Sam");
                character_Skill_Name[0].text = "�������";
                skill_Content_1 = "���� �ſ�� ����� ���� ���� ���������� ���� ���� ������ �ٴٶ���.\n\n" +
                    "ġ��Ÿ Ȯ�� +<color=#ff7f00>20</color>%\n" +
                    "ġ��Ÿ ������ +<color=#ff7f00>30</color>%\n" +
                    "��ų �ൿ�� �Ҹ� -<color=#ff7f00>1</color>";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Samurai/Skill02_Sam");
                character_Skill_Name[1].text = "������";
                skill_Content_2 = "������ ���� �ӵ��� ������ ����� ĳ���� ���ݷ��� <color=#ff7f00>150</color>% ��ŭ�� �������� <color=#ff7f00>3</color>ȸ �����Ѵ�.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Samurai/Skill03_Sam");
                character_Skill_Name[2].text = "�ݿ���";
                skill_Content_3 = "ȸ���ϸ� ���� ������ �ߵ��Ͽ� ������ ����� ĳ���� ���ݷ��� <color=#ff7f00>500</color>% ��ŭ�� �������� <color=#ff7f00>1</color>ȸ �����Ѵ�.";

                break;
            #endregion

            #region No.4 �Ḯ��, Archer
            case "�Ḯ��":
                rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Archer/Skill01_Arc");
                character_Skill_Name[0].text = "���� ����";
                skill_Content_1 = "����� ������ �����ؼ� �� �� ġ������ ������ ���� �� �ִ�.\n\n" +
                    "ġ��Ÿ Ȯ�� + <color=#ff7f00>30</color>%\n" +
                    "ġ��Ÿ ������ + <color=#ff7f00>40</color>%";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Archer/Skill02_Arc");
                character_Skill_Name[1].text = "�ַο� ��ο�";
                skill_Content_2 = "�� ���� ������ ���� ����\n" +
                    "ȭ���� �߻��Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>50</color>% ��ŭ�� �������� <color=#ff7f00>4</color>ȸ �����Ѵ�.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Archer/Skill03_Arc");
                character_Skill_Name[2].text = "��������";
                skill_Content_3 = "����� �޼Ҹ� ��Ȯ�� �븮�� ȭ���� �߻��Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>1000</color>% ��ŭ�� �������� <color=#ff7f00>1</color>ȸ �����Ѵ�.";

                break;
            #endregion

            #region No.5 �ö�ƾ, Guardian
            case "�ö�ƾ":
                rt.anchoredPosition = new Vector2(480f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Guardian/Skill01_Gur");
                character_Skill_Name[0].text = "������ ����";
                skill_Content_1 = "��ȣ ������ �Ͽ��� ������� ������ �ܴ��Ͽ� ���� ��Ʈ�� �� ����. �ڽ��� �޴� �������� <color=#ff7f00>15</color>% ���ҽ�Ų��.";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Guardian/Skill02_Gur");
                character_Skill_Name[1].text = "�Ŀ� �극��ũ";
                skill_Content_2 = "�������� ����� �ֵѷ� ĳ���� ���ݷ��� <color=#ff7f00>150</color>% ��ŭ�� �������� <color=#ff7f00>1</color>ȸ �����Ѵ�.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Guardian/Skill03_Gur");
                character_Skill_Name[2].text = "������ �Լ�";
                skill_Content_3 = "���ϰ� ��ȿ�Ͽ� �ֺ� ���� ��� ���� ���ݷ��� <color=#ff7f00>2</color>�ϰ� <color=#ff7f00>20</color>% ���ҽ�Ű�� <color=#ff7f00>2</color>�ϰ� �����Ѵ�.";

                break;
            #endregion

            #region No.6 ���̳�, Priest
            case "���̳�":
                rt.anchoredPosition = new Vector2(408f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Priest/Skill01_Pri");
                character_Skill_Name[0].text = "���� ����";
                skill_Content_1 = "���� ������ �޾� ȸ�� �迭 ��ų�� ȸ������ <color=#ff7f00>20</color>% �����Ѵ�.";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Priest/Skill02_Pri");
                character_Skill_Name[1].text = "����� ��";
                skill_Content_2 = "�Ʊ����� ����� ���� ��� ����� ü���� <color=#ff7f00>40</color>% ȸ����Ų��.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Priest/Skill03_Pri");
                character_Skill_Name[2].text = "��õ���� �ູ";
                skill_Content_3 = "��õ���� ������ ���� �� �Ʊ� ������ ü���� <color=#ff7f00>20</color>% ȸ����Ų��.";

                break;
            #endregion

            #region No.7 ���̺�, Wizard
            case "���̺�":
                rt.anchoredPosition = new Vector2(408f, -415f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Wizard/Skill01_Wiz");
                character_Skill_Name[0].text = "���� ����";
                skill_Content_1 = "������ �帧�� ���ֽ��� ���� Ȯ���� �ſ� ������ ������ �����Ѵ�.\n\n" +
                    "ġ��Ÿ Ȯ�� - <color=#ff7f00>12</color>%\n" +
                    "ġ��Ÿ ������ + <color=#ff7f00>300</color>%\n" +
                    "��� �ൿ�� �Ҹ� -<color=#ff7f00>1</color>";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Wizard/Skill02_Wiz");
                character_Skill_Name[1].text = "���� ��ô";
                skill_Content_2 = "�������� ������ ���� ���� �� ��� ���� ������ <color=#ff7f00>2</color>�ϰ� <color=#ff7f00>10</color>% ���ҽ�Ų��.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Wizard/Skill03_Wiz");
                character_Skill_Name[2].text = "���׿�";
                skill_Content_3 = "�Ŵ��� ���׿��� ��ȯ�Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>300</color>% ��ŭ�� �������� ���� �� ���� <color=#ff7f00>2</color>ȸ �����Ѵ�.";

                break;
            #endregion

            #region No.8 �ٳ׻�, Valkyrie
            case "�ٳ׻�":
                rt.anchoredPosition = new Vector2(408f, -336f);

                character_Skill_Image[0].sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill01_Val");
                character_Skill_Name[0].text = "�¸��� ����";
                skill_Content_1 = "�¸��� ���� ��Ű���� �� ���縸���ε� �Ʊ��� ��⸦ �����մϴ�.\n\n" +
                    "������ �ֺ� �Ʊ� ���ݷ� +<color=#ff7f00>5</color>%";

                character_Skill_Image[1].sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill02_Val");
                character_Skill_Name[1].text = "���";
                skill_Content_2 = "������ ���� ���ϰ� �� ĳ���� ���ݷ��� <color=#ff7f00>200</color>% ��ŭ�� �������� <color=#ff7f00>1</color>ȸ �����Ѵ�.";

                character_Skill_Image[2].sprite = Resources.Load<Sprite>(path + "Valkyrie/Skill03_Val");
                character_Skill_Name[2].text = "����";
                skill_Content_3 = "���� ���� ��� ������ ����Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>300</color>% ��ŭ�� �������� ���� �� ���� <color=#ff7f00>1</color>ȸ �����ϰ� ����� <color=#ff7f00>2</color>�ϰ� ���� ���·� �����.";

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

    #region ���� ��: ��ų Ȯ��
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

    #region é�͸�
    private void Set_ChapterNumName()
    {
        Txt_chapterNum.text = "�� " + StageManager.instance.m_chapterNum.ToString() + "��";
        Txt_chapterName.text = StageManager.instance.m_chapterName.ToString();
    }
    #endregion

    public bool GetChange_FormationCamera()
    {
        return ischange_formationCamera;
    }

    public bool GetSceneback()
    {
        return _sceneback;
    }
    public void SetSceneback(bool _sceneback)
    {
        this._sceneback = _sceneback;
    }
}