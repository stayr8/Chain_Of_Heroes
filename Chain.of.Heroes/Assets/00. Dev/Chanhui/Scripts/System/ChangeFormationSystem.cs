using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFormationSystem : MonoBehaviour
{
    public static ChangeFormationSystem Instance { get; private set; }

    private const int MAX_CHARACTER_NUMBER = 10;

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

    [SerializeField] private CharacterType[] characterType;

    private Vector2[] CharacterTr;

    [SerializeField] private int width;
    [SerializeField] private int height;


    private void Awake()
    {
        CharacterTr = new Vector2[MAX_CHARACTER_NUMBER];
    }

    private void Start()
    {
        for(int x = 0; x < 10; x++)
        {
            Vector2 gridpos = new Vector2(0, 0);
            CharacterTr[(int)CharacterType.Empty + x] = gridpos;
            Debug.Log(CharacterTr[x]);
        }
    }

    public void SetSeletedCharacterPos(int x, int y)
    {
        
    }

    public CharacterType GetCharacterTypeCheck(int number)
    {
        return characterType[number];
    }
}
