using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;
using UnityEngine.UI;

public class DealMPCards : CardHitValue {
	private bool cardsMovedToPlayer1, cardsMovedToPlayer2 = false;
	public GameObject tmpDeck;
	public GameObject deck;
	public GameObject pHand;
	public GameObject oHand;
	public GameObject oBoard;
	public GameObject imageList;
	public int indexCard;
	public bool statsAdded = false;
	public int maximum;
	private int max = 12;
	private bool cardsShuffled = false;

	public new List<JSONObject> cardLects = new List<JSONObject>();
	private List<GameObject> mainDeck = new List<GameObject>();
	public GameObject[] shuffledCards = new GameObject[12];

	// Update is called once per frame
	void Update () {
		// Shuffle cards before moving
		if (!GetComponent<NetworkView>().isMine) {
			if(tmpDeck.transform.childCount > 0 && !cardsShuffled &&
			   tmpDeck.GetComponentInChildren<DeckHandler> ().cardsMovedtoTmp) {
				if(mainDeck.Count < max) {
					GetComponent<NetworkView>().RPC("PopulateMainDeck", RPCMode.AllBuffered);
				}
				if(!statsAdded) {
					GetComponent<NetworkView>().RPC ("addJSONStats", RPCMode.AllBuffered);
				}
				shuffleDeck();
			}
		}
		
		if (tmpDeck.transform.childCount == 0 && deck.transform.childCount > 5 && cardsShuffled) {
			if (!cardsMovedToPlayer1 || !cardsMovedToPlayer2) {
				//if(GetComponent<NetworkView>().isMine) {
				GetComponent<NetworkView>().RPC ("moveCardsToPlayer", RPCMode.AllBuffered);
				//}
				//else { 
				GetComponent<NetworkView>().RPC ("moveCardsToOpposition", RPCMode.AllBuffered);
				//}
			}
		}
	}
	
	[RPC]
	void PopulateMainDeck() {
		foreach (Transform child in tmpDeck.transform) {
			mainDeck.Add(child.gameObject);
		}
	}
	
	// Called first
	void shuffleDeck(){
		maximum = tmpDeck.transform.childCount;
		indexCard = Random.Range (0, maximum);
		
		GetComponent<NetworkView>().RPC ("shuffleDeck2", RPCMode.AllBuffered, indexCard);
	}
	
	// Called second
	[RPC]
	void shuffleDeck2(int shuffIndex){
		tmpDeck.transform.GetChild (shuffIndex).SetParent(deck.transform);
		
		if (tmpDeck.transform.childCount == 0) {
			cardsShuffled = true;
		}
	}
	
	// Called third
	[RPC]
	void moveCardsToPlayer(){
		if(!cardsMovedToPlayer1) {
			// Move first 5 cards from deck to player's hand. These cards are already randomized.
			for (int i = 0; i < 5; i++) {
				if(GetComponent<NetworkView>().isMine){
					deck.transform.GetChild (0).gameObject.AddComponent<DragNDrop>();
					deck.transform.GetChild (0).SetParent (pHand.transform); // Add p1 cards to p1 hand
				}
				else {
					deck.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load("CardBack", typeof(Sprite)) as Sprite;
					deck.transform.GetChild (0).SetParent (oHand.transform); // Add p1 cards to p2 opposition
				}
			}
		}
		cardsMovedToPlayer1 = true;
	}
	
	//CalledFourth
	[RPC]
	void moveCardsToOpposition() {
		// Move first 5 cards from deck to player's hand. These cards are already randomized.
		for (int i = 0; i < 5; i++) {;
			if(!cardsMovedToPlayer2) {
				// If (Player != Host)
				if(!GetComponent<NetworkView>().isMine){
					deck.transform.GetChild(0).gameObject.AddComponent<DragNDrop>();
					deck.transform.GetChild (0).SetParent (pHand.transform); // Add p2 cards to p2 hand
				}
				else {
					deck.transform.GetChild (0).GetComponent<Image> ().sprite = Resources.Load("CardBack", typeof(Sprite)) as Sprite;
					deck.transform.GetChild (0).SetParent (oHand.transform); // Add p2 cards to p1 Opposition
				}
			}
		}
		cardsMovedToPlayer2 = true;
	}
	
	[RPC]
	void addJSONStats(){;
		cardLects.Add (cardLect1);
		cardLects.Add (cardLect2);
		cardLects.Add (cardLect3);
		cardLects.Add (cardLect4);
		cardLects.Add (cardLect5);
		cardLects.Add (cardLect6);
		cardLects.Add (cardLect7);
		cardLects.Add (cardLect8);
		
		cardLects.Add (cardStud1);
		cardLects.Add (cardStud2);
		cardLects.Add (cardStud3);
		cardLects.Add (cardStud4);

		int i = 0;
		foreach (Transform child in tmpDeck.transform) {
			child.gameObject.AddComponent <CardStats>();
			child.gameObject.GetComponent<CardStats> ().attack = (cardLects [i].GetNumber ("attack"));
			child.gameObject.GetComponent<CardStats> ().defence = (cardLects [i].GetNumber ("defence"));
			i++;

			if(!imageList.GetComponent<ImageListScript>().dictCardImages.ContainsKey(child.GetHashCode())) {
				imageList.GetComponent<ImageListScript>().dictCardImages.Add(child.GetHashCode(), child.GetComponent<Image>().sprite);
			}
		}
		statsAdded = true;
	}
}