using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Vector3 dragOrigin;

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
            InGameMenuHandler.instance.overMenuBlock.GetComponent<OverMenu>().turnOffMenus();
        }
        if (Input.GetMouseButton(2))
        {

            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
            foreach(GameObject i in TerritoryManager.instance.territoryList)
            {
                i.GetComponent<TerritoryHandler>().territoryStats.container.transform.position -= difference;
            }
            foreach (GameObject i in InGameMenuHandler.instance.listFloatingText)
            {
                i.GetComponent<Transform>().transform.position -= difference;
            }
        }

    }
}
