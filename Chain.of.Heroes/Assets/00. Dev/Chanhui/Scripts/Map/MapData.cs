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


    [Header("Monster_Information")]
    [SerializeField] private GameObject[] enemy_pf;
    public GameObject[] Enemy_pf { get { return enemy_pf; } }

    [SerializeField] private Vector2[] enemyXY;
    public Vector2[] EnemyXY { get { return enemyXY; } }

    [Header("Player_Information")]
    [SerializeField] private GameObject[] player_pf;
    public GameObject[] Player_pf { get { return player_pf; } }

    [SerializeField] private Vector2[] playerXY;
    public Vector2[] PlayerXY { get { return playerXY; } }

}
