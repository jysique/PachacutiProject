using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarManager : MonoBehaviour
{
    [SerializeField] private float initialSpeed;
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
    [SerializeField] private Text empire;
    [SerializeField] private Text power;
    [SerializeField] private Text empireD;
    [SerializeField] private Text powerD;
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
                        empire.text = "Incas";
                        break;
                    case Territory.TYPEPLAYER.NONE:
                        hatAttacker.sprite = null;
                        empire.text = "Libres";
                        break;
                    case Territory.TYPEPLAYER.BOT:
                        hatAttacker.sprite = chancaHat;
                        empire.text = "Chancas";
                        break;
                }
                switch (w.Territory.territoryStats.territory.TypePlayer)
                {
                    case Territory.TYPEPLAYER.PLAYER:
                        hatDefender.sprite = incaHat;
                        empireD.text = "Incas";
                        break;
                    case Territory.TYPEPLAYER.NONE:
                        hatDefender.sprite = null;
                        empireD.text = "Libres";
                        break;
                    case Territory.TYPEPLAYER.BOT:
                        hatDefender.sprite = chancaHat;
                        empireD.text = "Chancas";
                        break;
                }
                warContainer.SetActive(true);
                title.text = "Batalla de " + selected.territoryStats.territory.name;
                power.text = (w.Speed1*10).ToString();
                powerD.text = (w.Speed2*10).ToString();
                territorySprite.sprite = selected.sprite.sprite;
                selectedWar = w;
                peaceContainer.SetActive(false);
            }
        }
        
    }
    
    private void SetPeaceMenu()
    {
        switch (selected.territoryStats.territory.TypePlayer)
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
        title.text = selected.territoryStats.territory.name;
        peaceContainer.SetActive(true);
    }

    public void FinishWar(TerritoryHandler territory, Territory.TYPEPLAYER type, int survivors)
    {
        territory.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        territory.war = false;
        

        if(territory.territoryStats.territory.Population == 0)
        {
            
            territory.territoryStats.territory.TypePlayer = type;
            territory.territoryStats.territory.Population = survivors;
                
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

    public float SetAttackFormula(TerritoryHandler t, int warriorsNumber)
    {
        float V = initialSpeed;
        if (t.territory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            return V;
        }
        MilitarBoss mb = t.territory.MilitarBossTerritory;
        float strategyMod = 0;
        switch (mb.StrategyType)
        {
            case "AGGRESSIVE":
                strategyMod = V * 0.08f;
                break;
            case "TERRAIN_MASTER":
                strategyMod = V * -0.06f;
                break;
            case "DEFENSIVE":
                strategyMod = V * -0.02f;
                break;
            case "SACRED_WARRIOR":
                strategyMod = V * 0.05f;
                break;
            case "SIEGE_EXPERT":
                strategyMod = V * 0.15f;
                break;
        }
        float warriorNumberBonus = V * ((float)warriorsNumber / 100f);
        print(warriorNumberBonus);
        float experienceMod = V * ((float)mb.Experience/50f);
        float governorMod = 0;
        if(t.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            RoyalFamily governor = CharacterManager.instance.Governor;
            governorMod = V * ((float)governor.Militancy / 50f);
        }
        
        V = V + strategyMod + ((warriorNumberBonus + experienceMod + governorMod)*2);
        print((warriorNumberBonus + experienceMod + governorMod));
        print(V);
        return V;
    }

    public float SetDefenseFormula(TerritoryHandler tr)
    {
        float V = initialSpeed;
        if (tr.territory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            return V;
        }
        MilitarBoss mb = tr.territory.MilitarBossTerritory;
        float strategyMod = 0;
        switch (mb.StrategyType)
        {
            case "AGGRESSIVE":
                strategyMod = V * -0.02f;
                break;
            case "TERRAIN_MASTER":
                strategyMod = V * 0.15f;
                break;
            case "DEFENSIVE":
                strategyMod = V * 0.08f;
                break;
            case "SACRED_WARRIOR":
                strategyMod = V * 0.05f;
                break;
            case "SIEGE_EXPERT":
                strategyMod = V * -0.06f;
                break;
        }
        float warriorNumberBonus = V * ((float)tr.territoryStats.population / 100);
        float experienceMod = V * ((float)mb.Experience / 50);
        float governorMod = 0;
        if (tr.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            RoyalFamily governor = CharacterManager.instance.Governor;
            governorMod = V * ((float)governor.Militancy / 50);
        }
        V = V + strategyMod + ((warriorNumberBonus + experienceMod + governorMod) * 2);
        print(V);
        return V;
    }

}
