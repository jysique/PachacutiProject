using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Minimap : MonoBehaviour, IPointerClickHandler
{
	//attach this script to the camera

	[SerializeField] Camera cam;
	[SerializeField] Camera mini;
	private Ray ray;
	private Vector3 origin;
	private Vector3 center;
	private float speedMovement = 5.0f;
	RectTransform rt;
	void Start()
	{
		if (cam == null)
		{
			cam = GetComponent<Camera>();
		}
		rt = GetComponent<RectTransform>();
	}
	void Update()
	{
		origin = cam.transform.position;
		MoveCameraWithKeys();
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		MoveCameraInMinimap();
	}
	private void GetCenterMinimap()
	{
		Vector3[] v = new Vector3[4];
		rt.GetWorldCorners(v);
		center = new Vector3((v[1].x + v[2].x) / 2, (v[0].y + v[1].y) / 2, cam.transform.position.z);
	}
	private void MoveCameraInMinimap()
    {
		GetCenterMinimap();
		ray = cam.ScreenPointToRay(Input.mousePosition);
		Vector3 dif = new Vector3((ray.origin.x - center.x) * 30, (ray.origin.y - center.y) * 30 + 5.0f, -10);
		cam.transform.position = dif;
		UpdateGameobjectsInCamera();
	}
	private void MoveCameraWithKeys()
    {
		if (Input.GetKey(KeyCode.RightArrow))
		{
			cam.transform.position += new Vector3(speedMovement * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			cam.transform.position -= new Vector3(speedMovement * Time.deltaTime, 0, 0);
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			cam.transform.position -= new Vector3(0, speedMovement * Time.deltaTime, 0);
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			cam.transform.position += new Vector3(0, speedMovement * Time.deltaTime, 0);
		}
		UpdateGameobjectsInCamera();
	}
	private void UpdateGameobjectsInCamera()
	{
		Vector3 difference = origin - cam.transform.position;
		/*
		foreach (GameObject i in TerritoryManager.instance.territoryList)
		{
			//i.GetComponent<TerritoryHandler>().TerritoryStats.container.transform.position += difference;
		}
		*/
		foreach (GameObject i in InGameMenuHandler.instance.listFloatingText)
		{
			i.GetComponent<Transform>().transform.position += difference;
		}
	}
}
