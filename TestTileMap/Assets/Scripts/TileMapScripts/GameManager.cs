using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVS;
using System;

public class GameManager : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public InputManager inputManager;
    public RoadManager roadManager;
    private void Start()
    {
        inputManager.OnMouseClick += HandleMouseClick;
    }

    private void HandleMouseClick(Vector3Int position)
    {
        Debug.Log(position);
        roadManager.PlaceRoad(position);
    }

    private void Update()
    {
        cameraMovement.MoveCamera(new Vector2(inputManager.CameraMovementVector.x, inputManager.CameraMovementVector.y));
    }
}
