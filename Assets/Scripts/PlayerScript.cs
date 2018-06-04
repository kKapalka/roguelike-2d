using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour {

	//float speed=5.0f;
	float shootDelay=0.4f,specialDelay=1.5f;
	float time=0.0f,special=1.5f;
	Vector3 dir,shootDir;
	//float rotSpeed=25.0f;
	//private Rigidbody2D rigidBody;
	public GameObject bulletPrefab;
	public Controller moveController,shootController;
	public static int currentRoom;

	public static void setCurrentRoom(int cur){
		currentRoom = cur;
	}

	// Use this for initialization
	void Start () {
		Vector3[] spawnPointsArray=Rooms.validSpawnPoints.ToArray();
		Vector3 spawn = spawnPointsArray [Random.Range (0, spawnPointsArray.Length-1)];
		//rigidBody = GetComponent<Rigidbody2D> ();
		this.gameObject.transform.position=spawn;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		special += Time.deltaTime;

		if (moveController.InputDirection != Vector3.zero) {
			dir = moveController.InputDirection;
			Vector3 rotatedDir = new Vector3 (dir.x, dir.z, 0);

			this.gameObject.transform.position+=rotatedDir/10;
		}
		if (shootController.InputDirection != Vector3.zero) {
			
			if (time > shootDelay) {
				shootDir = shootController.InputDirection;
				Vector3 sRotatedDir = new Vector3 (shootDir.x, shootDir.z, 0);
				var bullet = (GameObject)Instantiate (
					bulletPrefab,
					this.transform.position,
					Quaternion.identity);
				bullet.transform.SetParent (this.transform);
				bullet.transform.position = this.transform.position;
				bullet.GetComponent<Bullet> ().setDirection (sRotatedDir);
				time = 0.0f;
			}
		}
	}
	public void shootInAllDirections(){
		if (special > specialDelay) {
			for (int i = 0; i < 12; i++) {
				Vector3 rotDir = new Vector3 (Mathf.Sin (Mathf.Deg2Rad*30.0f * i), Mathf.Cos (Mathf.Deg2Rad*30.0f * i), 0);
				var bullet = (GameObject)Instantiate (
					bulletPrefab,
					this.transform.position,
					Quaternion.identity);
				bullet.transform.SetParent (this.transform);
				bullet.transform.position = this.transform.position;
				bullet.GetComponent<Bullet> ().setDirection (rotDir);
			}
			special = 0.0f;
		}
	}
	public void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag=="Finish")
			StartCoroutine (WaitAndLoad ());
	}
	IEnumerator WaitAndLoad(){
		yield return new WaitForSeconds (2.0f);
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene,LoadSceneMode.Single);
	} 
}
