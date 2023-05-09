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

    [SerializeField, Header("[�޴�] ������Ʈ")] private GameObject UI_Menu;
    [SerializeField, Header("[���� ��] ������Ʈ")] private GameObject UI_UnitFormation;
    [SerializeField, Header("[��ġ ����] ������Ʈ")] private GameObject UI_ChangeFormation;

    #region instanceȭ :: Awake()�Լ� ����
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

    #region �� / ��ų Ȯ��
    [SerializeField, Header("[�� / ��ų Ȯ��] ������Ʈ")] private GameObject menuSelected;
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
    [SerializeField, Header("[����] ��������Ʈ")] private Sprite Img_OffFormation;
    public void OffFormation(GameObject obj)
    {
        // ��������Ʈ ����
        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OffFormation;

        // ��������Ʈ ���濡 ���� ������ ����
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-41.5f, 18.5f);
        childRT.sizeDelta = new Vector2(83f, 37f);
    }
    [SerializeField, Header("[�� �Ϸ�] ��������Ʈ")] private Sprite Img_OnFormation;
    public void OnFormation(GameObject obj)
    {
        // ��������Ʈ ����
        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();
        img.sprite = Img_OnFormation;

        // ��������Ʈ ���濡 ���� ������ ����
        RectTransform childRT = child.GetComponent<RectTransform>();
        childRT.anchoredPosition = new Vector2(-59.5f, 19f);
        childRT.sizeDelta = new Vector2(119f, 38f);
    }
    #endregion

    #region ���� ��: ĳ���� ���� ����
    [Header("[���� �� ��] �ؽ�Ʈ")]
    [SerializeField] private TextMeshProUGUI _Current; // "n"
    [SerializeField] private TextMeshProUGUI _Max; // "/ n"
    private void Update_Formation()
    {
        _Current.text = BattleReady_UnitFormationCursor.count.ToString();
        _Max.text = "/ " + "9"; // �� ĳ���� ���� 9����.
    }

    [Header("[ĳ���� ����] �ؽ�Ʈ")]
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
                character_Name.text = "��ī��";
                rt.anchoredPosition = new Vector2(480f, -415f);

                character_Skill_1.text = "�Ű�����";
                skill_Content_1 = "�˰� �ϳ��� �Ǿ� ġ������\n" +
                    "���� �ɷ��� ����Ų��.\n\n" +
                    "ġ��Ÿ Ȯ�� +<color=#ff7f00> 15</color>%\r\n" +
                    "ġ��Ÿ ������ +<color=#ff7f00> 20</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "�ϼ�";
                skill_Content_2 = "���� �ӵ��� ������ �� �Ѹ��� ĳ���� ���ݷ��� <color=#ff7f00>150</color>% ��ŭ�� �������� 1ȸ ����.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "����";
                skill_Content_3 = "������ ���� �˱⸦ ������ ���� ���� ���� ���� ĳ���� ���ݷ��� <color=#ff7f00>100</color>% ��ŭ��\n" +
                                  "�������� 4ȸ �����Ѵ�.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Kris": // _2
                character_Name.text = "ũ����";
                rt.anchoredPosition = new Vector2(447f, -415f);

                character_Skill_1.text = "�ż��� ��";
                skill_Content_1 = "�ż��� ���� ����\n" +
                    "�ڽ��� ���ݷ��� <color=#ff7f00>15</color>% ��ŭ ������Ų��.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "�ż� ��Ÿ";
                skill_Content_2 = "�ż��� ���� ��� ������\n" +
                    "����� ��Ÿ�Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>100</color>% ��ŭ�� �������� 1ȸ �����ϰ� ����� 2�ϰ� ���� ���·� �����.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "Ȧ�� ����";
                skill_Content_3 = "�ż��� ���� ���վ� �ֺ� �Ʊ��� ���ݷ��� <color=#ff7f00>30</color>% ��ŭ ������Ų��.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Teo": // _3
                character_Name.text = "�¿�";
                rt.anchoredPosition = new Vector2(383f, -415f);

                character_Skill_1.text = "�������";
                skill_Content_1 = "���� �ſ�� ����� ����\n" +
                    "���� ���������� ���� ���� ������ �ٴٶ���.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "������";
                skill_Content_2 = "������ ���� �ӵ��� ������ ����� ĳ���� ���ݷ��� <color=#ff7f00>150</color>% ��ŭ�� �������� 3ȸ �����Ѵ�.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "�ݿ���";
                skill_Content_3 = "ȸ���ϸ� ���� ������ �ߵ��Ͽ�\n" +
                    "������ ����� ĳ���� ���ݷ��� <color=#ff7f00>500</color>% ��ŭ�� �������� 1ȸ �����Ѵ�.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Melia": // _4
                character_Name.text = "�Ḯ��";
                rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "���� ����";
                skill_Content_1 = "����� ������ �����ؼ� �� �� ġ������ ������ ���� �� �ִ�.\n\n" +
                    "ġ��Ÿ Ȯ�� + <color=#ff7f00>30</color>%\n" +
                    "ġ��Ÿ ������ + <color=#ff7f00>40</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "�ַο� ��ο�";
                skill_Content_2 = "�� ���� ������ ���� ����\n" +
                    "ȭ���� �߻��Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>50</color>% ��ŭ�� �������� 4ȸ �����Ѵ�.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "��������";
                skill_Content_3 = "����� �޼Ҹ� ��Ȯ�� �븮�� ȭ���� �߻��Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>1000</color>% ��ŭ�� �������� 1ȸ �����Ѵ�.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Platin": // _5
                character_Name.text = "�ö�ƾ";
                //rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "������ ����";
                skill_Content_1 = "��ȣ ������ �Ͽ��� ������� ������ �ܴ��Ͽ� ���� ��Ʈ�� �� ����. �ڽ��� �޴� �������� <color=#ff7f00>15</color>% ���ҽ�Ų��.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "�Ŀ� �극��ũ";
                skill_Content_2 = "�������� ����� �ֵѷ� ĳ���� ���ݷ��� <color=#ff7f00>150</color>% ��ŭ�� �������� 1ȸ �����Ѵ�.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "������ �Լ�";
                skill_Content_3 = "���ϰ� ��ȿ�Ͽ� �ֺ� ���� ��� ���� ���ݷ��� 2�ϰ� <color=#ff7f00>20</color>% ���ҽ�Ű�� 2�ϰ� �����Ѵ�.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Raiden": // _6
                character_Name.text = "���̵�";
                //rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "�̱��˼�";
                skill_Content_1 = "�Ű��� ������ �̸��� 7�ڷ��� ���� ��������� �ٷ��.\n\n" +
                    "ġ��Ÿ Ȯ�� + <color=#ff7f00>10</color>%\n" +
                    "ġ��Ÿ ������ + <color=#ff7f00>10</color>%\n" +
                    "ĳ���� ���ݷ� + <color=#ff7f00>10</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "��� ����";
                skill_Content_2 = "2�ڷ��� ���� �����Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>125</color>% ��ŭ�� �������� 2ȸ �����Ѵ�.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "����";
                skill_Content_3 = "7�ڷ��� ���� ���� ĳ���� ���ݷ��� <color=#ff7f00>400</color>% ��ŭ�� �������� 1ȸ �����Ѵ�.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Eileene": // _7
                character_Name.text = "���ϸ�";
                rt.anchoredPosition = new Vector2(408f, -415f);

                character_Skill_1.text = "���� ����";
                skill_Content_1 = "���� ������ �޾� ȸ�� �迭 ��ų�� ȸ������ <color=#ff7f00>20</color>% �����Ѵ�.";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "����� ��";
                skill_Content_2 = "�Ʊ����� ����� ���� ��� ����� ü���� <color=#ff7f00>40</color>% ȸ����Ų��.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "��õ���� �ູ";
                skill_Content_3 = "��õ���� ������ ���� �� �Ʊ� ������ ü���� <color=#ff7f00>20</color>% ȸ����Ų��.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Jave": // _8
                character_Name.text = "���̺�";
                //rt.anchoredPosition = new Vector2(433f, -415f);

                character_Skill_1.text = "���� ����";
                skill_Content_1 = "������ �帧�� ���ֽ��� ���� Ȯ���� �ſ� ������ ������ �����Ѵ�.\n\n" +
                    "ġ��Ÿ Ȯ�� - <color=#ff7f00>12</color>%\n" +
                    "ġ��Ÿ ������ + <color=#ff7f00>300</color>%\n" +
                    "��� �ൿ�� �Ҹ� -<color=#ff7f00>1</color>";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "���� ��ô";
                skill_Content_2 = "�������� ������ ���� ���� �� ��� ���� ������ 2�ϰ� <color=#ff7f00>10</color>% ���ҽ�Ų��.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "���׿�";
                skill_Content_3 = "�Ŵ��� ���׿��� ��ȯ�Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>300</color>% ��ŭ�� �������� ���� �� ���� 2ȸ �����Ѵ�.";
                skill_Image_3 = Resources.Load<Sprite>("");

                break;

            case "Vanessa": // _9
                character_Name.text = "�ٳ׻�";
                rt.anchoredPosition = new Vector2(408f, -336f);

                character_Skill_1.text = "�¸��� ����";
                skill_Content_1 = "�¸��� ���� ��Ű���� �� ���縸���ε� �Ʊ��� ��⸦ �����մϴ�.\n\n" +
                    "������ �ֺ� �Ʊ� ���ݷ� + <color=#ff7f00>5</color>%";
                skill_Image_1 = Resources.Load<Sprite>("");

                character_Skill_2.text = "���";
                skill_Content_2 = "������ ���� ���ϰ� �� ĳ���� ���ݷ��� <color=#ff7f00>200</color>% ��ŭ�� �������� 1ȸ �����Ѵ�.";
                skill_Image_2 = Resources.Load<Sprite>("");

                character_Skill_3.text = "����";
                skill_Content_3 = "���� ���� ��� ������ ����Ͽ� ĳ���� ���ݷ��� <color=#ff7f00>300</color>% ��ŭ�� �������� ���� �� ���� 1ȸ �����ϰ� ����� 2�ϰ� ���� ���·� �����.";
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

    #region ���� ��: ��ų Ȯ��
    [SerializeField, Header("[��ų Ŀ��] ������Ʈ")] private GameObject Skill_Cursor;
    public void OnSkillCursor()
    {
        Skill_Cursor.SetActive(true);
    }
    public void OffSkillCursor()
    {
        Skill_Cursor.SetActive(false);
    }

    [SerializeField, Header("[��ų ����] ������Ʈ")] private GameObject Skill_Detail;
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