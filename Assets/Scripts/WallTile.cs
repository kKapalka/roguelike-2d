using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class WallTile : Tile {

    [SerializeField]
    private Sprite[] wallSprites;

    [SerializeField]
    private Sprite preview;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        
        Vector3Int[] neighborWallPositions =
        {
            new Vector3Int(position.x - 1, position.y + 1, position.z),
            new Vector3Int(position.x, position.y + 1, position.z),
            new Vector3Int(position.x + 1, position.y + 1, position.z),
            new Vector3Int(position.x + 1, position.y, position.z),
            new Vector3Int(position.x + 1, position.y - 1, position.z),
            new Vector3Int(position.x, position.y - 1, position.z),
            new Vector3Int(position.x - 1, position.y - 1, position.z),
            new Vector3Int(position.x - 1, position.y, position.z),
        };
        for(int i = 0; i < neighborWallPositions.Length; i++)
        {
            if( isWall(tilemap, neighborWallPositions[i]))
            {
                tilemap.RefreshTile(neighborWallPositions[i]);
            }
        }
        base.RefreshTile(position, tilemap);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        int neighbors = 0;
        int multiplier = 1;
        int[] mapperArray = { 0, 1, 2, 4, 5, 8, 9, 10, 16, 17, 18, 20, 21, 32, 33, 34, 36, 37, 40, 41, 42, 64,
            65, 66, 68, 69, 72, 73, 74, 80, 81, 82, 84, 85, 128, 130, 132, 136, 138, 144, 146, 148, 160, 162, 164, 168, 170 };
        Vector3Int[] neighborWallPositions =
        {
            new Vector3Int(position.x - 1, position.y + 1, position.z),
            new Vector3Int(position.x, position.y + 1, position.z),
            new Vector3Int(position.x + 1, position.y + 1, position.z),
            new Vector3Int(position.x + 1, position.y, position.z),
            new Vector3Int(position.x + 1, position.y - 1, position.z),
            new Vector3Int(position.x, position.y - 1, position.z),
            new Vector3Int(position.x - 1, position.y - 1, position.z),
            new Vector3Int(position.x - 1, position.y, position.z),
        };
        bool[] neighborState = new bool[8];

        tileData.sprite = wallSprites[0];
        for (int i = 0; i < neighborWallPositions.Length; i++)
        {
            neighborState[i] = !isWall(tilemap, neighborWallPositions[i]);
        }
        if (neighborState[1] || neighborState[7])
        {
            neighborState[0] = false;
        }
        if (neighborState[1] || neighborState[3])
        {
            neighborState[2] = false;
        }
        if (neighborState[3] || neighborState[5])
        {
            neighborState[4] = false;
        }
        if (neighborState[5] || neighborState[7])
        {
            neighborState[6] = false;
        }
        for(int i=0;i<neighborState.Length; i++)
        {
            int neighborValue =  neighborState[i]? 1 : 0;
            neighbors += (neighborValue * multiplier);
            multiplier *= 2;
        }
        tileData.sprite = wallSprites[System.Array.IndexOf(mapperArray, neighbors)];
        tileData.colliderType = ColliderType.Sprite;
    }
    public bool isWall(ITilemap tilemap, Vector3Int position)
    {
        return tilemap.GetTile(position) == this;
    }
#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WallTile")]
    public static void CreateWallTile(){
        string path = EditorUtility.SaveFilePanelInProject("Save WallTile","New WallTile", "asset", "Save WallTile", "Assets");
        if(path == ""){
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WallTile>(),path);
    }
#endif

}
