using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BoardState{
	Wall,
	Floor
}

public class BoardController : MonoBehaviour {
	public static int width=65,height=65;
	public static BoardState[,] board;
	public GameObject[] wallPrefabs;
	public GameObject floor;
	public GameObject boardController;
	public GameObject roomControl;
	public static GameObject roomController;
	public GameObject exit;
	public GameObject player;
	public Camera minimapCamera;
	Leaf main;

	void Awake(){
		int expand = 7;
		roomController = roomControl;



		Rooms.ClearData ();
		Rooms.wallSize = wallPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
		board = new BoardState[width+(expand*2), height+(expand*2)];

		minimapCamera.transform.position = new Vector3 (((float)width + 5.0f) / 2 * Rooms.wallSize, ((float)height + 5.0f) / 2 * Rooms.wallSize, -10.0f);
		//minimapCamera.fieldOfView = width * 3+3;

		main = new Leaf (expand, expand, width+expand, height+expand,0);
		GameObject prefabInstance;
		for (int i = 0; i < board.GetLength(0); i++) {
			for (int j = 0; j < board.GetLength(1); j++) {
				if (board [i, j] == BoardState.Wall) {
					if (i < 3 || j < 3 || i > board.GetLength (0)-3 || j > board.GetLength (1)-3) {
						prefabInstance = (GameObject)Instantiate (wallPrefabs [0], boardController.transform);
					} else {
						if (board [i - 1, j] == BoardState.Floor) {
							if (board [i, j - 1] == BoardState.Floor) {
								prefabInstance = (GameObject)Instantiate (wallPrefabs [8], boardController.transform);
							} else if (board [i, j + 1] == BoardState.Floor) {
								prefabInstance = (GameObject)Instantiate (wallPrefabs [5], boardController.transform);
							} else {
								prefabInstance = (GameObject)Instantiate (wallPrefabs [2], boardController.transform);
							}
						} else if (board [i + 1, j] == BoardState.Floor) {
							if (board [i, j - 1] == BoardState.Floor) {
								prefabInstance = (GameObject)Instantiate (wallPrefabs [7], boardController.transform);
							} else if (board [i, j + 1] == BoardState.Floor) {
								prefabInstance = (GameObject)Instantiate (wallPrefabs [6], boardController.transform);
							} else {
								prefabInstance = (GameObject)Instantiate (wallPrefabs [4], boardController.transform);
							}
						} else if (board [i, j - 1] == BoardState.Floor) {
							prefabInstance = (GameObject)Instantiate (wallPrefabs [1], boardController.transform);
						} else if (board [i, j + 1] == BoardState.Floor) {
							prefabInstance = (GameObject)Instantiate (wallPrefabs [3], boardController.transform);
						} else {
							prefabInstance = (GameObject)Instantiate (wallPrefabs [0], boardController.transform);

						}
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
		GameObject go_exit = (GameObject)Instantiate (exit, spawn, Quaternion.identity);
		player.SetActive (false);
		player.SetActive (true);
	}



}
