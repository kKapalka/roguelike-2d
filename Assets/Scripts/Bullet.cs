using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	Vector3 direction,position;
    Rigidbody2D body;
	void Update(){
        //this.body.MovePosition(new Vector2(this.body.position.x + (direction.x * Time.deltaTime * 1.8f), this.body.position.y + (direction.y * Time.deltaTime * 1.8f)));
	}
	void Start(){
        body = GetComponent<Rigidbody2D>();
        this.body.velocity = new Vector2(direction.x * 1.8f, direction.y * 1.8f);
		Destroy (this.gameObject, 3.0f);
	}
	public void setDirection(Vector3 dir){
		this.direction = dir;
	}
	void OnTriggerEnter2D(Collider2D other){
        if (other.name == "UnwalkablesTilemap")
			Destroy (this.gameObject);
	}
}
