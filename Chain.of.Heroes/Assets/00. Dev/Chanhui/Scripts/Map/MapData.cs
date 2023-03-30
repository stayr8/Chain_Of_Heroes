using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Map Data", menuName = "Scriptable Object/Map Data")]
public class MapData : ScriptableObject
{
    [Header("Map_Information")]
    [SerializeField] private string Map_Name;
    [SerializeField] private int map_stage_number;
    public int Map_Stage_Number { get { return map_stage_number; } }
    [SerializeField] private bool Map_Clear_Confirm;


    //[SerializeField] private int monster_num;
    //public int MONSTER_NUM { get { return monster_num; } }
    //[SerializeField] private int player_num;
    //public int PLAYER_NUM { get { return player_num; } }

    [Header("Monster_Information")]
    [SerializeField] private GameObject[] enemy_pf;
    public GameObject[] Enemy_pf { get { return enemy_pf; } }

    [Header("Player_Information")]
    [SerializeField] private GameObject[] player_pf;
    public GameObject[] Player_pf { get { return player_pf; } }

    //[SerializeField] private MonsterType[] type;
    //public MonsterType[] Type { get { return type; } }

    [Header("Position")]
    [SerializeField] private Vector2[] currentXY;
    public Vector2[] CurrentXY { get { return currentXY; } }

}
