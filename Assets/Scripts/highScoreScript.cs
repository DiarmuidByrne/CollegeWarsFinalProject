using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class highScoreScript : MonoBehaviour {

	public GameObject panelSP, panelMP;
	public Text spScoreText, spWinsText, spLossesText, spRatioText;
	public Text mpScoreText, mpWinsText, mpLossesText, mpRatioText;

	public Button clearButton;
	private float spHighScore, mpHighScore;
	private float spWins, mpWins;
	private float spLosses, mpLosses;
	
	void Start(){
		clearButton.onClick.AddListener(clearScores);

		// Check for previous locally recorded statistics. Create them if they don't exist
		if (!PlayerPrefs.HasKey ("PlayerHighScore")) {
			PlayerPrefs.SetFloat ("PlayerHighScore", 0f);
		}
		if (!PlayerPrefs.HasKey ("PlayerWins")) { 
			PlayerPrefs.SetFloat ("PlayerWins", 0f);
		}
		if (!PlayerPrefs.HasKey ("PlayerLosses")) { 
			PlayerPrefs.SetFloat ("PlayerLosses", 0f);
		}
		if (!PlayerPrefs.HasKey ("PlayerWLRatio")) { 
			PlayerPrefs.SetFloat ("PlayerWLRatio", 0.00f);
		}

		// Get Single player scores
		spWins = PlayerPrefs.GetFloat ("PlayerWins");
		spLosses = PlayerPrefs.GetFloat ("PlayerLosses");
		spHighScore = PlayerPrefs.GetFloat ("PlayerHighScore");

		// Get Multiplayer scores
		mpWins = PlayerPrefs.GetFloat ("PlayerWinsMP");
		mpLosses = PlayerPrefs.GetFloat ("PlayerLossesMP");
		mpHighScore = PlayerPrefs.GetFloat ("MPHighScore");

		showScores ();
	}

	// Clear all scores
	public void clearScores() {
		PlayerPrefs.DeleteAll ();
		showScores ();
		Application.LoadLevel(2);
	}

	void showScores () {
		// Display Single Player Score
		spWinsText.text = "Wins: " + spWins; 
		spLossesText.text = "Losses: " + spLosses;
		spRatioText.text = "Win/Loss Ratio: " + spWins /spLosses;
		spScoreText.text = "High Score: " + spHighScore;
		
		// Display Multiplayer Score
		mpWinsText.text = "Wins: " + mpWins;
		mpLossesText.text = "Losses: " + mpLosses;
		mpRatioText.text = "Win/Loss Ratio: " + mpWins / mpLosses;
		mpScoreText.text = "High Score: " + mpHighScore;
	}

}
