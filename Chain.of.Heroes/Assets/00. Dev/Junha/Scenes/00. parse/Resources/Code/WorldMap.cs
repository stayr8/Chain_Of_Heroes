using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;


public class WorldMap
{
	public int ID;
	public int WorldMapChNumber;
	public string ChapterName;
	public string ChapterInfoResourcePath;

	public bool Parse(SimpleJSON.JSONNode Data)
	{
		ID = Data["ID"].AsInt;
		WorldMapChNumber = Data["WorldMapChNumber"].AsInt;
		ChapterName = Data["ChapterName"];
		ChapterInfoResourcePath = Data["ChapterInfoResourcePath"];
		return true;
	}
}
