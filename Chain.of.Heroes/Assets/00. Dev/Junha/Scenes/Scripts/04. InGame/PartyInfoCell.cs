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
    [SerializeField] private TMP_Text Txt_ClassType;
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

        var CharData = unit.GetCharacterDataManager();

        Img.sprite = Resources.Load<Sprite>(CharData.m_resourcePath);
        Txt_Name.text = CharData.m_name;
        Txt_ClassType.text = CharData.m_class;
        Txt_Level.text = CharData.m_level.ToString();
        Txt_AP.text = Mathf.RoundToInt(CharData.m_attackPower).ToString();
        Txt_CAP.text = CharData.m_chainAttackPower.ToString();
        Txt_DP.text = CharData.m_defensePower.ToString();
    }
}