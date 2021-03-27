using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorsMoving : MonoBehaviour
{
    public GameObject target;
    public float speed;
    public int warriorsNumber;
    public Territory.TYPEPLAYER type;

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);


        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
        {
            InGameMenuHandler.instance.MoveWarriors(target.GetComponent<TerritoryHandler>(), warriorsNumber, type);
            Destroy(this.gameObject);
        }
        
    }
}
