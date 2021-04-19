using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarManager : MonoBehaviour
{
    private bool status;
    public TerritoryHandler selected;
    public War selectedWar;
    public static WarManager instance;
    public List<War> warList = new List<War>();
    [SerializeField] private GameObject warContainer;
    [SerializeField] private GameObject peaceContainer;
    [SerializeField] private Text warriorsCount1;
    [SerializeField] private Text warriorsCount2;
    [SerializeField] private Image territorySprite;
    [SerializeField] private Text title;
    //hats
    [SerializeField] private Sprite incaHat;
    [SerializeField] private Sprite chancaHat;
    [SerializeField] private Sprite chancaHat2;
    //
    [SerializeField] private Image hatAttacker;
    [SerializeField] private Image hatDefender;
    [SerializeField] private Image hatP1;
    [SerializeField] private Image hatP2;
    [SerializeField] private Image hatP3;


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
            SetWarMenu();
        }
        else
        {
            SetPeaceMenu();
        }
        status = t;
    }

    private void SetWarMenu()
    {
        foreach (War w in warList)
        {
            if (w.GetTerritory() == selected)
            {
                //hats
                switch (w.Attackers)
                {
                    case Territory.TYPEPLAYER.PLAYER:
                        hatAttacker.sprite = incaHat;
                        break;
                    case Territory.TYPEPLAYER.NONE:
                        hatAttacker.sprite = null;
                        break;
                    case Territory.TYPEPLAYER.BOT:
                        hatAttacker.sprite = chancaHat;
                        break;
                }
                switch (w.Territory.territory.TypePlayer)
                {
                    case Territory.TYPEPLAYER.PLAYER:
                        hatDefender.sprite = incaHat;
                        break;
                    case Territory.TYPEPLAYER.NONE:
                        hatDefender.sprite = null;
                        break;
                    case Territory.TYPEPLAYER.BOT:
                        hatDefender.sprite = chancaHat;
                        break;
                }
                warContainer.SetActive(true);
                title.text = "Batalla de " + selected.territory.name;
                territorySprite.sprite = selected.sprite.sprite;
                selectedWar = w;
                peaceContainer.SetActive(false);
            }
        }
        
    }
    
    private void SetPeaceMenu()
    {
        switch (selected.territory.TypePlayer)
        {
            case Territory.TYPEPLAYER.PLAYER:
                hatP1.sprite = incaHat;
                hatP2.sprite = incaHat;
                hatP3.sprite = incaHat;
                break;
            case Territory.TYPEPLAYER.NONE:
                hatP1.sprite = null;
                hatP2.sprite = null;
                hatP3.sprite = null;
                break;
            case Territory.TYPEPLAYER.BOT:
                hatP1.sprite = chancaHat2;
                hatP2.sprite = chancaHat2;
                hatP3.sprite = chancaHat2;
                break;
        }
        territorySprite.sprite = selected.sprite.sprite;
        warContainer.SetActive(false);
        title.text = selected.territory.name;
        peaceContainer.SetActive(true);
    }

    public void FinishWar(TerritoryHandler territory, Territory.TYPEPLAYER type, int survivors)
    {
        territory.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        territory.war = false;
        

        if(territory.territoryStats.population == 0)
        {
            
            territory.territory.TypePlayer = type;
            territory.territoryStats.population = survivors;
                
            if(type == Territory.TYPEPLAYER.PLAYER)
            {
                InGameMenuHandler.instance.InstantiateCharacterOption(territory);
            }
            
            
        }
        if (territory == selected)
        {
            SetPeaceMenu();
        }

    }
    public void AddMoreWarriors(TerritoryHandler otherTerritory, int warriors)
    {
        foreach (War w in warList)
        {
            if (w.GetTerritory() == otherTerritory)
            {
                w.warriors1Count += warriors;
            }
        }
    }

}
