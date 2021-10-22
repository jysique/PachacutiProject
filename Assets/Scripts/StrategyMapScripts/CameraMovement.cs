using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 dragOrigin;
    private float camSpeed = 2;
    public float zoom;
    private float minZoom = 2;
    private float maxZoom = 6;
    private void Update()
    {
        PanCamera();
    }
    /// <summary>
    /// Behavior of mouse button in camera
    /// </summary>
    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            MenuManager.instance.overMenuBlock.GetComponent<OverMenu>().turnOffMenus();
        }
        if (Input.GetMouseButton(2))
        {
            DragCamera();
        }
        /*
        if (cam.orthographicSize < 5) 
        {
            cam.orthographicSize = 5;
        }
        else
        {
            cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
        }
        */
        Zoom();

    }
    void DragCamera()
    {
        Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
        cam.transform.position += difference;
        /*
        foreach (GameObject i in TerritoryManager.instance.territoryList)
        {
            //i.GetComponent<TerritoryHandler>().TerritoryStats.container.transform.position -= difference;
        }
        */
        foreach (GameObject i in InGameMenuHandler.instance.listFloatingText)
        {
            i.GetComponent<Transform>().transform.position -= difference;
        }
    }
    void Zoom()
    {
        cam.orthographicSize = zoom + 3;
        if (Input.mouseScrollDelta.y > 0)
        {
            zoom -= camSpeed * Time.deltaTime * 10;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom += camSpeed * Time.deltaTime * 10;
        }

        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        /*
        foreach (GameObject i in TerritoryManager.instance.territoryList)
        {
            //i.GetComponent<TerritoryHandler>().TerritoryStats.container.transform.localScale = new Vector3(zoom / 2, zoom / 2, 1);
        }
        */
      //  DragCamera();
    }
}
