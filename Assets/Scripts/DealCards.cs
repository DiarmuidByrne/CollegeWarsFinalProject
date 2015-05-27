using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DealCards : MonoBehaviour {
	public GameObject tmpDeck;
	private bool cardsMoved = false;
	public GameObject deck;
	public GameObject pHand;
	public GameObject oHand;

	// Update is called once per frame
	void Update () {
		if (tmpDeck.transform.childCount <= 0) {
			if (!cardsMoved) {
				moveCardsToPlayer ();
				moveCardsToOpposition ();
			}
		}
	}

	void moveCardsToPlayer(){
		// Move first 5 cards from deck to player's hand. These cards are already randomized.
		for (int i = 0; i < 5; i++) {
			deck.transform.GetChild (0).SetParent (pHand.transform);
			pHand.transform.GetChild(i).gameObject.AddComponent<DragNDrop>();
			cardsMoved = true;
		}
	}
	
	void moveCardsToOpposition(){
		// Move first 5 cards from deck to opposition's hand. These cards are already randomized.
		for (int i = 0; i < 5; i++) {
			deck.transform.GetChild (0).SetParent (oHand.transform);
			oHand.transform.GetChild(i).GetComponent<Image>().sprite = Resources.Load("CardBack", typeof(Sprite)) as Sprite;
		}
	}
}
