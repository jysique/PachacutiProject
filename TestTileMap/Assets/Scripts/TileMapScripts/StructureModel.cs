using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModel : MonoBehaviour
{
    float yHeight = 0;

    public void CreateModel(GameObject model)
    {

        var structure = Instantiate(model, transform);
        yHeight = structure.transform.position.z;
    }
    public void SwapModel(GameObject model, Quaternion rotation)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        var structure = Instantiate(model, transform);
        structure.transform.localPosition = new Vector3(0, 0, yHeight);
        structure.transform.localRotation = rotation;
    }
}
