using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTypeManager : MonoBehaviour
{
    public static CharacterTypeManager Instance { get; private set; }

    // ItemType enum Á¤ÀÇ
    public enum CharacterType
    {
        Empty,
        Womanknight,
        Night,
        samurai,
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

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            bool uncharacter = false;
            isCharacter[i] = uncharacter;
        }
    }


    public bool GetIsCharacter(int number)
    {
        return isCharacter[number];
    }


}
