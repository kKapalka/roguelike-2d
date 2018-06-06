using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	public void Return(){
		SceneManager.LoadScene ("MainMenu");
	}
	public void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") {
			Rooms.complete = true;
		}
	}
	public void loadNewLevel(){
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene,LoadSceneMode.Single);
	}


}
