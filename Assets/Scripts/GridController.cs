using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridController : MonoBehaviour {

    static int BASE_WIDTH = 30, BASE_HEIGHT = 25;
    public static int SIDES_BUFFER = 7;
    static int WIDTH_GROWTH = 5, HEIGHT_GROWTH = 4;
    public static int BOARD_WIDTH, BOARD_HEIGHT;
    public Tilemap floors, walls;
    public Tile floor, wall;
    public static BoardState[,] board;
    int level = 1;

    public GameObject exit;
    public GameObject player;
    public GameObject killerTrap, shooterTrap;

    private void Awake()
    {
        Rooms.complete = false;
        //loaded = false;
        //endPanel.SetActive(false);
        Rooms.wallSize = floor.sprite.rect.size.x / 100;
    }
    private void Start()
    {
        player.GetComponent<PlayerScript>().moveController.gameObject.SetActive(true);
        board = null;
        if (PlayerPrefs.HasKey("Level"))
        {
            level = PlayerPrefs.GetInt("Level");
        }
        InitializeBoard();
        plantPlayerAndFinishPoint(player, exit);
        int numberOfKillerTrapsPerLevel = level;
        int numberOfShooterTrapsPerLevel = Mathf.Clamp(level - 3, 0, int.MaxValue);
        plantTraps(killerTrap, numberOfKillerTrapsPerLevel);
        plantTraps(shooterTrap, numberOfShooterTrapsPerLevel);
    }
    public void InitializeBoard()
    {
        walls.GetComponent<TilemapCollider2D>().gameObject.SetActive(false);
        BOARD_WIDTH = BASE_WIDTH + (WIDTH_GROWTH * level);
        BOARD_HEIGHT = BASE_HEIGHT + (HEIGHT_GROWTH * level);
        Rooms.ClearData();
        Leaf main = new Leaf(SIDES_BUFFER, SIDES_BUFFER, BOARD_WIDTH + SIDES_BUFFER, BOARD_HEIGHT + SIDES_BUFFER);

        for (int i = SIDES_BUFFER - 1; i < board.GetLength(0) - SIDES_BUFFER + 1; i++)
        {
            for (int j = SIDES_BUFFER - 1; j < board.GetLength(1) - SIDES_BUFFER + 1; j++)
            {
                if (board[i, j] == BoardState.Wall)
                {
                    walls.SetTile(new Vector3Int(i, j, 0), wall);
                    walls.SetColliderType(new Vector3Int(i, j, 0), Tile.ColliderType.Sprite);
                }
                else
                {
                    floors.SetTile(new Vector3Int(i, j, 0), floor);
                }
            }
        }
        walls.GetComponent<TilemapCollider2D>().gameObject.SetActive(true);
    }

    void plantPlayerAndFinishPoint(GameObject player, GameObject finish)
    {
        int leftSideRoomIndex = Random.Range(0, (Rooms.roomData.ToArray().Length) / 3);
        int rightSideRoomIndex = Random.Range(2 * (Rooms.roomData.ToArray().Length) / 3, Rooms.roomData.ToArray()
        .Length - 1);
        RoomData finishPosition, playerPosition;
        if (Random.Range(0, 10) > 5)
        {
            finishPosition = Rooms.roomData.ToArray()[leftSideRoomIndex];
            playerPosition = Rooms.roomData.ToArray()[rightSideRoomIndex];
        }
        else
        {
            playerPosition = Rooms.roomData.ToArray()[leftSideRoomIndex];
            finishPosition = Rooms.roomData.ToArray()[rightSideRoomIndex];
        }

        Rooms.roomData.Remove(finishPosition);
        Instantiate(exit, finishPosition.spawnPoint, Quaternion.identity);
        Rooms.roomData.Remove(playerPosition);
        player.transform.SetPositionAndRotation(playerPosition.spawnPoint,Quaternion.identity);
    }

    void plantTraps(GameObject trapType, int numberOfTraps)
    {

        for (int i = 0; i < numberOfTraps; i++)
        {
            if (Rooms.roomData.Count == 0)
                return;
            int randomRoomIndex = Random.Range(0, Rooms.roomData.ToArray().Length - 1);
            RoomData data = Rooms.roomData.ToArray()[randomRoomIndex];
            Rooms.roomData.Remove(data);
            Instantiate(trapType, data.spawnPoint, Quaternion.identity);
        }
    }
    
    // Update is called once per frame
    void Update () {
		
	}
}
