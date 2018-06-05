using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BoardState{
	Wall,
	Floor
}

public class BoardController : MonoBehaviour {
	public static int width,height,expand=7;
	public static int level=1;
	public static BoardState[,] board;
	public GameObject[] wallPrefabs;
	public GameObject floor;
	public GameObject boardController;
	public GameObject roomControl;
	public static GameObject roomController;
	public GameObject exit;
	public GameObject player;
	public Camera minimapCamera;


	void Awake(){
		width = Rooms.Width + (5 * level);
		height = Rooms.Height + (3 * level);
		roomController = roomControl;



		Rooms.ClearData ();
		Rooms.wallSize = wallPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
		board = new BoardState[width+(expand*2), height+(expand*2)];

		minimapCamera.transform.position = new Vector3 (((float)width + 5.0f) / 2 * Rooms.wallSize, ((float)height + 5.0f) / 2 * Rooms.wallSize, -10.0f);

		Leaf main = new Leaf (expand, expand, width+expand, height+expand);
		GameObject prefabInstance;
		for (int i = 0; i < board.GetLength(0); i++) {
			for (int j = 0; j < board.GetLength(1); j++) {
				if (board [i, j] == BoardState.Wall) {
					if (i < 3 || j < 3 || i > board.GetLength (0)-3 || j > board.GetLength (1)-3) {
						prefabInstance = (GameObject)Instantiate (wallPrefabs[0], boardController.transform);
					} else {
						prefabInstance = (GameObject)Instantiate (wallPrefabs [getNeighbors (i, j)], boardController.transform);
					}
				} else {
					prefabInstance = (GameObject)Instantiate (floor, boardController.transform);

				}
				prefabInstance.transform.position = new Vector3 (i*Rooms.wallSize, j*Rooms.wallSize, 0f);

			}
		}
		Vector3[] spawnPointsArray=Rooms.validSpawnPoints.ToArray();
		Vector3 spawn = spawnPointsArray [Random.Range (0, spawnPointsArray.Length-1)];
		Rooms.validSpawnPoints.Remove (spawn);
		Instantiate (exit, spawn, Quaternion.identity);
	}


	
	int getNeighbors(int i, int j){
		int neighbors = 0;
		if (board [i - 1, j] == BoardState.Floor)
			neighbors += 1;
		if (board [i, j+1] == BoardState.Floor)
			neighbors += 2;
		if (board [i+1, j] == BoardState.Floor)
			neighbors += 4;
		if (board [i, j-1] == BoardState.Floor)
			neighbors += 8;
		return neighbors;
	}
}
