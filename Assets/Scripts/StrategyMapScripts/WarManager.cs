using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarManager : MonoBehaviour
{
    [SerializeField] private GameObject warriorsPrefab;

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
    [SerializeField] private Image attackColor;
    [SerializeField] private Image deffendColor;
    //hats
    [SerializeField] private Sprite incaHat;
    [SerializeField] private Sprite chancaHat;
    [SerializeField] private Sprite mochicaHat;
    [SerializeField] private Sprite chavinHat;
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
    private void FixedUpdate()
    {

        foreach(War w in warList)
        {
            w.UpdateStatus();
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
                        attackColor.color = new Color(0.627f, 0.858f, 1,1);
                        break;
                    case Territory.TYPEPLAYER.NONE:
                        hatAttacker.sprite = null;
                        empire.text = "No empire";
                        attackColor.color = Color.white;
                        break;
                    case Territory.TYPEPLAYER.BOT:
                        hatAttacker.sprite = chancaHat;
                        empire.text = "Chancas";
                        attackColor.color = new Color(0.973f, 0.576f, 0.57f,1);
                        break;
                    case Territory.TYPEPLAYER.BOT2:
                        hatAttacker.sprite = mochicaHat;
                        empire.text = "Mochica";
                        attackColor.color = new Color(0.973f, 0.576f, 0.57f, 1);
                        break;
                    case Territory.TYPEPLAYER.BOT3:
                        hatAttacker.sprite = chavinHat;
                        empire.text = "Chavin";
                        attackColor.color = new Color(0.973f, 0.576f, 0.57f, 1);
                        break;
                    case Territory.TYPEPLAYER.BOT4:
                        hatAttacker.sprite = chancaHat;
                        empire.text = "Pendiente";
                        attackColor.color = new Color(0.973f, 0.576f, 0.57f, 1);
                        break;
                }
                switch (w.Territory.territoryStats.territory.TypePlayer)
                {
                    case Territory.TYPEPLAYER.PLAYER:
                        hatDefender.sprite = incaHat;
                        empireD.text = "Incas";
                        deffendColor.color = new Color(160,219,255);
                        break;
                    case Territory.TYPEPLAYER.NONE:
                        hatDefender.sprite = null;
                        empireD.text = "No empire";
                        deffendColor.color = Color.white;
                        break;
                    case Territory.TYPEPLAYER.BOT:
                        hatDefender.sprite = mochicaHat;
                        empireD.text = "Chancas";
                        deffendColor.color = new Color(248, 147, 146);
                        break;
                    case Territory.TYPEPLAYER.BOT2:
                        hatDefender.sprite = chavinHat;
                        empireD.text = "Mochica";
                        deffendColor.color = new Color(248, 147, 146);
                        break;
                    case Territory.TYPEPLAYER.BOT3:
                        hatDefender.sprite = chancaHat;
                        empireD.text = "Chavin";
                        deffendColor.color = new Color(248, 147, 146);
                        break;
                    case Territory.TYPEPLAYER.BOT4:
                        hatDefender.sprite = chancaHat;
                        empireD.text = "Pendiente";
                        deffendColor.color = new Color(248, 147, 146);
                        break;
                }
                warContainer.SetActive(true);
                title.text = "Battle of " + selected.territoryStats.territory.name;
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
            case Territory.TYPEPLAYER.BOT2:
                hatP1.sprite = mochicaHat;
                hatP2.sprite = mochicaHat;
                hatP3.sprite = mochicaHat;
                break;
            case Territory.TYPEPLAYER.BOT3:
                hatP1.sprite = chavinHat;
                hatP2.sprite = chavinHat;
                hatP3.sprite = chavinHat;
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
            if (type == Territory.TYPEPLAYER.PLAYER)
            {
                MenuManager.instance.OpenCurrentCaseMenu(territory);
              //  InGameMenuHandler.instance.InstantiateCharacterOption(territory);

            }
            else if(type == Territory.TYPEPLAYER.BOT)
            {
                MilitarBoss newMilitarBoss = new MilitarBoss();
                newMilitarBoss.GetMilitarBoss();
                //print(newMilitarBoss.CharacterName);
                territory.territoryStats.territory.MilitarBossTerritory = newMilitarBoss;
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
        MilitarBoss mb = t.territoryStats.territory.MilitarBossTerritory;
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
        //print(warriorNumberBonus);
        float experienceMod = V * ((float)mb.Experience/50f);
        float governorMod = 0;
        if(t.territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            RoyalFamily governor = CharacterManager.instance.Governor;
            governorMod = V * ((float)governor.Militancy / 50f);
        }
        float motivationMod = V * ((float)t.territoryStats.territory.SacredPlaceTerritory.Motivation / 10f);
        float attackMod = V * ((float)t.territoryStats.territory.BarracksTerritory.PlusAttack / 50f);
        //print("Ataque: inicial: " + V.ToString() + " estrategia: " + strategyMod.ToString() + " warriorNumberBonus: " + warriorNumberBonus.ToString() + " experiencia: " + experienceMod.ToString() + " governador: " + governorMod.ToString() + " motivacion: " + motivationMod.ToString() + " ataque: " + attackMod.ToString());
        V = V + ((strategyMod + warriorNumberBonus + experienceMod + governorMod+motivationMod+attackMod)*3);
        
    //    print((warriorNumberBonus + experienceMod + governorMod));
    //    print(V);
        return V;
    }

    public float SetDefenseFormula(TerritoryHandler t)
    {
        float V = initialSpeed;
        
        MilitarBoss mb = t.territoryStats.territory.MilitarBossTerritory;
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
        float warriorNumberBonus = V * ((float)t.territoryStats.territory.Population / 100);
        float experienceMod = V * ((float)mb.Experience / 50);
        float governorMod = 0;
        if (t.territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            RoyalFamily governor = CharacterManager.instance.Governor;
            governorMod = V * ((float)governor.Militancy / 50);
        }
        float motivationMod = V * ((float)t.territoryStats.territory.SacredPlaceTerritory.Motivation / 10f);
        float defenseMod = V * ((float)t.territoryStats.territory.FortressTerritory.PlusDefense / 50f);
        //print("Defensa: inicial: " + V.ToString() + " estrategia: " + strategyMod.ToString() + " warriorNumberBonus: " + warriorNumberBonus.ToString() + " experiencia: " + experienceMod.ToString() + " governador: " + governorMod.ToString() + " motivacion: " + motivationMod.ToString() + " defensa: " + defenseMod.ToString());
        V = V + ((strategyMod + warriorNumberBonus + experienceMod + governorMod+defenseMod+motivationMod) * 3);
        
        return V;
    }

    public void SelectTerritory(int warriorsNumber)
    {
        if (TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory.Population - warriorsNumber >= 0)
        {
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
        }
    }

    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, int _warriorsNumber)
    {

        selected.territoryStats.territory.Population = selected.territoryStats.territory.Population - _warriorsNumber;
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, _warriorsNumber, selected);
        Instantiate(warriorsPrefab, selected.transform.position, Quaternion.identity);

    }

    public void MoveWarriors(TerritoryHandler otherTerritory, int attackPower, TerritoryHandler attacker)
    {
        Territory.TYPEPLAYER temp = otherTerritory.territoryStats.territory.TypePlayer;
        if (otherTerritory.territoryStats.territory.TypePlayer == attacker.territoryStats.territory.TypePlayer)
        {
            otherTerritory.territoryStats.territory.Population = otherTerritory.territoryStats.territory.Population + attackPower;
        }
        else
        {
            if (otherTerritory.war)
            {
                AddMoreWarriors(otherTerritory, attackPower);

            }
            else
            {
                float vAttack = SetAttackFormula(attacker, attackPower);
                float vDef = SetDefenseFormula(otherTerritory);
                AddWar(attackPower, otherTerritory.territoryStats.territory.Population, vAttack, vDef, otherTerritory, attacker.territoryStats.territory.TypePlayer);

                otherTerritory.war = true;
                otherTerritory.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }

        }


    }

}
