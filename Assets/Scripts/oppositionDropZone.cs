using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class oppositionDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
	
	public int oChildCount;
	public string lastDropped;
	
	void Start () {
	//	pBoard = GameObject.Find ("Player board");
	}

	public void OnPointerEnter(PointerEventData eventData) {
		//Debug.Log("OnPointerEnter");
	}
	
	public void OnPointerExit(PointerEventData eventData) {
		//Debug.Log("OnPointerExit");
	}
	
	public void OnDrop(PointerEventData eventData) {
		Debug.Log (eventData.pointerDrag.name + " was dropped on " + gameObject.name);
		lastDropped = eventData.pointerDrag.name;
		DragNDropOpposition d = eventData.pointerDrag.GetComponent<DragNDropOpposition>();
		if(d != null) 
		{
			d.parentToReturnTo = this.transform;
		}
	}
}
