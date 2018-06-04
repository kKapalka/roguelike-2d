using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rooms{
	public static int currentSplits=0,maxSplits = 45;
	public static int minSize = 20;
	public static int rooms=0;
	public static float wallSize;
	public static List<Vector3> validSpawnPoints = new List<Vector3>();
	public static List<GameObject> roomControllers=new List<GameObject>();
	public static void ClearData(){
		currentSplits = 0;
		validSpawnPoints.Clear ();
		roomControllers.Clear ();
	}



}
