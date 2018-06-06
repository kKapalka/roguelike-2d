using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum BoardState{
	Wall,
	Floor,
	Added
}

public class BoardController : MonoBehaviour {
	public static int width,height,expand=7;
	int level=0;
	public static BoardState[,] board;
	float time=0.0f;
	public GameObject[] wallPrefabs;
	public GameObject floor,killerTrap;
	public GameObject boardController;
	public GameObject exit;
	public GameObject player;
	public GameObject endPanel;
	public Text levelText,timerText;
	public Camera minimapCamera;
	int seconds,fraction;
	bool loaded;
	void Awake(){
		Rooms.complete = false;
		loaded = false;
		endPanel.SetActive (false);
		if (!PlayerPrefs.HasKey ("Score"))
			PlayerPrefs.SetInt ("Score", 0);
		if (!PlayerPrefs.HasKey ("Level")) {
			PlayerPrefs.SetInt ("Level", 1);
		} else {
			level=PlayerPrefs.GetInt("Level");
		}
		player.GetComponent<PlayerScript> ().moveController.gameObject.SetActive (true);
		minimapCamera.rect = new Rect (0.02f, 0.7f, 0.2f, 0.3f);
		minimapCamera.fieldOfView = 124+(2*level);

		width = Rooms.Width + (3 * level);
		height = Rooms.Height + (2 * level);
		levelText.text = "Level " + level;


		Rooms.ClearData ();
		Rooms.wallSize = wallPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
		board = new BoardState[width+(expand*2), height+(expand*2)];

		minimapCamera.transform.position = new Vector3 (((float)width + 5.0f) / 2 * Rooms.wallSize, ((float)height+5 + 5.0f) / 2 * Rooms.wallSize, -10.0f);

		Leaf main = new Leaf (expand, expand, width+expand, height+expand);
		GameObject prefabInstance;
		for (int i = expand-1; i < board.GetLength(0)-expand+1; i++) {
			for (int j = expand-1; j < board.GetLength(1)-expand+1; j++) {
				if (board [i, j] == BoardState.Wall) {
					if (getNeighbors (i, j) != 0) {
						prefabInstance = (GameObject)Instantiate (wallPrefabs [getNeighbors (i, j)], boardController.transform);
						prefabInstance.transform.position = new Vector3 (i*Rooms.wallSize, j*Rooms.wallSize, 0f);
					}
				} else {
					if(board[i,j]==BoardState.Floor)
						prefabInstance = (GameObject)Instantiate (floor, boardController.transform);
					else prefabInstance = (GameObject)Instantiate (floor, boardController.transform);
					prefabInstance.transform.position = new Vector3 (i*Rooms.wallSize, j*Rooms.wallSize, 0f);
				}
			}
		}
		//player here
		int randomNumber = Random.Range (0, (Rooms.roomData.ToArray ().Length)/3);
		//finish here
		int randomNumber2 = Random.Range (2*(Rooms.roomData.ToArray ().Length)/3, Rooms.roomData.ToArray ().Length-1);
		Rooms.RoomData data, data2;
		if (Random.Range (0, 10) > 5) {
			data = Rooms.roomData.ToArray () [randomNumber];
			data2 = Rooms.roomData.ToArray () [randomNumber2];
		} else {
			data2 = Rooms.roomData.ToArray () [randomNumber];
			data = Rooms.roomData.ToArray () [randomNumber2];
		}

		Rooms.roomData.Remove (data);
		Instantiate (exit, data.spawnPoint, Quaternion.identity);
		Rooms.roomData.Remove (data2);
		player.transform.position = data2.spawnPoint;
		//to ensure there is more space between them
		for (int i = 0; i < level; i ++) {
			if (Rooms.roomData.Count == 0)
				return;
			randomNumber = Random.Range (0, Rooms.roomData.ToArray ().Length - 1);
			data = Rooms.roomData.ToArray() [randomNumber];
			Rooms.roomData.Remove (data);
			Instantiate (killerTrap, data.spawnPoint, Quaternion.identity);
			killerTrap.GetComponent<KillerTrapScript> ().setSize (data.diameter/2);
		}

	}

	void Update(){
		time += Time.deltaTime;
		seconds = (int)Mathf.Floor (time);
		fraction = (int)(time * 100.0f) % 100;
		timerText.text = seconds + " : " + fraction;
		if (Rooms.complete && !loaded) {
			timerText.gameObject.SetActive (false);
			minimapCamera.cullingMask |= 1 << LayerMask.NameToLayer ("Tracks");
			minimapCamera.cullingMask |= 1 << LayerMask.NameToLayer ("Walls");
			minimapCamera.rect = new Rect (0.5f, 0.1f, 0.5f, 0.8f);
			endPanel.SetActive (true);
			player.GetComponent<PlayerScript> ().moveController.gameObject.SetActive (false);
			if (Rooms.lose) {
				int score = PlayerPrefs.GetInt ("Score");
				endPanel.transform.GetChild (0).gameObject.GetComponent<Text> ().text = "you  lost";
				endPanel.transform.GetChild (1).gameObject.GetComponent<Text> ().text = Rooms.DeathText;
				endPanel.transform.GetChild (1).gameObject.GetComponent<Text> ().text += "\nscore:   " + score;

				if (HighScoreManager.SubmitNewHighScore (score)) {
					endPanel.transform.GetChild (1).gameObject.GetComponent<Text> ().text += "\nnew high score!";
				}
				endPanel.transform.GetChild (3).GetChild(0).gameObject.GetComponent<Text> ().text = "Restart";

				PlayerPrefs.SetInt ("Level", 1);
				PlayerPrefs.SetInt ("Score", 0);


			} else {
				endPanel.transform.GetChild (1).gameObject.GetComponent<Text> ().text = "you  completed  the  level\nin  " + seconds + ":" + fraction + "  seconds!";
				PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")+Mathf.Clamp(100+(20*level)-Mathf.RoundToInt(time*3),0,int.MaxValue));
				PlayerPrefs.SetInt ("Level", PlayerPrefs.GetInt ("Level") + 1);
				endPanel.transform.GetChild (1).gameObject.GetComponent<Text> ().text += "\ncurrent  score:   " + PlayerPrefs.GetInt ("Score");
			}
			loaded = true;

		}
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
