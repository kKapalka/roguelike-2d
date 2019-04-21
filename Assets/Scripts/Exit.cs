using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	public void Return(){
		SceneManager.LoadScene ("MainMenu");
	}
	
	public void loadNewLevel(){
        Rooms.lose = false;
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene,LoadSceneMode.Single);
	}


}
