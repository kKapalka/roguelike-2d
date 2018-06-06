using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {


	float walkTime,trackDelay=0.4f;
	Vector3 dir,shootDir;
	public GameObject trackPrefab;
	public Controller moveController;
	public BoxCollider2D collider1,collider2;

	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
		if (moveController.InputDirection != Vector3.zero && !Rooms.complete) {
			walkTime += Time.deltaTime;
			if (walkTime > trackDelay) {
				Instantiate (trackPrefab, this.transform.position, Quaternion.identity);
				walkTime = 0f;
			}
			dir = moveController.InputDirection;
			Vector3 rotatedDir = new Vector3 (dir.x, dir.z, 0);
			this.transform.position+=rotatedDir/10;
		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (collider1.IsTouching (other) && other.gameObject.tag == "Killer") {
			Rooms.lose = true;
			Rooms.complete = true;
			Rooms.DeathText = "you  have  been  smacked   by  the  killer  trap";
		} else if (collider1.IsTouching (other) && other.gameObject.tag == "Finish") {
			Rooms.lose = false;
			Rooms.complete = true;
		} else if (other.gameObject.tag == "Wall") {
			other.gameObject.layer = 11;
		}
	}
}
