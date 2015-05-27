
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Deck handler.
/// This operation initializes a standard deck of cards based
/// off of Vector2's. (First input is suit, second is value.)
/// This both instantiates a deck and shuffles the deck.
/// </summary>
public class DeckHandler : MonoBehaviour{
	public GameObject card;
	public GameObject pHand;
	public GameObject tmpDeck;
	public GameObject deck;
	public bool cardsMovedtoTmp = false;
	
	void Start() {
		pHand = GameObject.Find ("Player hand");
		tmpDeck = GameObject.Find ("tmpDeck");
		deck = GameObject.Find ("Deck");
	}
	
	void Update() {
		if(pHand.GetComponent<CardValueMP> ().isServerInitialized) {
			if (!pHand.GetComponent<CardValueMP> ().p2CardsToMove 
			    && !GetComponent<NetworkView>().isMine && !cardsMovedtoTmp) {
				card.transform.SetParent (tmpDeck.transform);
				if (tmpDeck.transform.childCount == 12) {
					cardsMovedtoTmp = true;
				}
			}
		}
	}
}
	