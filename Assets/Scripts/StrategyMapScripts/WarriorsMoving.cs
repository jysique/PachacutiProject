using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarriorsMoving : MonoBehaviour
{
    //prefab variables
    [SerializeField]private GameObject target;
    [SerializeField]private float speed;
    //stats
    [SerializeField]private int warriorsNumber;
    [SerializeField]private Territory.TYPEPLAYER territorytype;
    private MilitarBoss mb;
    private MilitarBoss.TYPESTRAT militaryBossStrategy;
    private int militaryBossExperience;
    //result
    [SerializeField] private float attackPower;


    public void SetAttack(GameObject _target, float _speed, int _warriorsNumber, Territory.TYPEPLAYER _type, MilitarBoss _mb)
    {
        
        //float strategyMod = 0;
        target = _target;
        speed = _speed;
        warriorsNumber = _warriorsNumber;
        territorytype = _type;
        mb = _mb;
        
        /*
        militaryBossStrategy = _mb.Type;
        militaryBossExperience = _mb.Experience;
        
        switch (militaryBossStrategy)
        {
            case MilitarBoss.TYPESTRAT.AGGRESSIVE:
                strategyMod = warriorsNumber * 0.1f;
                break;
            case MilitarBoss.TYPESTRAT.TERRAIN_MASTER:
                strategyMod = warriorsNumber * 0.06f;
                break;
            case MilitarBoss.TYPESTRAT.DEFENSIVE:
                strategyMod = warriorsNumber * -0.02f;
                break;
            case MilitarBoss.TYPESTRAT.SACRED_WARRIOR:
                strategyMod = warriorsNumber * 0.2f;
                break;
            case MilitarBoss.TYPESTRAT.SIEGE_EXPERT:
                strategyMod = warriorsNumber * 0.12f;
                break;

        }
        attackPower = warriorsNumber + strategyMod + militaryBossExperience;
        this.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = attackPower.ToString("F2");
        */
        this.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = warriorsNumber.ToString();
     //   print(territorytype);

    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
//        print(territorytype);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
        {
 //           print(territorytype);
            InGameMenuHandler.instance.MoveWarriors(target.GetComponent<TerritoryHandler>(), warriorsNumber, territorytype);
            Destroy(this.gameObject);
        }
        
    }
}
