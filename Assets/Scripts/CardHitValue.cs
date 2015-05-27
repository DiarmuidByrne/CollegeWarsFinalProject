using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

//public class Card {
//	public string Name;
//	public double Attack, Defence;
//
//	public Card() {
//		Name = "lecturer1";
//		Attack = 62;
//		Defence = 70;
//	}
//
//	public getCard(string name) {
//		ame = 
//	}
//}

public class CardHitValue : MonoBehaviour {
	public List<JSONObject> cardLects = new List<JSONObject>();


	public 	JSONObject cardLect1 = new JSONObject {
		{"name", "lecturer1"},
		{"attack", 62},
		{"defence", 70},
	}; 
	public JSONObject cardLect2 = new JSONObject {
		{"name", "lecturer2"},
		{"attack", 43},
		{"defence", 85},
	};
	public JSONObject cardLect3 = new JSONObject {
		{"name", "lecturer3"},
		{"attack", 50},
		{"defence", 82},
	};
	public JSONObject cardLect4 = new JSONObject {
		{"name", "lecturer4"},
		{"attack", 78},
		{"defence", 37},
	};
	public JSONObject cardLect5 = new JSONObject {
		{"name", "lecturer5"},
		{"attack", 75},
		{"defence", 70},
	};
	public JSONObject cardLect6 = new JSONObject {
		{"name", "lecturer6"},
		{"attack", 45},
		{"defence", 90},
	};
	public JSONObject cardLect7 = new JSONObject {
		{"name", "lecturer7"},
		{"attack", 71},
		{"defence", 49},
	};
	public JSONObject cardLect8 = new JSONObject {
		{"name", "lecturer8"},
		{"attack", 14},
		{"defence", 95},
	};
	public JSONObject cardStud1 = new JSONObject {
		{"name", "student1"},
		{"attack", 95},
		{"defence", 62},
	};
	public JSONObject cardStud2 = new JSONObject {
		{"name", "student2"},
		{"attack", 72},
		{"defence", 64},
	};
	public JSONObject cardStud3 = new JSONObject {
		{"name", "student3"},
		{"attack", 80},
		{"defence", 62},
	};
	public JSONObject cardStud4 = new JSONObject {
		{"name", "student4"},
		{"attack", 38},
		{"defence", 79},
	};
}
