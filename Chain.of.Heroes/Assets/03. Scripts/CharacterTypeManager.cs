using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTypeManager : MonoBehaviour
{
    public static CharacterTypeManager Instance { get; private set; }

    // CharacterType enum Á¤ÀÇ
    public enum CharacterType
    {
        SwordWoman,
        Knight,
        Samurai,
        Archer,
        Guardian,
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

}
