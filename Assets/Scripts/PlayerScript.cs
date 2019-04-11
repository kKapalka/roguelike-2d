using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {


	float walkTime,trackDelay=0.4f;
	Vector3 dir,shootDir;
	float animationTime=0.0f;
	public Sprite[] animationSprites;
	public Sprite[] idle;
	public GameObject trackPrefab;
	public Controller moveController;
	public BoxCollider2D collider1,collider2;
	Vector3 lastDirection;
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
            Debug.Log(dir.x);
            Debug.Log(dir.z);
            lastDirection = rotatedDir;
			this.transform.position += rotatedDir / 12;
            animationTime += Time.deltaTime * Mathf.Clamp(Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.z)), 0.25f, float.MaxValue);
            GetComponent<SpriteRenderer> ().sprite = animationSprites [(int)Mathf.Floor (animationTime * 8) % 4+(getDirection(rotatedDir)*4)];

		} else {
			animationTime = 0.0f;
			GetComponent<SpriteRenderer> ().sprite = idle[getDirection(lastDirection)];

		}
	}
	void OnTriggerEnter2D(Collider2D other){
		if (collider1.IsTouching (other) && other.gameObject.tag == "Killer") {
			Rooms.lose = true;
			Rooms.complete = true;
			Rooms.DeathText = "you  have  been  smacked   by  the  killer  trap";
		} else if (collider1.IsTouching (other) && other.gameObject.tag == "Bullet") {
			Destroy (other.gameObject);
			Rooms.lose = true;
			Rooms.complete = true;
			Rooms.DeathText = "you  have  been  shot   by  the  shooter  trap";
		} else if (collider1.IsTouching (other) && other.gameObject.tag == "Finish") {
			Rooms.lose = false;
			Rooms.complete = true;
		} else if (other.gameObject.tag == "Wall") {
			other.gameObject.layer = 11;
		}
	}
	int getDirection(Vector3 dir){
		int direction = 0;
		if (dir.x > 0)
			direction += 1;
		if (dir.y > 0)
			direction += 2;
		return direction;
	}
}
