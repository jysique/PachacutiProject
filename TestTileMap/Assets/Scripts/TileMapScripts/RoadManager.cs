using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public PlacementManager placementManager;
    public List<Vector3Int> temporaryPlacementPositions = new List<Vector3Int>();
    public List<Vector3Int> roadPositionToRecheck = new List<Vector3Int>();
    public GameObject roadStraight;
    private RoadFixer roadFixer;
    private void Start()
    {
        roadFixer = GetComponent<RoadFixer>();
    }
    public void PlaceRoad(Vector3Int position)
    {
        if (placementManager.CheckIfPositionInBound(position) == false)
        {
            return;
        }
        if (placementManager.CheckIfPositionInFree(position) == false)
        {
            return;
        }
        temporaryPlacementPositions.Clear();
        temporaryPlacementPositions.Add(position);
        placementManager.PlaceTemporaryStructure(position, roadStraight, CellType.Road);
        FixRoadPrefabs();

    }

    private void FixRoadPrefabs()
    {
        foreach (var temporaryPosition in temporaryPlacementPositions)
        {
            roadFixer.FixRoadAtPosition(placementManager, temporaryPosition);
            var neighbours = placementManager.GetNeighbourOfTypesFor(temporaryPosition, CellType.Road);
            foreach (var roadPosition in neighbours)
            {
                roadPositionToRecheck.Add(roadPosition);
            }
        }
        foreach (var positionToFix in roadPositionToRecheck)
        {
            roadFixer.FixRoadAtPosition(placementManager, positionToFix);
        }
    }
}
