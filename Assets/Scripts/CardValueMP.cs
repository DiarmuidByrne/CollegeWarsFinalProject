using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Boomlagoon.JSON;

public class CardValueMP : CardHitValue {
	
	public int indexCard;
	public int minimum = 0;
	public int maximum;
	public GameObject tmpDeck;
	public bool cardsMoved = false;
	private GameObject loadingPanel;
	public List<GameObject> mainDeck = new List<GameObject>();
	public new List<JSONObject> cardLects = new List<JSONObject>();
	public bool p2CardsToMove = false;
	public bool isServerInitialized = false;
	public GameObject deck;
	public GameObject pHand;
	public GameObject oHand;
	public GameObject oBoard;
	public GameObject imageList;

	// Use this for initialization
	void OnServerInitialized() {
		bool serverInit = true;
		GetComponent<NetworkView>().RPC ("SetServerInitialized", RPCMode.AllBuffered, serverInit);
		GetComponent<NetworkView>().RPC ("InstantiateDeck", RPCMode.AllBuffered);
		GetComponent<NetworkView>().RPC ("PopulateImageList", RPCMode.AllBuffered);
	}

	[RPC]
	void PopulateImageList() {
		// Loop through each card in tmpDeck before they're shuffled.
		// Adds image associated with each GameObject along with the accompanying GameObject HashCode to a Dictionary (oDictImages)
		// This HashCode is then used as a key to retrieve the image in future
		int i;
		for(i=0; i<tmpDeck.transform.childCount; i++){
			if(!imageList.GetComponent<ImageListScript>().dictCardImages.ContainsKey(tmpDeck.transform.GetChild (i).GetHashCode())) {
				imageList.GetComponent<ImageListScript>().dictCardImages.Add(tmpDeck.transform.GetChild (i).GetHashCode(), tmpDeck.transform.GetChild (i).GetComponent<Image>().sprite);
			}
		}
	}

	[RPC]
	void SetServerInitialized(bool init) {
		isServerInitialized = init;
	}

	// Instantiate each card
	[RPC]
	void InstantiateDeck(){
		if (GetComponent<NetworkView>().isMine) {

			GameObject tmp1 = Network.Instantiate (Resources.Load ("lecturer1", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp1.transform.SetParent (tmpDeck.transform, false);
			mainDeck.Add (tmp1);
		
			GameObject tmp2 = Network.Instantiate (Resources.Load ("lecturer2", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp2.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp2);

			GameObject tmp3 = Network.Instantiate (Resources.Load ("lecturer3", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp3.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp3);

			GameObject tmp4 = Network.Instantiate (Resources.Load ("lecturer4", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp4.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp4);
		
			GameObject tmp5 = Network.Instantiate (Resources.Load ("lecturer5", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp5.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp5);
		
			GameObject tmp6 = Network.Instantiate (Resources.Load ("lecturer6", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp6.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp6);
		
			GameObject tmp7 = Network.Instantiate (Resources.Load ("lecturer7", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp7.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp7);
		
			GameObject tmp8 = Network.Instantiate (Resources.Load ("lecturer8", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp8.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp8);
		
			GameObject tmp9 = Network.Instantiate (Resources.Load ("student1", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp9.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp9);
		
			GameObject tmp10 = Network.Instantiate (Resources.Load ("student2", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp10.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp10);
		
			GameObject tmp11 = Network.Instantiate (Resources.Load ("student3", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp11.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp11);

			GameObject tmp12 = Network.Instantiate (Resources.Load ("student4", typeof(GameObject)), tmpDeck.transform.position, tmpDeck.transform.rotation, 0) as GameObject;
			tmp12.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
			mainDeck.Add (tmp12);

			p2CardsToMove = true;
		}
	}
}