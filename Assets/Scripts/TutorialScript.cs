using UnityEngine;
using System.Collections;

public class TutorialScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// If player has never played before, or user has tutorial enabled
		if(!PlayerPrefs.HasKey("TimeaPlayed")){
			PlayerPrefs.SetInt("TimesPlayed", 0);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
