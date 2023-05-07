using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTypeManager : MonoBehaviour
{
    public static CharacterTypeManager Instance { get; private set; }

    // CharacterType enum ����
    public enum CharacterType
    {
        Womanknight,
        Knight,
        Samurai,
        Archer,
        Guardian,
        Manknight,
        Priest,
        Wizard,
        Valkyrie,
    }

    [SerializeField] private bool[] isCharacter;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public bool[] GetIsCharacter()
    {
        return isCharacter;
    }
    public void SetIsCharacter(int index, bool isCharacter)
    {
        this.isCharacter[index] = isCharacter;
    }

    public void SetIsCharacter(int number)
    {
        this.isCharacter[number] = isCharacter[number];
    }


}
