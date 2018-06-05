using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

	public int roomBelonging;
	public void setColliderBounds(Leaf leaf){
		int x1 = leaf.x1;
		int y1 = leaf.y1;
		int x2 = leaf.x2;
		int y2=  leaf.y2;
		transform.position = new Vector3 ((float)(x1 + x2-1) / 2.0f * Rooms.wallSize, (float)(y1 + y2-1) / 2.0f * Rooms.wallSize, 0f);
		transform.localScale = new Vector3 ((float)(x2-x1+1.5) * Rooms.wallSize, (float)(y2-y1+1.5) * Rooms.wallSize,0f);
	}
}
