
/*This Script handles the battle between the players
 *Each Root GameObject is stored in a variable on start.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using Boomlagoon.JSON;
using UnityEngine.UI;

/* Things to Implement:
 * Remove fight button & streamline moves
 * Add boolean for when endOfMovePanel is active
 * */

public class BattleScript : CardHitValue {
	public GameObject oBoard, pBoard, deck;
	public GameObject pHand, oHand;

	private GameObject gameOverPanel;
	private GameObject endOfMovePanel;

	private Text titleText;
	public JSONArray jsonCards;
	public Button fightBtn;

	public bool playerTurn = true; 
	private bool cardsDealt = false;
	private bool cardToSpin = false, cardsToRotate = true;
	private int score;
	public double attack, defence;
	private float win, loss;
	// Change won to int gameStatus; 0 = in play, 1 = lost, 2 = won
	public static int gameStatus = 0;
	public static bool won = false;
	public double highOCardStat;
	public int highOCardIndex;

	public bool rotatedOnce = false;
	private bool gameOver = false;
	public float turnSpeed = 600f;
	public GameObject imageList;

	// Use this for initialization
	void Start () {
		imageList = GameObject.Find ("ImageList");
		gameOverPanel = GameObject.Find ("GameOverPanel");
		endOfMovePanel = GameObject.Find ("MoveOverPanel");
		fightBtn = GameObject.Find ("Canvas").transform.FindChild ("Fight").gameObject.GetComponent<Button> ();

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

		fightBtn.interactable = false;
		fightBtn.enabled = false;
		fightBtn.GetComponentInChildren<Text> ().text = "Fight!";
	}
	

	void FixedUpdate() {
		if(cardsToRotate) {
			rotateCards();
		}

		if (cardToSpin) {
			rotatedOnce = false;
			if(oBoard.transform.childCount > 0) {
				if (oBoard.transform.GetChild (0).rotation.y > .1f) {
					oBoard.transform.GetChild (0).rotation.Set (0f, 1f, 0f, 0f);
					rotatedOnce = true;
					cardToSpin = false;
				}
				else{
					oBoard.transform.GetChild (0).Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
					oBoard.transform.GetChild (0).GetComponent<Image>().sprite = imageList.GetComponent<ImageListScript>().dictCardImages[oBoard.transform.GetChild(0).GetHashCode()];
				}
			}
		}
	}

	void rotateCards() {
		int i = 0;
		if(playerTurn) {
			// Player attacks, rotate oCards
			foreach(Transform child in oHand.transform) {
				child.localEulerAngles = new Vector3(0f,0f,-90f);
				i++;
			}
			oHand.GetComponent<HorizontalLayoutGroup> ().spacing = 35;
			foreach (Transform child in pHand.transform) {
				child.localEulerAngles = new Vector3(0f,0f,0f);
			}
			pHand.GetComponent<HorizontalLayoutGroup> ().spacing = 20;
		} else {
			// Player attacks, rotate oCards
			foreach(Transform child in pHand.transform) {
				child.localEulerAngles = new Vector3(0f,0f,-90f);
				i++;
			}
			pHand.GetComponent<HorizontalLayoutGroup> ().spacing = 35;
			foreach (Transform child in oHand.transform) {
				child.localEulerAngles = new Vector3(0f,0f,0f);
			}
			oHand.GetComponent<HorizontalLayoutGroup> ().spacing = 20;
		}
		if(i > 0) {
			cardsToRotate = false;
		}
	}

	// Continues game after move results
	public void onContinueClicked() {
		endOfMovePanel.GetComponent<CanvasGroup> ().alpha = 0f;
		// Won
		if (gameStatus == 2) {
			Destroy (oBoard.transform.GetChild (0).gameObject);
			Destroy (pBoard.transform.GetChild (0).gameObject);
			endOfMovePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
			endOfMovePanel.GetComponent<CanvasGroup>().interactable = false;
			gameStatus = 0;
			playerTurn = true;
			fightBtn.interactable = false;
			fightBtn.enabled = false;

		// Lost
		} else if (gameStatus == 1) {
			gameStatus = 0;
			Destroy(pBoard.transform.GetChild(0).gameObject);
			Destroy(oBoard.transform.GetChild(0).gameObject);
			fightBtn.interactable = false;
			fightBtn.enabled = false;
			endOfMovePanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
			endOfMovePanel.GetComponent<CanvasGroup>().interactable = false;

			// Loops through opposition hand to find card with highest stat
			// and places it on the board
			int i = 0;
			foreach(Transform child in oHand.transform) {
				if(i == 0){
					// Send card with highest attack to board
					highOCardStat = child.GetComponent<CardStats>().attack;		
					highOCardIndex = child.GetSiblingIndex();
				} else {
					if(child.GetComponent<CardStats>().attack > highOCardStat) {
						highOCardStat = child.GetComponent<CardStats>().attack;
						highOCardIndex = child.GetSiblingIndex();
					}
				}
				i++;
			}

			if(oHand.transform.childCount > 0) {
				oHand.transform.GetChild (highOCardIndex).SetParent (oBoard.transform);
				oBoard.transform.GetChild(0).GetComponent<Image>().sprite = imageList.GetComponent<ImageListScript>().dictCardImages[oBoard.transform.GetChild(0).GetHashCode()];
				if (oBoard.transform.childCount > 0){
					cardToSpin = true;
				}
			}
			playerTurn = false;
			fightBtn.interactable = false;
			fightBtn.enabled = false;
		}
	}

	// Update is called once per frame 
	void Update () {
		// Checks Every frame that the player board has 1 child
		// If conditions are met, their values are compared
		if (pBoard.transform.childCount == 1 && gameStatus == 0) {
			fightBtn.interactable = true;
			fightBtn.enabled = true;
		} else {
			fightBtn.interactable = false;
			fightBtn.enabled = false;
		}

		if (pHand.transform.childCount == 5) { 
			cardsDealt = true;
		}

		// If cards have been dealt and no cards remain for players (Game is over)
		if (!gameOver && pHand.transform.childCount == 0 && pBoard.transform.childCount == 0
			&& oHand.transform.childCount == 0 && oBoard.transform.childCount == 0 && cardsDealt) {
			gameOverPanel.GetComponent<CanvasGroup>().alpha = 1;
			gameOverPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
			gameOverPanel.GetComponent<CanvasGroup>().interactable = true;
			gameOver = true;

			int timesplayed = PlayerPrefs.GetInt("TimesPlayed");
			timesplayed ++;
			PlayerPrefs.SetInt("TimesPlayed", timesplayed);

			if(score < 0) {
				// Loss
				gameOverPanel.transform.FindChild ("statusText").GetComponent<Text>().text = "You lost!\nYour score: " + score;

				loss = PlayerPrefs.GetFloat("PlayerLosses");
				loss++;
				PlayerPrefs.SetFloat("PlayerLosses", loss);

				fightBtn.interactable = false;
				fightBtn.enabled = false;
				return;

			}
			else if (score > 0) {
				// Win
				gameOverPanel.transform.FindChild ("statusText").GetComponent<Text>().text = "You Won!\nYour score: " + score;
				win = 1;
				win += PlayerPrefs.GetFloat("PlayerWins");

				// If Current score > personal best, save to PlayerPrefs storage
				float highScore = PlayerPrefs.GetFloat("PlayerHighScore");
				if(score > highScore) {
					PlayerPrefs.SetFloat("PlayerHighScore", score);
				}

				// Add to playerprefs
				PlayerPrefs.SetFloat("PlayerWins", win);

				fightBtn.interactable = false;
				fightBtn.enabled = false;
				return;
			}
		}
	}

	public void onClickfightBtn(){

		if (playerTurn) {
			// Loops through opposition hand to find card with highest stat
			// and places it on the board
			int i = 0;
			foreach(Transform child in oHand.transform) {
				if(i == 0){
					// Set highest attack int to first card
					highOCardStat = child.GetComponent<CardStats>().defence;		
					highOCardIndex = child.GetSiblingIndex();
				} else {
					if(child.GetComponent<CardStats>().defence > highOCardStat) {
						highOCardStat = child.GetComponent<CardStats>().defence;
						highOCardIndex = child.GetSiblingIndex();
					}
				}
				i++;
			}

			oHand.transform.GetChild(highOCardIndex).SetParent(oBoard.transform);
			fight ();
		} else {
			fightDefend();
		}
	}


	void fight(){
		// Compares attack value of playerCard to defence of opposition 
		if(pBoard.transform.GetChild(0).GetComponent<CardStats>().attack > 
		   oBoard.transform.GetChild(0).GetComponent<CardStats>().defence) {
			gameStatus = 2;
			endOfMovePanel.GetComponent<CanvasGroup>().alpha = 1;
			endOfMovePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
			endOfMovePanel.GetComponent<CanvasGroup>().interactable = true;
			endOfMovePanel.transform.FindChild("ContinueBtn").GetComponent<Button>().interactable = true;
			score+=50;
			endOfMovePanel.transform.FindChild("statusText").GetComponent<Text>().text = "Opposition Card Destroyed! \n+50Points!\nCurrent score is " + score;
			oBoard.transform.GetChild(0).GetComponent<Image>().sprite = imageList.GetComponent<ImageListScript>().dictCardImages[oBoard.transform.GetChild(0).GetHashCode()];
			cardToSpin = true;
			fightBtn.interactable =false;
			fightBtn.enabled = false;
			playerTurn = true;
			cardsToRotate = true;
		}

		// Lost attack - Bring new opposition card up front
		else {
			gameStatus = 1;
			endOfMovePanel.GetComponent<CanvasGroup>().alpha = 1;
			endOfMovePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
			endOfMovePanel.GetComponent<CanvasGroup>().interactable = true;
			score-=50;
			endOfMovePanel.transform.FindChild("statusText").GetComponent<Text>().text = "Your Card was Destroyed! \n-50Points!\nCurrent score is " + score;
			oBoard.transform.GetChild(0).GetComponent<Image>().sprite = imageList.GetComponent<ImageListScript>().dictCardImages[oBoard.transform.GetChild(0).GetHashCode()];
			cardToSpin = true;
			// Disable fight button
			fightBtn.interactable =false;
			fightBtn.enabled = false;
			// Player lost, opposition makes next turn
			playerTurn = false;
			cardsToRotate = true;

		}
	}

	void fightDefend(){
		// Compares defend value of playerCard to attack of opposition 
		if (oBoard.transform.GetChild (0).GetComponent<CardStats> ().attack < 
			pBoard.transform.GetChild (0).GetComponent<CardStats> ().defence) {
			won = true;
			gameStatus = 2;
			playerTurn = true; // Player must start next attack
			endOfMovePanel.GetComponent<CanvasGroup>().alpha = 1;
			endOfMovePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
			endOfMovePanel.GetComponent<CanvasGroup>().interactable = true;
			endOfMovePanel.transform.FindChild("ContinueBtn").GetComponent<Button>().interactable = true;
			score+=50;
			endOfMovePanel.transform.FindChild("statusText").GetComponent<Text>().text = "Opposition Card Destroyed! \n+50Points!\nCurrent score is " + score;;
			cardToSpin = true;
			fightBtn.interactable =false;
			fightBtn.enabled = false;
			oBoard.transform.GetChild(0).GetComponent<Image>().sprite = imageList.GetComponent<ImageListScript>().dictCardImages[oBoard.transform.GetChild(0).GetHashCode()];
			cardsToRotate = true;
		} else {

			won = false;
			gameStatus = 1;
			playerTurn=false; // Opposition must start next attack
			endOfMovePanel.GetComponent<CanvasGroup>().alpha = 1;
			endOfMovePanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
			endOfMovePanel.GetComponent<CanvasGroup>().interactable = true;
			score-=50;	
			endOfMovePanel.transform.FindChild("statusText").GetComponent<Text>().text = "Your Card was Destroyed! \n-50Points!\nCurrent score is " + score;;
			fightBtn.interactable =false;
			fightBtn.enabled = false;
			cardsToRotate = true;
		}
	}
}	