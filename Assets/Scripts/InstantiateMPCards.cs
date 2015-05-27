using UnityEngine;
using System.Collections;

public class InstantiateMPCards : MonoBehaviour {
	public GameObject card;
	public GameObject pHand, oHand, pBoard, oBoard;
	public GameObject deck, tmpDeck;
	public string cardParent;
	public GameObject canvas;
	// Use this for initialization
	void Start () {
		pHand = GameObject.Find ("Player hand");
		oHand = GameObject.Find ("Opposition hand");
		pBoard = GameObject.Find ("Player board");
		oBoard = GameObject.Find ("Opposition board");
		deck = GameObject.Find ("Deck");
		tmpDeck = GameObject.Find ("tmpDeck");
		canvas = GameObject.Find ("Canvas");
		cardParent= "Canvas";
	}
}
