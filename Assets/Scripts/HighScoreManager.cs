using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class HighScoreManager{

	public static bool SubmitNewHighScore(int score){
		int[] newHighScore;

		if (PlayerPrefs.HasKey ("HighScoreList")) {
			string[] HighScore = PlayerPrefs.GetString ("HighScoreList").Split (',');
			newHighScore = new int[HighScore.Length+1];
			for (int i = 0; i < HighScore.Length; i++) {
				newHighScore [i] = int.Parse (HighScore [i]);
			}
			newHighScore [HighScore.Length] = score;
			newHighScore = newHighScore.OrderByDescending (sc => sc).ToArray ();
			int[] highScoreCopy = new int[10];
			System.Array.Copy (newHighScore, highScoreCopy, (int)Mathf.Min(newHighScore.Length,10));
			newHighScore = highScoreCopy;
		} else {
			newHighScore = new int[10]{score,0,0,0,0,0,0,0,0,0};

		}
		PlayerPrefs.SetString("HighScoreList",string.Join(",",newHighScore.Select(x=>x.ToString()).ToArray()));
		return score == newHighScore [0];
	}
}
