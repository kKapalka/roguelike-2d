using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public enum BoardState{
	Wall,
	Floor,
}

public class BoardController : MonoBehaviour {

    public static int BOARD_WIDTH, BOARD_HEIGHT, BOARD_SIDES_BUFFER = 7;
    public static int WIDTH_GROWTH = 3;
    public static int HEIGHT_GROWTH = 2;
	
	public static Rect MINIMAP_POSITION = new Rect(0.02f, 0.7f, 0.2f, 0.3f);
	public static int MINIMAP_STARTING_FIELDOFVIEW = 118;
	int level=0;
	float time=0.0f;
	
	public static BoardState[,] board;
	public GameObject[] wallPrefabs;
	public GameObject floor,killerTrap,shooterTrap;
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
		minimapCamera.rect = MINIMAP_POSITION;
		minimapCamera.fieldOfView = MINIMAP_STARTING_FIELDOFVIEW+(WIDTH_GROWTH*level);

		BOARD_WIDTH = Rooms.Width + (WIDTH_GROWTH * level);
		BOARD_HEIGHT = Rooms.Height + (HEIGHT_GROWTH * level);
		levelText.text = "Level " + level;


		Rooms.ClearData ();
		Rooms.wallSize = wallPrefabs[0].GetComponent<SpriteRenderer>().bounds.size.x;
		board = new BoardState[BOARD_WIDTH+(BOARD_SIDES_BUFFER*2), BOARD_HEIGHT+(BOARD_SIDES_BUFFER*2)];

		minimapCamera.transform.position = new Vector3 (((float)BOARD_WIDTH + 10.0f) / 2 * Rooms.wallSize, ((float)BOARD_HEIGHT+5 + 8.0f) / 2 * Rooms.wallSize, -10.0f);

		Leaf main = new Leaf (BOARD_SIDES_BUFFER, BOARD_SIDES_BUFFER, BOARD_WIDTH+BOARD_SIDES_BUFFER, BOARD_HEIGHT+BOARD_SIDES_BUFFER);
		generateLayout(board);

        plantPlayerAndFinishPoint(player, exit);


        int numberOfKillerTrapsPerLevel = level;
        int numberOfShooterTrapsPerLevel = Mathf.Clamp(level - 3, 0, int.MaxValue);


        plantTraps(killerTrap,numberOfKillerTrapsPerLevel);
        plantTraps(shooterTrap,numberOfShooterTrapsPerLevel);

	}

    void generateLayout(BoardState[,] board){
        GameObject prefabInstance;
        for (int i = BOARD_SIDES_BUFFER-1; i < board.GetLength(0)-BOARD_SIDES_BUFFER+1; i++) {
            for (int j = BOARD_SIDES_BUFFER-1; j < board.GetLength(1)-BOARD_SIDES_BUFFER+1; j++) {
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
    }

    void plantPlayerAndFinishPoint(GameObject player, GameObject finish){
        int leftSideRoomIndex = Random.Range (0, (Rooms.roomData.ToArray ().Length)/3);
        int rightSideRoomIndex = Random.Range (2*(Rooms.roomData.ToArray ().Length)/3, Rooms.roomData.ToArray ()
        .Length-1);
        RoomData finishPosition, playerPosition;
        if (Random.Range (0, 10) > 5) {
            finishPosition = Rooms.roomData.ToArray () [leftSideRoomIndex];
            playerPosition = Rooms.roomData.ToArray () [rightSideRoomIndex];
        } else {
            playerPosition = Rooms.roomData.ToArray () [leftSideRoomIndex];
            finishPosition = Rooms.roomData.ToArray () [rightSideRoomIndex];
        }

        Rooms.roomData.Remove (finishPosition);
        Instantiate (exit, finishPosition.spawnPoint, Quaternion.identity);
        Rooms.roomData.Remove (playerPosition);
        player.transform.position = playerPosition.spawnPoint;
    }

    void plantTraps(GameObject trapType, int numberOfTraps){

        for (int i = 0; i < numberOfTraps; i ++) {
            if (Rooms.roomData.Count == 0)
                return;
            int randomRoomIndex = Random.Range (0, Rooms.roomData.ToArray ().Length - 1);
            RoomData data = Rooms.roomData.ToArray()[randomRoomIndex];
            Rooms.roomData.Remove (data);
            Instantiate (trapType, data.spawnPoint, Quaternion.identity);
        }
    }

	void Update(){
		time += Time.deltaTime;
		seconds = (int)Mathf.Floor (time);
		fraction = (int)(time * 100.0f) % 100;
		timerText.text = seconds + " : " + fraction;
		if (!loaded && Rooms.lose) {
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
				PlayerPrefs.SetInt("Score",PlayerPrefs.GetInt("Score")+Mathf.Clamp(80+(35*level)-Mathf.RoundToInt(time*3),0,int.MaxValue));
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
