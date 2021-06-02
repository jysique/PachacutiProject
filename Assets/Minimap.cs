using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Minimap : MonoBehaviour
{
	//attach this script to the camera

	[SerializeField] Camera cam; 
	[SerializeField] Camera mini;
	private Ray ray;
	private Vector3 origin;
	private float speed = 5.0f;
	RectTransform rt;
	void Start()
	{
		if (cam == null)
		{
			cam = GetComponent<Camera>();
		}
		rt = GetComponent<RectTransform>();
	}
	Vector3 center;

	void GetCenterMinimap()
	{
		Vector3[] v = new Vector3[4];
		rt.GetWorldCorners(v);
		center = new Vector3((v[1].x + v[2].x)/2,(v[0].y + v[1].y)/2,cam.transform.position.z);
	}

	void Update()
	{
		origin = cam.transform.position;
		MoveCameraInMiniMap();		
		CameraMovementKeys();
		UpdateGameobjectsInCamera();
	}
	private void MoveCameraInMiniMap()
    {
		if (IspointerOverUiObject())
		{
			if (Input.GetMouseButtonDown(0))
			{
				GetCenterMinimap();
				ray = cam.ScreenPointToRay(Input.mousePosition);
				Vector3 dif = new Vector3((ray.origin.x - center.x) * 30 , (ray.origin.y - center.y) * 30 + 5.0f, -10);
				//print("c|" + center);
				//print("d|" + dif);
				cam.transform.position = dif;
			}
		}
	}
	private void UpdateGameobjectsInCamera()
    {
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
	private void CameraMovementKeys()
    {
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
