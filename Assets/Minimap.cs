using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Minimap : MonoBehaviour
{
	//attach this script to the camera

	[SerializeField]
	Camera cam; //for raycast instead of using Camera.main
	[SerializeField]
	GameObject camToMove; // the gameobject the camera is attached to
	Ray ray;
	private Vector3 origin;
	public float speed = 5.0f;
	void Start()
	{
		if (cam == null)
		{
			cam = GetComponent<Camera>();
		}
		
	}
	void Update()
	{
		origin = cam.transform.position;

		//to move camera :
		if (IspointerOverUiObject())
		{
			if (Input.GetMouseButtonDown(2))
			{
				//pending
			}
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			cam.transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			cam.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			cam.transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			cam.transform.position += new Vector3(0, speed * Time.deltaTime, 0);
		}
		Vector3 difference = origin - cam.transform.position;
		foreach (GameObject i in TerritoryManager.instance.territoryList)
		{
			i.GetComponent<TerritoryHandler>().territoryStats.container.transform.position += difference;
		}
		foreach (GameObject i in InGameMenuHandler.instance.listFloatingText)
		{
			i.GetComponent<Transform>().transform.position += difference;
		}
	}


	//this function dectects clicks on ui objects
	private bool IspointerOverUiObject()
	{
		PointerEventData EventDataCurrentPosition = new PointerEventData(EventSystem.current);
		EventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> result = new List<RaycastResult>();
		EventSystem.current.RaycastAll(EventDataCurrentPosition, result);
		return result.Count > 0;
	}
}
