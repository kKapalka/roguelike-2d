using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

	bool notOnMainMenu=false;
	public GameObject continueButton;
	public Text highScoreText;
	public GameObject hintText,creditsText;
	void Start(){
		continueButton.SetActive (false);
		if (PlayerPrefs.HasKey ("Level") && PlayerPrefs.GetInt ("Level") != 1) {
			continueButton.SetActive (true);
		}
		hintText.SetActive (false);
		highScoreText.gameObject.SetActive (false);
		creditsText.SetActive (false);
	}
	public void StartNewGame(){
		PlayerPrefs.SetInt ("Level", 1);
		PlayerPrefs.SetInt ("Score", 0);
		SceneManager.LoadScene ("Level");
	}
	public void ContinueGame(){
		SceneManager.LoadScene ("Level");
	}
	public void ShowCredits(){
		transform.GetChild (1).gameObject.SetActive (false);
		hintText.SetActive (true);
		creditsText.SetActive (true);
		notOnMainMenu = true;
	}
	public void ShowHighScore(){
		transform.GetChild (1).gameObject.SetActive (false);
		hintText.SetActive (true);
		highScoreText.gameObject.SetActive (true);
		highScoreText.text = "";
		if (PlayerPrefs.HasKey ("HighScoreList")) {
			string[] highScore = PlayerPrefs.GetString ("HighScoreList").Split (',');
			for (int i = 0; i < highScore.Length; i++) {
				highScoreText.text+= (i+1)+":  "+(int.Parse(highScore[i])>0?highScore[i]:"- - -")+"\n";
			}

		} else {
			highScoreText.text = "there is no high score at the moment.\nstart playing!";
		}
		notOnMainMenu = true;
	}
	public void ExitGame(){
		Application.Quit ();
	}
	void Update(){
		if (((Input.touchCount>0 && Input.GetTouch (0).phase==TouchPhase.Began) || Input.anyKey || Input.GetMouseButton (0)) && notOnMainMenu) {
			transform.GetChild (1).gameObject.SetActive (true);
			highScoreText.gameObject.SetActive (false);
			hintText.SetActive (false);
			creditsText.SetActive (false);
			notOnMainMenu = false;
		}
	}
}
