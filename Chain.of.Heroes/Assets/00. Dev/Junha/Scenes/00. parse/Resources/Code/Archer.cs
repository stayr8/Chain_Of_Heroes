using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class Archer
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
	public string UnitProperty;
	public string ResourcePath;
	public string BResourcePath;
	public int UnlockMapID;

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
		UnitProperty = Data["UnitProperty"];
		ResourcePath = Data["ResourcePath"];
		BResourcePath = Data["BResourcePath"];
		UnlockMapID = Data["UnlockMapID"].AsInt;
		return true;
	}
}
