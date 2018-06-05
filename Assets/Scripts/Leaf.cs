using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf{
	public int x1,y1,x2,y2;
	Leaf left,right;
	string splitDirection;
	public GameObject roomController;

	public Leaf(int x1,int y1,int x2,int y2){
		this.x1 = x1;
		this.y1 = y1;
		this.x2 = x2;
		this.y2 = y2;
		Split ();
		if (left != null && right != null) {
			ConnectRooms();
		} else {
			CreateRoom ();
		}
	}

	void Split(){
		if (x2 - x1 > y2 - y1) {
			splitDirection = "Vertical";
			if (x2 - x1 < Rooms.minSize * 2)
				return;
		} else {
			splitDirection = "Horizontal";
			if (y2 - y1 < Rooms.minSize * 2)
				return;
		}
		int pos;


		switch (splitDirection) {
		case "Horizontal":
			pos = Mathf.Clamp((int)Random.Range(y1,y2),y1+Rooms.minSize,y2-Rooms.minSize);
			left = new Leaf (x1, y1, x2, pos);
			right = new Leaf (x1, pos, x2, y2);
			break;
		case "Vertical":
			pos = Mathf.Clamp((int)Random.Range(x1,x2),x1+Rooms.minSize,x2-Rooms.minSize);
			left = new Leaf (x1, y1, pos, y2);
			right = new Leaf (pos,y1, x2, y2);
			break;
		}
	}

	void CreateRoom(){
		
		int width = x2 - x1;
		int height = y2 - y1;
		//subtract from outer bounds to create proper walls
		x1=Mathf.Clamp(x1+Random.Range(1,(width/3)+1),0,BoardController.width+BoardController.expand-1);
		y1 = Mathf.Clamp (y1 + Random.Range (1, (height/3)+1), 0, BoardController.height+BoardController.expand-1);
		x2=Mathf.Clamp(x2-Random.Range(1,(width/3)+1),0,BoardController.width+BoardController.expand-1);
		y2=Mathf.Clamp(y2-Random.Range(1,(height/3)+1),0,BoardController.height+BoardController.expand-1);
		//fill inside with floor
		for (int i = x1; i < x2; i++) {
			for (int j = y1; j < y2; j++) {
				BoardController.board [i, j] = BoardState.Floor;
			}
		}
		Rooms.validSpawnPoints.Add(new Vector3((x1+x2-1)/2*Rooms.wallSize,(y1+y2-1)/2*Rooms.wallSize,0f));
		createRoomController ();
	}

	void ConnectRooms(){
		//create a bounding box for all future room connections
		x1 = Mathf.Min (left.x1, right.x1);
		y1= Mathf.Min (left.y1, right.y1);
		x2 = Mathf.Max (left.x2, right.x2);
		y2= Mathf.Max (left.y2, right.y2);
		//coords for the smallest box where a corridor can be placed to connect two rooms together
		int tempx1, tempx2, tempy1, tempy2;
		int fill;

		int coord;
		switch (splitDirection) {
		case "Horizontal":
			tempx1 = Mathf.Max (left.x1, right.x1);
			tempx2 = Mathf.Min (left.x2, right.x2);
			tempy1 = left.y2;
			tempy2 = right.y1;
			coord = Random.Range (tempx1, tempx2);
			//main corridor part
			for (int i = tempy1; i < tempy2; i++) {
				BoardController.board [coord, i] = BoardState.Floor;
				BoardController.board [((coord > BoardController.width / 2) ? coord - 1 : coord + 1), i] = BoardState.Floor;
			}
			//fill until floor has been found
			fill =tempy1-1;
			while (BoardController.board [coord, fill] != BoardState.Floor && fill>BoardController.expand) {
				BoardController.board [coord, fill] = BoardState.Floor;
				BoardController.board [((coord > BoardController.width / 2) ? coord - 1 : coord + 1), fill--] = BoardState.Floor;
			}
			fill = tempy2;
			while (BoardController.board [coord, fill] != BoardState.Floor && fill<BoardController.height) {
				BoardController.board [coord, fill] = BoardState.Floor;
				BoardController.board [((coord > BoardController.width / 2) ? coord - 1 : coord + 1), fill++] = BoardState.Floor;
			}
			break;
		case "Vertical":
			tempy1 = Mathf.Max (left.y1, right.y1);
			tempy2 = Mathf.Min (left.y2, right.y2);
			tempx1 = left.x2;
			tempx2 = right.x1;
			coord = Random.Range (tempy1, tempy2);

			//main corridor part
			for (int i = tempx1; i < tempx2; i++) {
				BoardController.board [i, coord] = BoardState.Floor;
				BoardController.board [i,((coord > BoardController.height / 2) ? coord - 1 : coord + 1)] = BoardState.Floor;
			}
			//fill until floor has been found
			fill =tempx1-1;
			while (BoardController.board [fill,coord] != BoardState.Floor && fill>BoardController.expand) {
				BoardController.board [fill,coord] = BoardState.Floor;
				BoardController.board [fill--,((coord > BoardController.width / 2) ? coord - 1 : coord + 1)] = BoardState.Floor;
			}
			fill = tempx2;
			while (BoardController.board [fill,coord] != BoardState.Floor && fill<BoardController.width) {
				BoardController.board [fill,coord] = BoardState.Floor;
				BoardController.board [fill++,((coord > BoardController.width / 2) ? coord - 1 : coord + 1)] = BoardState.Floor;
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
