using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf{
	public int x1,y1,x2,y2;
	int pass;
	Leaf left,right;
	string splitDirection;
	public GameObject roomController;

	public Leaf(int x1,int y1,int x2,int y2,int pass){
		this.x1 = x1;
		this.y1 = y1;
		this.x2 = x2;
		this.y2 = y2;
		this.pass = pass;
		if ((x2 - x1 > Rooms.minSize || y2 - y1 > Rooms.minSize) && Rooms.currentSplits<Rooms.maxSplits) {
			Split ();
			ConnectRooms ();
		} else {
			CreateRoom ();
		}
	}

	void Split(){
		Rooms.currentSplits++;
		int maxVariance;
		if (x2 - x1 > y2 - y1) {
			splitDirection = "Vertical";
			maxVariance = Mathf.RoundToInt ((float)(x2 - x1) / 3.0f);
		} else {
			splitDirection = "Horizontal";
			maxVariance = Mathf.RoundToInt ((float)(y2 - y1) / 3.0f);
		}
		//splitDirection = (Mathf.RoundToInt (Random.value) == 1) ? "Horizontal" : "Vertical";
		int pos;


		switch (splitDirection) {
		case "Horizontal":
			pos = (int)Random.Range ((y1 + y2) / 2.0f - maxVariance, (y1 + y2) / 2.0f + maxVariance);
			left = new Leaf (x1, y1, x2, pos, pass + 1);
			right = new Leaf (x1, pos, x2, y2, pass + 1);
			break;
		case "Vertical":
			pos = (int)Random.Range ((x1 + x2) / 2.0f-maxVariance, (x1 + x2) / 2.0f+maxVariance);
			left = new Leaf (x1, y1, pos, y2,pass+1);
			right = new Leaf (pos,y1, x2, y2,pass+1);
			break;
		}
	}

	void CreateRoom(){
		/*x1 = Random.Range (x1, (x1+x2)/2);
		y1 = Random.Range (y1, (y1+y2)/2);
		x2 = Random.Range (x1, x2);
		y2 = Random.Range (y1, y2);*/

		x1=Mathf.Clamp(x1+Random.Range(1,2),0,BoardController.width-1);
		y1 = Mathf.Clamp (y1 + Random.Range (1, 2), 0, BoardController.height-1);
		x2=Mathf.Clamp(x2-Random.Range(1,2),0,BoardController.width-1);
		y2=Mathf.Clamp(y2-Random.Range(1,2),0,BoardController.height-1);
		try{

		for (int i = x1; i < x2; i++) {
			for (int j = y1; j < y2; j++) {
				BoardController.board [i, j] = BoardState.Floor;
			}
		}
		}catch(System.Exception ex){
			Debug.Log (x1);
			Debug.Log (x2);
			Debug.Log (y1);
			Debug.Log (y2);
		}
		Rooms.validSpawnPoints.Add(new Vector3((x1+x2-1)/2*Rooms.wallSize,(y1+y2-1)/2*Rooms.wallSize,0f));
		createRoomController ();
		//Debug.Log ("[" + x1 + "," + y1 + "]-[" + x2 + "," + y2 + "]");
	}

	void ConnectRooms(){
		x1 = Mathf.Min (left.x1, right.x1);
		y1= Mathf.Min (left.y1, right.y1);
		x2 = Mathf.Max (left.x2, right.x2);
		y2= Mathf.Max (left.y2, right.y2);

		int tempx1, tempx2, tempy1, tempy2;


		int coord;
		switch (splitDirection) {
		case "Horizontal":
			tempx1 = Mathf.Max (left.x1, right.x1);
			tempx2 = Mathf.Min (left.x2, right.x2);
			tempy1 = left.y2;
			tempy2 = right.y1;
			coord = Random.Range (tempx1, tempx2);
			for (int i = tempy1-2; i < tempy2+2; i++) {
				BoardController.board [coord, i] = BoardState.Floor;
				BoardController.board [((coord > BoardController.width / 2) ? coord - 1 : coord + 1), i] = BoardState.Floor;
			}
			break;
		case "Vertical":
			tempy1 = Mathf.Max (left.y1, right.y1);
			tempy2 = Mathf.Min (left.y2, right.y2);
			tempx1 = left.x2;
			tempx2 = right.x1;
			coord = Random.Range (tempy1, tempy2);
			for (int i = tempx1-4; i < tempx2+4; i++) {
				BoardController.board [i, coord] = BoardState.Floor;
				BoardController.board [i,((coord > BoardController.height / 2) ? coord - 1 : coord + 1)] = BoardState.Floor;

			}
			break;
		}
	}

	public void createRoomController(){
		roomController = (GameObject)UnityEngine.MonoBehaviour.Instantiate (BoardController.roomController,Vector3.zero,Quaternion.identity);
		roomController.GetComponent<RoomController> ().setColliderBounds (this);
		roomController.GetComponent<RoomController> ().roomBelonging = Rooms.rooms++;
		Rooms.roomControllers.Add (roomController);
	}

}
