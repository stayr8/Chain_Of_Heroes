using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Map Data", menuName = "Scriptable Object/Map Data")]
public class MapData : ScriptableObject
{
    [Header("Map_Information")]
    [SerializeField] private string Map_Name;

    [SerializeField] private int player_actionpoint;
    public int Player_ActionPoint { get { return player_actionpoint; } }
    [SerializeField] private int enemy_actionpoint;
    public int Enemy_ActionPoint { get { return enemy_actionpoint; } }

    [SerializeField] private int clear_exp;
    public int Clear_Exp { get { return clear_exp; } }

    [SerializeField] private int stage_monsterLV;
    public int Stage_MonsterLV { get { return stage_monsterLV; } }



    [Header("Monster_Information")]
    [SerializeField] private GameObject[] enemy_pf;
    public GameObject[] Enemy_pf { get { return enemy_pf; } }

    [SerializeField] private Vector3[] enemyXY;
    public Vector3[] EnemyXY { get { return enemyXY; } }
}
