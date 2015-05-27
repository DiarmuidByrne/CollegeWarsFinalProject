using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Boomlagoon.JSON;


public class cardValue : CardHitValue
{
//	public List<JSONObject> cardLects = new List<JSONObject>();

	public int indexCard;
	public int minimum = 0;
	public int maximum;
	public GameObject tmpDeck;
	private GameObject loadingPanel;
	public List<GameObject> mainDeck = new List<GameObject>();
	public GameObject deck;
	public GameObject pHand;
	private float turnSpeed = 600f;
	private bool rotatedOnce = false;
	public GameObject imageList;

	// Use this for initialization
	void Start () {
		// Initialize variables and call method to shuffle cards

		instantiateDeck ();
		// Loop through each card in tmpDeck before they're shuffled.
		// Adds image associated with each GameObject along with the accompanying GameObject HashCode to a Dictionary (oDictImages)
		// This HashCode is then used as a key to retrieve the image in future
		int i;
		for(i=0; i<tmpDeck.transform.childCount; i++){
			if(!imageList.GetComponent<ImageListScript>().dictCardImages.ContainsKey(tmpDeck.transform.GetChild (i).GetHashCode())) {
				imageList.GetComponent<ImageListScript>().dictCardImages.Add(tmpDeck.transform.GetChild (i).GetHashCode(), tmpDeck.transform.GetChild (i).GetComponent<Image>().sprite);
			}
		}
		addJSONCards ();
	}
	
	// Update is called once per frame
	void Update () {
		if (tmpDeck.transform.childCount > 0) {
			shuffleDeck ();
		} 
		if (pHand.transform.childCount == 5) {
			foreach (Transform child in pHand.transform) {
				if (child.transform.rotation.y > .1f) {
					rotatedOnce = true;
					child.rotation.Set(0f,1f,0f,0f);
				}
				else if (!rotatedOnce) {
					child.Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
				}
			}
		}
	}

	[RPC]
	void shuffleDeck(){
		maximum = mainDeck.Count;
		indexCard = Random.Range(minimum, maximum); 
		if(mainDeck[indexCard] != null) 
		{
			mainDeck[indexCard].transform.SetParent(deck.transform);
			mainDeck[indexCard] = null;
		}
	}

	// Instantiate each card
	void instantiateDeck(){
		GameObject tmp1 = Instantiate (Resources.Load ("lecturer1", typeof(GameObject))) as GameObject;
		tmp1.transform.SetParent (tmpDeck.transform, false);
		mainDeck.Add (tmp1);

		GameObject tmp2 = Instantiate (Resources.Load ("lecturer2", typeof(GameObject))) as GameObject;
		tmp2.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp2);

		GameObject tmp3 = Instantiate (Resources.Load ("lecturer3", typeof(GameObject))) as GameObject;
		tmp3.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp3);

		GameObject tmp4 = Instantiate (Resources.Load ("lecturer4", typeof(GameObject))) as GameObject;
		tmp4.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp4);

		GameObject tmp5 = Instantiate (Resources.Load ("lecturer5", typeof(GameObject))) as GameObject;
		tmp5.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp5);

		GameObject tmp6 = Instantiate (Resources.Load ("lecturer6", typeof(GameObject))) as GameObject;
		tmp6.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp6);
	
		GameObject tmp7 = Instantiate (Resources.Load ("lecturer7", typeof(GameObject))) as GameObject;
		tmp7.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp7);

		GameObject tmp8 = Instantiate (Resources.Load ("lecturer8", typeof(GameObject))) as GameObject;
		tmp8.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp8);

		GameObject tmp9 = Instantiate (Resources.Load ("student1", typeof(GameObject))) as GameObject;
		tmp9.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp9);

		GameObject tmp10 = Instantiate (Resources.Load ("student2", typeof(GameObject))) as GameObject;
		tmp10.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp10);

		GameObject tmp11 = Instantiate (Resources.Load ("student3", typeof(GameObject))) as GameObject;
		tmp11.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp11);

		GameObject tmp12 = Instantiate (Resources.Load ("student4", typeof(GameObject))) as GameObject;
		tmp12.transform.SetParent (tmpDeck.transform, worldPositionStays: false);
		mainDeck.Add (tmp12);
	}

	void addJSONCards(){
		int i = 0;
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

		foreach (Transform child in tmpDeck.transform) {
			child.gameObject.AddComponent <CardStats>();
			child.gameObject.GetComponent<CardStats> ().attack = (cardLects [i].GetNumber ("attack"));
			child.gameObject.GetComponent<CardStats> ().defence = (cardLects [i].GetNumber ("defence"));
			i++;
		}
	}
}