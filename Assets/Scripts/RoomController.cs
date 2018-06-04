using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour {

	public int roomBelonging;
	List<GameObject> walls = new List<GameObject>();
	BoxCollider2D col;
	void Start(){
		col = GetComponent<BoxCollider2D> ();
	}
	public void setColliderBounds(Leaf leaf){
		int x1 = leaf.x1;
		int y1 = leaf.y1;
		int x2 = leaf.x2;
		int y2=  leaf.y2;
		transform.position = new Vector3 ((float)(x1 + x2-1) / 2.0f * Rooms.wallSize, (float)(y1 + y2-1) / 2.0f * Rooms.wallSize, 0f);
		transform.localScale = new Vector3 ((float)(x2-x1+1.5) * Rooms.wallSize, (float)(y2-y1+1.5) * Rooms.wallSize,0f);
	}


	public void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			Debug.Log ("Player has entered the room no. " + roomBelonging);
			PlayerScript.setCurrentRoom (roomBelonging);
			foreach (GameObject wall in walls) {
				wall.layer = 11;
			}
		} else {
			walls.Add (other.gameObject);
		}
	}
	/*public void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
		foreach (GameObject wall in walls) {
				wall.layer = 11;
			}
		} else {
			walls.Add (other.gameObject);
		}
	}*/

}
