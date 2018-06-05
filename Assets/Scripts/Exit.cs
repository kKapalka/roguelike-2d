using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	public void Ex(){
		Application.Quit ();
	}
	public void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag=="Player")
			StartCoroutine (WaitAndLoad ());
	}


	IEnumerator WaitAndLoad(){
		yield return new WaitForSeconds (2.0f);
		int scene = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (scene,LoadSceneMode.Single);
	} 

}
