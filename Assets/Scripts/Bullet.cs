using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	Vector3 direction,position;

	void Update(){
		this.transform.position += direction / 5;
	}
	void Start(){
		Destroy (this.gameObject, 3.0f);
	}
	public void setDirection(Vector3 dir){
		this.direction = dir;
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Wall")
			Destroy (this.gameObject);
	}
}
