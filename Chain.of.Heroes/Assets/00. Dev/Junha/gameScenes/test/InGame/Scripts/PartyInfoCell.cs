using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyInfoCell : MonoBehaviour
{
    public Selectable Selectable { get; private set; }

    [SerializeField] private Image Img;
    [SerializeField] private TMP_Text Txt_Name;
    [SerializeField] private TMP_Text Txt_Class;
    [SerializeField] private TMP_Text Txt_Type;
    [SerializeField] private TMP_Text Txt_Level;
    [SerializeField] private TMP_Text Txt_AP;
    [SerializeField] private TMP_Text Txt_CAP;
    [SerializeField] private TMP_Text Txt_DP;


    void Awake()
    {
        Selectable = GetComponent<Selectable>();
    }

    public void UpdateText(Unit unit)
    {
        if (unit == null) { return; }

        Txt_Name.text = unit.GetCharacterDataManager().m_name;
        Txt_Class.text = unit.GetCharacterDataManager().m_class;
        //Txt_Type.text = unit.GetCharacterDataManager().m_type;
        Txt_Level.text = unit.GetCharacterDataManager().m_level.ToString();
        Txt_AP.text = unit.GetCharacterDataManager().m_attackPower.ToString();
        Txt_CAP.text = unit.GetCharacterDataManager().m_chainAttackPower.ToString();
        Txt_DP.text = unit.GetCharacterDataManager().m_defensePower.ToString();
    }
}
