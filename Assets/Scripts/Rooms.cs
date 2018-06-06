using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rooms{
	public static int minSize = 10;
	public static int Width = 27, Height = 27;
	public static float wallSize;
	public static bool complete=false,lose=false;
	public static string DeathText;

	public static void ClearData(){
		roomData.Clear ();
	}
	public class RoomData{
		public Vector3 spawnPoint = new Vector3();
		public float diameter;

		public RoomData(Vector3 sp, float d){
			this.spawnPoint=sp;
			this.diameter=d;
		}

	}
	public static List<RoomData> roomData = new List<RoomData>();

}
