using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class GoblinWarrior
{
	public int ID;
	public string Monster_Name;
	public int Monster_Level;
	public float AttackPower;
	public float Monster_DefensePower;
	public float Monster_Hp;
	public float CriticalRate;
	public float CriticalDamage;
	public int Monster_ActionPoint;
	public float Monster_Speed;
	public int Monster_MovementRange;
	public int Monster_AttackRange;
	public int Monster_Rank;
	public int UniyProperty;

	public bool Parse(SimpleJSON.JSONNode Data)
	{
		ID = Data["ID"].AsInt;
		Monster_Name = Data["Monster_Name"];
		Monster_Level = Data["Monster_Level"].AsInt;
		AttackPower = Data["AttackPower"].AsFloat;
		Monster_DefensePower = Data["Monster_DefensePower"].AsFloat;
		Monster_Hp = Data["Monster_Hp"].AsFloat;
		CriticalRate = Data["CriticalRate"].AsFloat;
		CriticalDamage = Data["CriticalDamage"].AsFloat;
		Monster_ActionPoint = Data["Monster_ActionPoint"].AsInt;
		Monster_Speed = Data["Monster_Speed"].AsFloat;
		Monster_MovementRange = Data["Monster_MovementRange"].AsInt;
		Monster_AttackRange = Data["Monster_AttackRange"].AsInt;
		Monster_Rank = Data["Monster_Rank"].AsInt;
		UniyProperty = Data["UniyProperty"].AsInt;
		return true;
	}
}
