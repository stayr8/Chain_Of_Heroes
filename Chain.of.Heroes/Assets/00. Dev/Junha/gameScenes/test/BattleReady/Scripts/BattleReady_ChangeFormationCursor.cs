using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleReady_ChangeFormationCursor : CursorBase
{

    private RectTransform rt;
    private GameObject BeforeCurrentSelected;
    [SerializeField] private GameObject AfterCurrentSelect;

    [SerializeField] private bool _seletedImageMove;
    [SerializeField] private CharacterUI _characterUi;
    [SerializeField] private CharacterUI _characterUiChange;
    private bool _seleted;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        _seleted = false;
    }

    private const float INIT_X = 476f;
    private const float INIT_Y = -50f;
    private void OnEnable()
    {
        Init(rt, INIT_X, INIT_Y, ref BeforeCurrentSelected, "_04");
    }

    private const float MOVE_DISTANCE_X = 125f; private const float MOVE_DISTANCE_Y = 125f;
    private const float MAX_POSITION_X = 851f; private const float MAX_POSITION_Y = -50;
    private const float MIN_POSITION_X = 476f; private const float MIN_POSITION_Y = -300f;
    private void Update()
    {
        Movement(rt, ref BeforeCurrentSelected, MOVE_DISTANCE_X, MOVE_DISTANCE_Y, MIN_POSITION_X, MAX_POSITION_X, MIN_POSITION_Y, MAX_POSITION_Y);

        Update_Data();
        MenuFunction();
        
    }

    private void MenuFunction()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(_seleted)
            {
                _seleted = false;
                cursorSeleted();
            }
            else
            {
                _seleted = true;
                cursorSeleted();
            }

            switch (BeforeCurrentSelected.name)
            {
                case "_04":
                    Debug.Log("x: 0, y: 4 슬롯 선택");
                    ChangePosAtSeleted(8);

                    break;
                case "_24":
                    Debug.Log("x: 2, y: 4 슬롯 선택");
                    ChangePosAtSeleted(9);
                    break;
                case "_44":
                    Debug.Log("x: 4, y: 4 슬롯 선택");
                    ChangePosAtSeleted(10);
                    break;
                case "_64":
                    Debug.Log("x: 6, y: 4 슬롯 선택");
                    ChangePosAtSeleted(11);
                    break;

                case "_02":
                    Debug.Log("x: 0, y: 2 슬롯 선택");
                    ChangePosAtSeleted(4);
                    break;
                case "_22":
                    Debug.Log("x: 2, y: 2 슬롯 선택");
                    ChangePosAtSeleted(5);
                    break;
                case "_42":
                    Debug.Log("x: 4, y: 2 슬롯 선택");
                    ChangePosAtSeleted(6);
                    break;
                case "_62":
                    Debug.Log("x: 6, y: 2 슬롯 선택");
                    ChangePosAtSeleted(7);
                    break;

                case "_00":
                    Debug.Log("x: 0, y: 0 슬롯 선택");
                    ChangePosAtSeleted(0);
                    break;
                case "_20":
                    Debug.Log("x: 2, y: 0 슬롯 선택");
                    ChangePosAtSeleted(1);
                    break;
                case "_40":
                    Debug.Log("x: 4, y: 0 슬롯 선택");
                    ChangePosAtSeleted(2);
                    break;
                case "_60":
                    Debug.Log("x: 6, y: 0 슬롯 선택");
                    ChangePosAtSeleted(3);
                    break;
            }
        }
    }

    private void ChangePosAtSeleted(int pos)
    {
        if(!_seleted)
        {
            if(pos == _characterUi.GetCharUIpos())
            {
                return;
            }

            // 이동하려는 자리에 UI가 있으면
            if (_seletedImageMove && ChangeFormationSystem.Instance.GetIsGround()[pos])
            {
                // 원래 자리 UI 먼저 삭제
                _characterUiChange = BeforeCurrentSelected.GetComponentInChildren<CharacterUI>();
                ChangeFormationSystem.Instance.SingleDestroyCharacterUI(_characterUiChange, (int)_characterUiChange.ImageType(), _characterUiChange.GetCharUIpos());
                // 이동하려는 UI 삭제
                ChangeFormationSystem.Instance.SingleDestroyCharacterUI(_characterUi, (int)_characterUi.ImageType(), _characterUi.GetCharUIpos());
                // 서로의 UI들 생성
                ChangeFormationSystem.Instance.CreateCharacterUI((int)_characterUi.ImageType(), pos);
                ChangeFormationSystem.Instance.CreateCharacterUI((int)_characterUiChange.ImageType(), _characterUi.GetCharUIpos());
                _seletedImageMove = false;
            }
            else if(_seletedImageMove)
            {
                Debug.Log((int)_characterUi.ImageType());
                ChangeFormationSystem.Instance.SingleDestroyCharacterUI(_characterUi, (int)_characterUi.ImageType(), _characterUi.GetCharUIpos());
                ChangeFormationSystem.Instance.CreateCharacterUI((int)_characterUi.ImageType(), pos);
                _seletedImageMove = false;
            }
        }
        else
        {
            if(!_seletedImageMove && ChangeFormationSystem.Instance.GetIsGround()[pos])
            {
                //Update_Data();
                _seletedImageMove = true;
                _characterUi = BeforeCurrentSelected.GetComponentInChildren<CharacterUI>();
            }
            else
            {
                return;
            }
        }
    }

    private void cursorSeleted()
    {
        if (_seleted)
        {
            AfterCurrentSelect.SetActive(_seleted);
        }
        else
        {
            AfterCurrentSelect.SetActive(_seleted);
        }
    }

    [Header("[캐릭터 정보] 텍스트")]
    [SerializeField] private TextMeshProUGUI character_Name;
    [SerializeField] private TextMeshProUGUI character_Class;
    [SerializeField] private TextMeshProUGUI character_HP;
    [SerializeField] private TextMeshProUGUI character_MaxHP;
    [SerializeField] private TextMeshProUGUI character_AttackPower;
    [SerializeField] private TextMeshProUGUI character_ChainAttackPower;
    [SerializeField] private TextMeshProUGUI character_DefensePower;
    private CharacterDataManager data;
    private void Update_Data()
    {
        GameObject obj = BeforeCurrentSelected;
        if (obj.GetComponentInChildren<CharacterDataManager>() == null)
        {
            return;
        }
        data = obj.GetComponentInChildren<CharacterDataManager>();

        Set_NameAndImage();
        character_HP.text = data.m_hp.ToString();
        character_MaxHP.text = data.m_hp.ToString();
        character_AttackPower.text = data.m_attackPower.ToString();
        character_ChainAttackPower.text = data.m_chainAttackPower.ToString();
        character_DefensePower.text = data.m_defensePower.ToString();
    }

    private void Set_NameAndImage()
    {
        switch (data.m_name)
        {
            case "Akame": // _1
                character_Name.text = "아카메";
                
                break;

            case "Kris": // _2
                character_Name.text = "크리스";
                
                break;

            case "Teo": // _3
                character_Name.text = "태오";
                
                break;

            case "Melia": // _4
                character_Name.text = "멜리아";
                
                break;

            case "Platin": // _5
                character_Name.text = "플라틴";
                
                break;

            case "Raiden": // _6
                character_Name.text = "라이덴";
                
                break;

            case "Eileene": // _7
                character_Name.text = "아일린";
                
                break;

            case "Jave": // _8
                character_Name.text = "제이브";
                

                break;

            case "Vanessa": // _9
                character_Name.text = "바네사";
                
                break;
        }
    }
}