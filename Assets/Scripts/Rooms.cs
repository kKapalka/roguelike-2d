using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rooms{
	public static int minSize = 8;
	public static int Width = 27, Height = 27;
	public static float wallSize;
	public static bool complete=false,lose=false;
	public static string DeathText;

	public static void ClearData(){
		roomData.Clear ();
	}
	
	public static List<RoomData> roomData = new List<RoomData>();

}
