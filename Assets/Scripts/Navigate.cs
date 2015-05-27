using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Navigate : MonoBehaviour 
{
	GameObject quitPanel;
	GameObject tutFadePanel;
	//GameObject tutorialPanel;
	bool quitMenuActive;

	void Start(){
		quitPanel = GameObject.Find ("QuitPanel");
		tutFadePanel = GameObject.Find ("TutFadePanel");
		//tutorialPanel = tutFadePanel.transform.FindChild ("TutorialPanel").gameObject;
		quitMenuActive = false;

	}

	public void startButtonClick(int index)
	{
		Application.LoadLevel(index);
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(quitMenuActive) {
				cancelMainMenu();
			}
			else {
				quitMainMenu();
			}
		}
	}
	
	public void closeTutorial(){
		//tutorialPanel.transform.GetComponent<CanvasGroup> ().alpha = 0;
		//tutorialPanel.transform.GetComponent<CanvasGroup> ().interactable = false;
		//tutorialPanel.transform.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		tutFadePanel.GetComponent<CanvasGroup> ().alpha = 0;
		tutFadePanel.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		tutFadePanel.GetComponent<CanvasGroup> ().interactable = false;
	}

	public void cancelMainMenu(){
		quitPanel.transform.GetComponent<CanvasGroup> ().alpha = 0;
		quitPanel.transform.GetComponent<CanvasGroup> ().interactable = false;
		quitPanel.transform.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		quitMenuActive = false;
	}

	public void quitMainMenu(){
		quitPanel.transform.GetComponent<CanvasGroup> ().alpha = 1;
		quitPanel.transform.GetComponent<CanvasGroup> ().interactable = true;
		quitPanel.transform.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		quitMenuActive = true;
	}
	

	public void LoadBoardScene(string levelName)
	{
		Application.LoadLevel("boardScene");
	}

	public void LoadInstructionsScene() {
		Application.LoadLevel ("instructions");
	}

	public void LoadScoreScene() {
		Application.LoadLevel ("scoreScene");
	}

	public void LoadMenuScene(string levelName)
	{
		Application.LoadLevel("opening");
	}

	public void QuitMultiplayer(string levelName)
	{
		Network.Disconnect ();
		Application.LoadLevel("opening");
	}

	public void LoadNetworkScene()
	{
		Application.LoadLevel("multiplayerScene");
	}

}
