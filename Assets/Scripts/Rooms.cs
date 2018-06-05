using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rooms{
	public static int minSize = 10;
	public static int rooms=0;
	public static int Width = 37, Height = 37;
	public static float wallSize;
	public static List<Vector3> validSpawnPoints = new List<Vector3>();
	public static List<GameObject> roomControllers=new List<GameObject>();
	public static void ClearData(){
		validSpawnPoints.Clear ();
		roomControllers.Clear ();
	}



}
