using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerTrapScript : MonoBehaviour {

	float speed,size;

	void Start(){
		speed = Random.Range (3, 9);
		size = Random.Range (4, Rooms.minSize);
		speed = Mathf.Clamp(Random.Range (8, 14)-size,3,float.MaxValue);
		transform.GetChild (0).transform.localScale = new Vector3(size/4,size/4,1);
	}
	public void setSize(float s){
		Debug.Log (s);

	}
	// Update is called once per frame
	void Update () {
		transform.GetChild (0).transform.Rotate (new Vector3 (0.0f, 0.0f, speed/3));
	}
}
