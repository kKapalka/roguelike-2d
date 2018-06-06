using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTrapScript : MonoBehaviour {

	float time,shootDelay=1.0f;
	GameObject bulletPrefab;
	int shootPhase=0;
	// Use this for initialization
	void Start () {
		bulletPrefab = (GameObject)Resources.Load ("Bullet");
		Debug.Log (bulletPrefab);
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > shootDelay) {
			for (int i = 0; i < 4; i++) {
				Vector3 rotDir = new Vector3 (Mathf.Sin (i*Mathf.PI/2+(shootPhase*Mathf.PI/6)), Mathf.Cos (i*Mathf.PI/2+(shootPhase*Mathf.PI/6)), 0);
				GameObject bullet = (GameObject)Instantiate (bulletPrefab, transform.position, Quaternion.identity);
				bullet.transform.SetParent (this.transform);
				bullet.transform.position = this.transform.position;
				bullet.GetComponent<Bullet> ().setDirection (rotDir);
			}
			shootPhase++;
			time = 0f;
		}
	}
}
