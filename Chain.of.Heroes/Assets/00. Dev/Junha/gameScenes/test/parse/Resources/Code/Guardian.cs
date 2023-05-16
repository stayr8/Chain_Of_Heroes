using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class Guardian
{
	public int ID;
	public string Name;
	public int Level;
	public int MaxLevel;
	public float CurrentExp;
	public float MaxExp;
	public float AttackPower;
	public float ChainAttackPower;
	public float DefensePower;
	public float Hp;
	public float CriticalRate;
	public float CriticalDamage;
	public string Class;
	public string UniyProperty;
	public string ResourcePath;

	public bool Parse(SimpleJSON.JSONNode Data)
	{
		ID = Data["ID"].AsInt;
		Name = Data["Name"];
		Level = Data["Level"].AsInt;
		MaxLevel = Data["MaxLevel"].AsInt;
		CurrentExp = Data["CurrentExp"].AsFloat;
		MaxExp = Data["MaxExp"].AsFloat;
		AttackPower = Data["AttackPower"].AsFloat;
		ChainAttackPower = Data["ChainAttackPower"].AsFloat;
		DefensePower = Data["DefensePower"].AsFloat;
		Hp = Data["Hp"].AsFloat;
		CriticalRate = Data["CriticalRate"].AsFloat;
		CriticalDamage = Data["CriticalDamage"].AsFloat;
		Class = Data["Class"];
		UniyProperty = Data["UniyProperty"];
		ResourcePath = Data["ResourcePath"];
		return true;
	}
}
