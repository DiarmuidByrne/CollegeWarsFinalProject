using UnityEngine;
using System.Collections;

public class Multiplayer : MonoBehaviour 
{
	
	///this script is connected to the multiplayer manager
	
	private string titleMessage = "GTGD Series 1 prototype";
	
	private string connectToIP = "127.0.0.1";
	
	private int connectionPort = 26500;
	
	private bool useNAT = false;
	
	private string ipAddress;
	
	private string port;
	
	private int numberOfPlayers = 2;
	
	public string playerName;
	
	public string serverName;
	
	public string serverNameForClient;
	
	private bool iWantToSetUpAServer = false;
	
	private bool iWantToConnectToAServer = false;
	
	// These are used to define the main window
	
	private Rect connectionWindowRect;
	
	private int connectionWindowWidth = 400;
	
	private int connectionWindowHeight = 280;
	
	private int buttonHeight = 60;
	
	private int leftIndent;
	
	private int topIndent;
	
	//server disconnection
	
	private Rect serverDisWindowRect;
	
	private int serverDisWindowWidth = 300;
	
	private int serverDisWindowHeight = 150;
	
	private int serverDisWindowLeftIndent = 10;
	
	private int serverDisWindowTopIndent = 10;
	
	//used for client disconect
	
	private Rect clientDisWindowRect;
	
	private int clientDisWindowWidth = 300;
	
	private int clientDisWindowHeight = 170;
	
	private bool showDisconnectWindow = false;
	
	void Start() 
	{
		//load the last used server name
		serverName = PlayerPrefs.GetString ("serverName");
		
		if (serverName == "") 
		{
			serverName = "Server";
		}
		
		//load last ued player name
		playerName = PlayerPrefs.GetString("playerName");
		if (playerName == "") 
		{
			playerName = "Player";
		}
		
	}
	
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			showDisconnectWindow = !showDisconnectWindow;
		}
	}
	
	void ConnectWindow(int windowID)
	{
		//leave a gap from the header
		
		GUILayout.Space(15);
		
		//when player launches the game/ option to create or join server
		
		if(iWantToSetUpAServer == false && iWantToConnectToAServer == false)
		{
			if(GUILayout.Button("Setup a server", GUILayout.Height(buttonHeight)))
			{
				iWantToSetUpAServer = true;
			}
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Connect to a server", GUILayout.Height(buttonHeight)))
			{
				iWantToConnectToAServer = true;
			}
			
			GUILayout.Space(10);
			
			
			if(Application.isWebPlayer == false && Application.isEditor == false)
			{
				if(GUILayout.Button("Exit", GUILayout.Height(buttonHeight)))
				{
					Application.Quit();
				}
			}
		}
		
		if(iWantToSetUpAServer == true){
			//the user gives the erver a name
			
			GUILayout.Label("Enter server name");
			serverName = GUILayout.TextField(serverName);
			
			GUILayout.Space(5);
			
			//user can enter port number in this text field
			//we gave a default above
			
			//GUILayout.Label("Server Port");
			//connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Create Room", GUILayout.Height(30)))
			{
				Network.InitializeServer(numberOfPlayers, connectionPort, useNAT);
				
				//save the server name
				
				PlayerPrefs.SetString("Rom Name", serverName);
				
				iWantToSetUpAServer = false;
			}
			
			if(GUILayout.Button ("Go Back", GUILayout.Height(30)))
			{
				iWantToSetUpAServer = false;
			}
		}
		
		if(iWantToConnectToAServer == true)
		{
			//user enters their player name
			GUILayout.Label("Enter your player name");
			
			playerName = GUILayout.TextField(playerName);
			
			//GUILayout.Space(5);
			
			//IP address for the server
			//GUILayout.Label("Server IP");
			
			//connectToIP = GUILayout.TextField(connectToIP);
			
			//GUILayout.Space(5);
			
			//enter port number for the server they want to connect to
			//GUILayout.Label("Server Port");
			
			//connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			//GUILayout.Space(5);
			
			//connection button
			
			if(GUILayout.Button("Connect", GUILayout.Height(25)))
			{
				//ensure player cant join empty game.
				
				if(playerName == "")
				{
					playerName = "Player";
				}
				
				if(playerName != "")
				{
					//connect to a server
					//using connectToIP and connectionPort
					
					Network.Connect(connectToIP, connectionPort);
					
					PlayerPrefs.SetString("playerName", playerName);
				}
			}//connect button
			
			//GUILayout.Space(5);
			if(GUILayout.Button("Go Back", GUILayout.Height(25)))
			{
				iWantToConnectToAServer = false;
			}
		}
	}
	
	void serverDisconnectWindow(int windowId)
	{
		GUILayout.Label ("Server Name: " + serverName);
		
		//Number of connected player
		
		GUILayout.Label ("Number of players Connected: " +  Network.connections.Length);
		
		if(Network.connections.Length >= 1){
			GUILayout.Label("Ping: " +Network.GetAveragePing(Network.connections[0]));
		}
		
		if(GUILayout.Button("Disconnect")){
			Network.Disconnect();
		}
	}
	
	
	void clientDisconnectWindow(int windowId)
	{
		GUILayout.Label ("Server Name: " +serverName);
		
		GUILayout.Label ("Ping: " + Network.GetAveragePing(Network.connections[0]));
		
		GUILayout.Space (7);
		
		//player disconnects
		
		if (GUILayout.Button ("Give Up", GUILayout.Height (25))) {
			Network.Disconnect();
		}
		
		GUILayout.Space (5);
		if(GUILayout.Button("Return To Game", GUILayout.Height(25))){
			showDisconnectWindow = false;
		}
		
	}
	
	
	void OnDisconnectedFromServer(){
		//if the player disconnects from server return to wlcome screen
		Application.LoadLevel ("opening");
		//Application.LoadLevel (Application.LoadLevel);
	}
	
	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		//when the player leaves the server delete them from
		//from the network along with their rpc so other players dont see them
		
		Network.RemoveRPCs (networkPlayer);
		
		Network.DestroyPlayerObjects (networkPlayer);
	}
	
	void OnPlayerConnected(NetworkPlayer networkPlayer)
	{
		GetComponent<NetworkView>().RPC ("TellPlayerServerName", networkPlayer, serverName);
	}
	
	void OnGUI()
	{
		//if the player is disconnect run this code
		
		if(Network.peerType == NetworkPeerType.Disconnected)
		{
			leftIndent = (Screen.width/2) - connectionWindowWidth / 2;
			topIndent = (Screen.height/2) - connectionWindowHeight / 2;
			
			connectionWindowRect = new Rect (leftIndent, topIndent, connectionWindowWidth,
			                                 connectionWindowHeight);
			
			connectionWindowRect = GUILayout.Window(0, connectionWindowRect, ConnectWindow, titleMessage);
		}
		
		if(Network.peerType == NetworkPeerType.Server)
		{
			serverDisWindowRect = new Rect(serverDisWindowLeftIndent, serverDisWindowTopIndent, serverDisWindowWidth, serverDisWindowHeight);
			serverDisWindowRect = GUILayout.Window (1, serverDisWindowRect, serverDisconnectWindow, "");
		}
		
		if(Network.peerType == NetworkPeerType.Client && showDisconnectWindow == true)
		{
			clientDisWindowRect = new Rect (Screen.width/2 - clientDisWindowWidth /2,
			                                Screen.height/2 - clientDisWindowHeight /2,
			                                clientDisWindowWidth, clientDisWindowHeight);
			
			clientDisWindowRect = GUI.Window(1, clientDisWindowRect, clientDisconnectWindow, "");
		}
	}
	
	[RPC]
	void TellPlayerServerName(string servername)
	{
		serverName = servername;
	}
	
}