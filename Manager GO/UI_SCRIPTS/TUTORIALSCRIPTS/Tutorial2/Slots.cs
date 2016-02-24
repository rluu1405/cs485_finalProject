//in each slots
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
//using UnityEngine.UI;

public class Slots : MonoBehaviour, IDropHandler
{

	public GameObject item
	{
		get
		{
			if(transform.childCount > 0)
			{
				return transform.GetChild(0).gameObject;
			}
			return null;
		}
	}
	
	public void OnDrop (PointerEventData eventData)
	{
		if(!item)
		{
			DragHandler.itemBeginDragged.transform.SetParent (transform);
			//ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
		}
		
	}
	
}
