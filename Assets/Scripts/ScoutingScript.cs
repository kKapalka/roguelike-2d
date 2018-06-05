using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutingScript : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Wall") {
			other.gameObject.layer = 11;
		}
	}
}
