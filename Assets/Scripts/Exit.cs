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
            PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
	}
	public void loadNewLevel(){
        Rooms.lose = false;
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene,LoadSceneMode.Single);
	}


}
