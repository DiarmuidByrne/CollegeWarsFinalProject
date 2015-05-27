using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour {
	
	private const string typeName = "CollegeWars";
	private string gameName = "RoomName";
	private HostData[] hostList;
	public GameObject tmpDeck;
	public GameObject awaitingPlayerPanel, awaitingPlayerText;
	public GameObject deck;
	public GameObject pHand;
	private bool awaitingPlayer = false;
	private float timeLimit = 4.5f;
	private bool textAdded1, textAdded2, textAdded3;
	
	public float smooth = 10;
	private Color newColour;
	
	public int indexCard;
	public int minimum = 0;
	public int maximum;
	
	public List<GameObject> mainDeck = new List<GameObject>();
	
	void Start() {
		awaitingPlayerPanel.SetActive (false);
		textAdded1 = textAdded2 = textAdded3 = false;
	}
	
	// Use this for initialization
	void StartServer () {
		Network.InitializeServer (2, 25000, !Network.HavePublicAddress ());
		MasterServer.RegisterHost (typeName, gameName);
	}

	// Await second player to join
	void OnServerInitialized() {
		awaitingPlayerPanel.SetActive (true);
		awaitingPlayer = true;
	}
	
	void Update() {
		if(awaitingPlayer) {
			displayAwaitingPanel();
		}
	}

	// Display awaiting player text. Refresh text every few seconds
	void displayAwaitingPanel() { 
		if(timeLimit > 0f) {
			// Decrease timeLimit.
			timeLimit -= Time.deltaTime;
			if(timeLimit > 3f  && timeLimit < 4.5f && textAdded1 == false) {
				awaitingPlayerText.GetComponent<Text> ().text = "Waiting for \nPlayer to join.";
				textAdded1 = true;
			} 
			else if (timeLimit > 1.5f  && timeLimit < 3f && textAdded2 == false) {
				awaitingPlayerText.GetComponent<Text> ().text += ".";
				textAdded2 = true;
			}
			else if (timeLimit > 0f  && timeLimit < 1.5f && textAdded3 == false) {
				awaitingPlayerText.GetComponent<Text> ().text += ".";
				textAdded3 = true;
			}
		} else {
			timeLimit = 4.5f;
			textAdded1 = textAdded2 = textAdded3 = false;
		}
	}
	
	void moveCardsToPlayer(){
		// Move first 5 cards from deck to player's hand. These cards are already randomized.
		for (int i = 0; i < 5; i++) {
			deck.transform.GetChild (0).SetParent (pHand.transform);
			pHand.transform.GetChild(i).gameObject.AddComponent<DragNDrop>();
		}
	}
	
	void OnGUI(){
		if (!Network.isClient && !Network.isServer)
		{
		gameName = GUI.TextField(new Rect(100, 70, 250, 30), gameName, 25);

			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
				StartServer();
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
				RefreshHostList();
			
			if (hostList != null)
			{
				for (int i = 0; i < hostList.Length; i++)
				{
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
						JoinServer(hostList[i]);
				}
			}
		}
	}
	
	private void RefreshHostList()
	{
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}
	// Join the server
	private void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	
	[RPC] void setPlayerParents() {
		foreach (Transform child in pHand.transform) {
			child.parent = child.parent;
		}
	}
	
	// Initialize when connected to server
	void OnConnectedToServer()
	{
		GetComponent<NetworkView> ().RPC ("RemoveAwaitPlayersPanel", RPCMode.OthersBuffered);
	}
	
	[RPC]
	void RemoveAwaitPlayersPanel() {
		awaitingPlayerPanel.SetActive (false);
	}
	
	void moveCardsToOpposition(){
		// Move first 5 cards from deck to player's hand. These cards are already randomized.
		for (int i = 0; i < 5; i++) {
			deck.transform.GetChild (0).SetParent (pHand.transform);
			pHand.transform.GetChild(i).gameObject.AddComponent<DragNDrop>();
		}
	}
}
