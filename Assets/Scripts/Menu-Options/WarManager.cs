using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarManager : MonoBehaviour
{
    private bool status;
    public TerritoryHandler selected;
    private War selectedWar;
    public static WarManager instance;
    public List<War> warList = new List<War>();

    [SerializeField] private Text warriorsCount1;
    [SerializeField] private Text warriorsCount2;
    [SerializeField] private Image territorySprite;
    [SerializeField] private Text title;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        status = false;
    }
    public void AddWar(int c1, int c2, float s1, float s2, TerritoryHandler ta, Territory.TYPEPLAYER a)
    {
        War w = new War(c1, s1, s2,ta, a);
        warList.Add(w);
    }
    private void Update()
    {
        foreach(War w in warList)
        {
            w.UpdateStatus(Time.deltaTime);
        }
        if(status)
        {
            warriorsCount1.text = selectedWar.warriors1Count.ToString();
            warriorsCount2.text = selectedWar.warriors2Count.ToString();
        }

    }
    public void SetWarStatus(bool t)
    {
        if (t)
        {
            foreach (War w in warList)
            {
                if(w.GetTerritory() == selected)
                {
                    title.text = "Batalla de " + selected.territory.name;
                    territorySprite.sprite = selected.sprite.sprite;
                    selectedWar = w;
                }
            }
        }
        status = t;
    }

    public void FinishWar(TerritoryHandler territory, Territory.TYPEPLAYER type)
    {
        territory.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        territory.war = false; 

    }

}
