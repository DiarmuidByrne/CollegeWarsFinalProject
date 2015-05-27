using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler {

	//GameObject pBoard;
	public int pChildCount;
	public string lastDropped;

	public void OnDrop(PointerEventData eventData) {
		lastDropped = eventData.pointerDrag.name;
		DragNDrop d = eventData.pointerDrag.GetComponent<DragNDrop>();
		if(d != null) 
		{
			d.parentToReturnTo = this.transform;
		}
	}
}
