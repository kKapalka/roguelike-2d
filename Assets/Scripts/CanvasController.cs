using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    int level = 0, score = 0;
    float time = 0.0f;
    public GameObject endPanel;
    public Text levelText, scoreText, timerText;
    private static int DEFAULT_SECONDS_PER_POINT = 6;
    int seconds, fraction;
    bool loaded;

    // Use this for initialization
    void Start () {
        endPanel.SetActive(false);

        if (!PlayerPrefs.HasKey("Score"))
        {
            PlayerPrefs.SetInt("Score", 0);
        }
        else
        {
            score = PlayerPrefs.GetInt("Score");
        }
        if (!PlayerPrefs.HasKey("Level"))
        {
            PlayerPrefs.SetInt("Level", 1);
        }
        else
        {
            level = PlayerPrefs.GetInt("Level");
        }
        levelText.text = "Level " + level;
        scoreText.text = "Score: " + score;
    }
	
	// Update is called once per frame
	void Update () {
        if (levelText.color.a > 0)
        {
            Color levelTextColor = levelText.color;
            levelTextColor.a = Mathf.Clamp((levelText.color.a - (Time.deltaTime / 3f)), 0.0f, 1.0f);
            levelText.color = levelTextColor;
        }
        time += Time.deltaTime;
        if (!Rooms.complete)
        {
            timerText.text = seconds + " : " + fraction;
        }
        seconds = (int)Mathf.Floor(time);
        fraction = (int)(time * 100.0f) % 100;
        if (!loaded && (Rooms.lose || Rooms.complete))
        {
            if (Rooms.lose)
            {
                timerText.gameObject.SetActive(false);
                endPanel.SetActive(true);
                int score = PlayerPrefs.GetInt("Score");
                endPanel.transform.GetChild(0).gameObject.GetComponent<Text>().text = "you  lost";
                endPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text = Rooms.DeathText;
                endPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text += "\nscore:   " + score;

                if (HighScoreManager.SubmitNewHighScore(score))
                {
                    endPanel.transform.GetChild(1).gameObject.GetComponent<Text>().text += "\nnew high score!";
                }
                endPanel.transform.GetChild(3).GetChild(0).gameObject.GetComponent<Text>().text = "Restart";

                PlayerPrefs.SetInt("Level", 1);
                PlayerPrefs.SetInt("Score", 0);
            }
            else if (Rooms.complete)
            {
                int secondsPerLevel = DEFAULT_SECONDS_PER_POINT + (PlayerPrefs.GetInt("Level") * 2);
                int scoreAddition = Mathf.Clamp((int)Mathf.Floor(((secondsPerLevel * 3) - seconds) / (float)secondsPerLevel), 0, 3);
                StartCoroutine(AddScore(PlayerPrefs.GetInt("Score"), 1 + scoreAddition));
                int newScore = PlayerPrefs.GetInt("Score") + 1 + scoreAddition;
                PlayerPrefs.SetInt("Score", newScore);
                timerText.text += "\nlevel complete!";
            }
            loaded = true;

        }
    }
    IEnumerator AddScore(int baseScore, int scoreAddition)
    {
        int score = baseScore;
        for (int i = 0; i < scoreAddition; i++)
        {
            score += 1;
            scoreText.text = "Score: " + score;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
