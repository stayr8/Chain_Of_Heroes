using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class SpeechBubble
{
	public int ID;
	public int MapID;
	public string Speaker;
	public string StringKR;
	public float TalkSpeed;
	public float TextSpeed;
	public string ResourcePath;
	public string BResourcePath;

	public bool Parse(SimpleJSON.JSONNode Data)
	{
		ID = Data["ID"].AsInt;
		MapID = Data["MapID"].AsInt;
		Speaker = Data["Speaker"];
		StringKR = Data["StringKR"];
		TalkSpeed = Data["TalkSpeed"].AsFloat;
		TextSpeed = Data["TextSpeed"].AsFloat;
		ResourcePath = Data["ResourcePath"];
		BResourcePath = Data["BResourcePath"];
		return true;
	}
}
