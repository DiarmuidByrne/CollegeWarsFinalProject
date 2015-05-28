using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {
	public GameObject pBoard, pHand, deck;
	public GameObject oBoard, oHand;
	public GameObject imageList;
	
	// Initialize Panels to display info throughout game
	public GameObject quitPanel, moveOverPanel, gameOverPanel;
	public GameObject p1Ready, p2Ready;
	public GameObject gameOverText;
	
	// Cards that have previously fought get sent here
	public GameObject graveYard;
	
	public Button btnFight;
	public Button btnQuit;
	public Button btnQuitSure;
	public Button btnMoveOver;
	public Button btnGameOverMainMenu;
	
	public string pChildName;
	public bool cardsDealt = false;
	public int p1Points = 0, p2Points = 0;
	public bool myTurn = true;
	public bool player1Ready, player2Ready;
	public bool isHost = false;
	public bool roundOver = true;
	public bool gameOver = false;
	
	private bool childChanged = false;
	private bool gameStarted = false;
	private bool cardsRotated = false;
	private bool quitMenuActive = false;
	// Use this for initialization
	void Start () {
		// Add Appropriate functions to Buttons 
		// Functions will then run automatically when the
		// button is pressed
		btnFight.onClick.AddListener(onBtnFightClicked);
		btnQuitSure.onClick.AddListener(onBtnQuitSureClicked);
		btnQuit.onClick.AddListener(onBtnQuitClicked);
		btnMoveOver.onClick.AddListener(onBtnMoveOverClicked);
		btnGameOverMainMenu.onClick.AddListener(onBtnQuitSureClicked);

		// Hides the various panels by default 
		hidePanels();

		// Check for previous locally recorded statistics. Create them if they don't exist
		if (!PlayerPrefs.HasKey ("MPHighScore")) {
			PlayerPrefs.SetFloat ("MPHighScore", 0f);
		}
		if (!PlayerPrefs.HasKey ("PlayerWinsMP")) { 
			PlayerPrefs.SetFloat ("PlayerWinsMP", 0f);
		}
		if (!PlayerPrefs.HasKey ("PlayerLossesMP")) { 
			PlayerPrefs.SetFloat ("PlayerLossesMP", 0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!cardsDealt && pHand.transform.childCount > 0) {
			cardsDealt = true;
		}
		if(pHand.GetComponent<CardValueMP> ().isServerInitialized && !gameOver) {
			battleFunction();

		}

	}
	
	void FixedUpdate () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			onBtnQuitClicked();
		}
	}
	
	void battleFunction() {
		if (cardsDealt) {
			if(pHand.transform.childCount == 0 && pBoard.transform.childCount == 0 || oHand.transform.childCount == 0 && oBoard.transform.childCount == 0) {
				if (GetComponent<NetworkView> ().isMine) {
					GetComponent<NetworkView> ().RPC ("SendP1Points", RPCMode.OthersBuffered, p1Points);
				} else {
					GetComponent<NetworkView> ().RPC ("SendP2Points", RPCMode.OthersBuffered, p2Points);
				}
				GetComponent<NetworkView> ().RPC ("EndGame", RPCMode.AllBuffered);
				gameOver = true;
				return;
			}
		}

		if(pHand.GetComponent<CardValueMP> ().isServerInitialized && !gameStarted) {
			if( GetComponent<NetworkView> ().isMine)
			{
				myTurn = true;
				gameStarted = true;
			} else {
				myTurn = false;
				gameStarted = true;
			}
		}
		// Rotate card position depending on attack stance or defence
		if(!cardsRotated && roundOver) {
			if(pHand.transform.childCount > 0 || oHand.transform.childCount > 0) {
				GetComponent<NetworkView> ().RPC ("RotateCards", RPCMode.AllBuffered);
			}
		}
		
		// Synchronize Board cards between Network using RPC
		if(pBoard.transform.childCount == 1 && childChanged) {
			pChildName = pBoard.transform.GetChild(0).name;
			
			childChanged = false;
			btnFight.enabled = true;
			btnFight.interactable = true;
		}
		else if(pBoard.transform.childCount != 1) {
			if(!childChanged) {
				if(GetComponent<NetworkView>().isMine) {
					GetComponent<NetworkView>().RPC ("RemoveBoardChild", RPCMode.AllBuffered, true);
				} else {
					GetComponent<NetworkView>().RPC ("RemoveBoardChild", RPCMode.AllBuffered, false);
				}
			}
			childChanged = true;
			btnFight.enabled = false;
			btnFight.interactable = false;
		}
	}
	
	[RPC]
	void RotateCards() {
		int i = 0;
		
		if(myTurn) {
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
			// Player defends, rotate pCards
			foreach(Transform child in pHand.transform) {
				i++;
				child.localEulerAngles = new Vector3(0f,0f,-90f);
			}
			pHand.GetComponent<HorizontalLayoutGroup> ().spacing = 35;

			foreach (Transform child in oHand.transform) {
				child.localEulerAngles = new Vector3(0f,0f,0f);
			}
			oHand.GetComponent<HorizontalLayoutGroup> ().spacing = 20;
		}
		if (i > 0 ) {
			cardsRotated = true;
			roundOver = false;
		}
	}
	
	
	void onBtnFightClicked() {
		btnFight.enabled = false;
		btnFight.interactable = false;
		GetComponent<NetworkView>().RPC ("SendBoardChild", RPCMode.OthersBuffered, pChildName);
		if(GetComponent<NetworkView>().isMine) {
			GetComponent<NetworkView> ().RPC("SetPlayer1Ready", RPCMode.AllBuffered);
		} else {
			GetComponent<NetworkView> ().RPC("SetPlayer2Ready", RPCMode.AllBuffered);
		}
		// If both players are ready, compare values
		if(player1Ready && player2Ready) {
			// If this Player won last round - Attack again 
			//if(myTurn){
			GetComponent<NetworkView> ().RPC ("Attack", RPCMode.AllBuffered);
			//}
		}
	}
	
	[RPC]
	void SetPlayer1Ready() {
		// Lock in Player 1's board card:
		player1Ready = true;
		
		if (GetComponent<NetworkView> ().isMine) {
			p2Ready.SetActive (true);
		} else {
			p1Ready.SetActive (true);
		}
	}
	
	[RPC]
	void SetPlayer2Ready() {
		// Lock in Player 2's board card:
		player2Ready = true;
		if (GetComponent<NetworkView> ().isMine) {
			p1Ready.SetActive (true);
		} else {
			p2Ready.SetActive (true);
		}
	}
	
	[RPC]
	void SendBoardChild(string name) {
		GameObject.Find (name).transform.SetParent(oBoard.transform);
	}
	
	[RPC]
	void RemoveBoardChild(bool isHost) {
		if (isHost) {
			player1Ready = false;
			if(!GetComponent<NetworkView> ().isMine) { 
				foreach(Transform child in oBoard.transform) {
					child.SetParent(oHand.transform);
				}
			}
		} else {
			player2Ready = false;
			if(GetComponent<NetworkView> ().isMine) { 
				foreach(Transform child in oBoard.transform) {
					child.SetParent(oHand.transform);
				}
			}
		}
	}
	
	[RPC]
	void Attack() {
		// Resets UI and boolean values to prepare for next round
		resetRound ();
		
		// Reveal the opponent's card before removing it
		oBoard.transform.GetChild(0).GetComponent<Image> ().sprite = imageList.GetComponent<ImageListScript> ().dictCardImages[oBoard.transform.GetChild(0).GetHashCode()];
		// Set End of round score panel to Opaque
		activateMoveOverPanel();
		
		if(myTurn) {
			// Compare Player Board card with Opposition board card
			if(pBoard.transform.GetChild(0).GetComponent<CardStats> ().attack > 
			   oBoard.transform.GetChild(0).GetComponent<CardStats> ().defence) {
				
				if(GetComponent<NetworkView> ().isMine) {
					// This player wins round - Display winning message
					p1Points += 50;
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You win the round!\n+50 Points!\nYour points : " + p1Points;
					myTurn = true;
				} else {
					// This player lost - Display losing message
					p2Points += 50;
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You win the round!\n+50 Points!\nYour points : " + p2Points;
					myTurn = true;
				}
			}else {
				
				if(GetComponent<NetworkView> ().isMine) {
					// This player loses round
					if(deck.transform.childCount > 0) {
						deck.transform.GetChild(0).GetComponent<Image> ().sprite = Resources.Load ("CardBack", typeof(Sprite)) as Sprite;
						deck.transform.GetChild(0).SetParent(oHand.transform);
					}
					p1Points -= 50;
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You lost the attack!\n-50 Points!\nYour points : " + p1Points;
					myTurn = false;
					
				} else {
					// This player lost - Display losing message
					if(deck.transform.childCount > 0) {
						deck.transform.GetChild(0).GetComponent<Image> ().sprite = Resources.Load ("CardBack", typeof(Sprite)) as Sprite;
						deck.transform.GetChild(0).SetParent(oHand.transform);
					}
					p2Points -= 50;
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You lost the attack!\n-50 Points!\nYour points : " + p2Points;
					myTurn = false;
				}
			}
		} else {
			if(pBoard.transform.GetChild(0).GetComponent<CardStats> ().defence > 
			   oBoard.transform.GetChild(0).GetComponent<CardStats> ().attack ) {
				
				if(GetComponent<NetworkView> ().isMine) {
					// This player wins round - Display winning message
					if(deck.transform.childCount > 0) {
						deck.transform.GetChild(0).gameObject.AddComponent<DragNDrop> ();
						deck.transform.GetChild(0).SetParent(pHand.transform);
						
					}
					p1Points += 50;
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You win the round!\n+50 Points!\nYour points : " + p1Points;
					myTurn = true;
					// Pull card from deck
					
				} else {
					// This client wins - Display winning message
					if(deck.transform.childCount > 0) {
						deck.transform.GetChild(0).gameObject.AddComponent<DragNDrop> ();
						deck.transform.GetChild(0).SetParent(pHand.transform);
					}
					p2Points += 50;
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You win the round!\n+50 Points!\nYour points : " + p2Points;
					myTurn = true;
				}
			}else {
				
				if(GetComponent<NetworkView> ().isMine) {
					// This host loses round
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You lost the defence!\nNo point change!\nYour points : " + p1Points;
					myTurn = false;
				} else {
					moveOverPanel.transform.GetChild(0).GetComponent<Text> ().text = "You lost the defence!\nNo point change\nYour points : " + p2Points;
					myTurn = false;
				}
			}
		}
	}
	// Display message to choose to quit to main menu
	void onBtnQuitClicked() {
		if(quitMenuActive) {
			quitPanel.transform.GetComponent<CanvasGroup> ().alpha = 0;
			quitPanel.transform.GetComponent<CanvasGroup> ().interactable = false;
			quitPanel.transform.GetComponent<CanvasGroup> ().blocksRaycasts = false;
			quitMenuActive = false;
		} else {
			quitPanel.transform.GetComponent<CanvasGroup> ().alpha = 1;
			quitPanel.transform.GetComponent<CanvasGroup> ().interactable = true;
			quitPanel.transform.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			quitMenuActive = true;
		}
	}
	
	void onBtnQuitSureClicked () {
		if(pHand.GetComponent<CardValueMP> ().isServerInitialized) {
			GetComponent<NetworkView> ().RPC ("onBtnDisconnectClicked", RPCMode.AllBuffered);
		}
		Application.LoadLevel("opening");
	}

	// Reset bool variables for next round
	void resetRound() {
		cardsRotated = false;
		roundOver = true;
		p1Ready.SetActive (false);
		p2Ready.SetActive (false);
	}
	
	void activateMoveOverPanel() {
		moveOverPanel.GetComponent<CanvasGroup> ().alpha = 1;
		moveOverPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		moveOverPanel.GetComponent<CanvasGroup> ().interactable = true;
	}

	// Continue to next round
	void onBtnMoveOverClicked() {
		GetComponent<NetworkView> ().RPC ("Continue", RPCMode.AllBuffered);
	}

	[RPC]
	void SendP1Points(int points) {
		p1Points = points;
		Debug.Log ("P1Points received: " + p1Points);
	}

	[RPC]
	void SendP2Points(int points) {
		p2Points = points;
		Debug.Log ("P2Points received: " + p2Points);
	}

	// Start next round
	[RPC]
	void Continue() {
		moveOverPanel.GetComponent<CanvasGroup> ().alpha = 0;
		moveOverPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		moveOverPanel.GetComponent<CanvasGroup> ().interactable = false;
		pBoard.transform.GetChild(0).SetParent(graveYard.transform);
		oBoard.transform.GetChild(0).SetParent(graveYard.transform);
	}

	// Display Game over panel. Show outcome and points for respective
	// player
	[RPC]
	void EndGame() {
		gameOverPanel.GetComponent<CanvasGroup> ().alpha = 1;
		gameOverPanel.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		gameOverPanel.GetComponent<CanvasGroup> ().interactable = true;
		if (p1Points > p2Points) {
			if (GetComponent<NetworkView> ().isMine) {
				gameOverText.GetComponent<Text> ().text = "You Won!\nTotal Points: " + p1Points;
				// Save win to PlayerPrefs storage
				float win = PlayerPrefs.GetFloat ("PlayerWinsMP");
				win++;
				PlayerPrefs.SetFloat ("PlayerWinsMP", win);

				// Save score if it beats personal record
				float highScore = PlayerPrefs.GetFloat ("MPHighScore");
				if (p1Points > highScore) {
					PlayerPrefs.SetFloat ("MPHighScore", p1Points);
				}

			} else {
				gameOverText.GetComponent<Text> ().text = "You Lost!\nTotal Points: " + p2Points;
				float loss = PlayerPrefs.GetFloat ("PlayerLossesMP");
				loss++;
				PlayerPrefs.SetFloat ("PlayerLossesMP", loss);
			}
		} else if (p1Points < p2Points) {
			if (GetComponent<NetworkView> ().isMine) {
				gameOverText.GetComponent<Text> ().text = "You Lost!\nTotal Points: " + p1Points;
				float loss = PlayerPrefs.GetFloat ("PlayerLossesMP");
				loss++;
				PlayerPrefs.SetFloat ("PlayerLossesMP", loss);
			} else {
				gameOverText.GetComponent<Text> ().text = "You Won!\nTotal Points: " + p2Points;
				float win = PlayerPrefs.GetFloat ("PlayerWinsMP");
				win++;
				PlayerPrefs.SetFloat ("PlayerWinsMP", win);

				// Save score if it beats personal record
				float highScore = PlayerPrefs.GetFloat ("MPHighScore");
				if (p1Points > highScore) {
					PlayerPrefs.SetFloat ("MPHighScore", p1Points);
				}
			}
		} else {
			if (GetComponent<NetworkView> ().isMine) {
				gameOverText.GetComponent<Text> ().text = "Draw!\nTotal Points: " + p1Points;;
			} else {
				gameOverText.GetComponent<Text> ().text = "Draw!\nTotal Points: " + p2Points;
			}
		}
	}
	// Hide all Panels at the beginning by default
	void hidePanels() {
		p1Ready.SetActive (false);
		p2Ready.SetActive (false);
		quitPanel.GetComponent<CanvasGroup> ().alpha = 0;
		quitPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		quitPanel.GetComponent<CanvasGroup> ().interactable = false;
		gameOverPanel.GetComponent<CanvasGroup> ().alpha = 0;
		gameOverPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		gameOverPanel.GetComponent<CanvasGroup> ().interactable = false;
		moveOverPanel.GetComponent<CanvasGroup> ().alpha = 0;
		moveOverPanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		moveOverPanel.GetComponent<CanvasGroup> ().interactable = false;
	}
	
	[RPC]
	void onBtnDisconnectClicked() {
		Network.Disconnect();
	}
}
