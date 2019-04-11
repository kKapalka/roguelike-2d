using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerTrapScript : MonoBehaviour {

	float speed,size;

	void Start(){
		size = Random.Range (5, Rooms.minSize);
		speed = Mathf.Clamp(Random.Range (8, 14)-size,3,float.MaxValue);
		transform.GetChild (0).transform.localScale = new Vector3(size/6,size/6,1);
	}

	// Update is called once per frame
	void Update () {
		transform.GetChild (0).transform.Rotate (new Vector3 (0.0f, 0.0f, speed/3));
	}
}
