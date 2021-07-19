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
    public GameObject battleCanvas;
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
    //
    private Color32 playerColor;
    private Color32 enemyColor;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //battleCanvas = GameObject.Find("Battle");
        status = false;
        enemyColor = new Color32(248, 147, 146,255);
        playerColor = new Color32(114, 165, 195,255);
    }
   
    public void AddWar(int c1, int c2, float s1, float s2,float cr1, float cr2 ,TerritoryHandler ta, Territory.TYPEPLAYER a)
    {
        War w = new War(c1, s1, s2,ta, a, cr1, cr2);
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
                peaceContainer.SetActive(false);
                warContainer.SetActive(true);
                SetWarriorAnimation(w.Attackers, hatAttacker, attackColor ,empire);
                SetWarriorAnimation(w.TerritoryWar.TerritoryStats.Territory.TypePlayer, hatDefender, deffendColor, empireD);
              
                title.text = GameMultiLang.GetTraduction("BattleOf") + selected.TerritoryStats.Territory.name;

                power.text = (w.Speed1*10).ToString();
                powerD.text = (w.Speed2*10).ToString();

                territorySprite.sprite = selected.sprite.sprite;

                selectedWar = w;

                
            }
        }
        
    }
    private void SetWarriorAnimation(Territory.TYPEPLAYER type, Image hat, Image background, Text empire)
    {
        switch (type)
        {
            case Territory.TYPEPLAYER.PLAYER:
                hat.sprite = incaHat;
                empire.text = "Incas";
                background.color = playerColor;
                break;
            case Territory.TYPEPLAYER.NONE:
                hat.sprite = null;
                empire.text = "No empire";
                background.color = Color.white;
                break;
            case Territory.TYPEPLAYER.BOT:
                hat.sprite = mochicaHat;
                empire.text = "Chancas";
                background.color = enemyColor;
                break;
            case Territory.TYPEPLAYER.BOT2:
                hat.sprite = chavinHat;
                empire.text = "Mochica";
                background.color = enemyColor;
                break;
            case Territory.TYPEPLAYER.BOT3:
                hat.sprite = chancaHat;
                empire.text = "Chavin";
                background.color = enemyColor;
                break;
            case Territory.TYPEPLAYER.BOT4:
                hat.sprite = chancaHat;
                empire.text = "Pendiente";
                background.color = enemyColor;
                break;
        }
    }
    private void SetPeaceMenu()
    {
        switch (selected.TerritoryStats.Territory.TypePlayer)
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
        title.text = selected.TerritoryStats.Territory.name;
        peaceContainer.SetActive(true);
    }
    public void FinishWar(TerritoryHandler territory, Territory.TYPEPLAYER type)
    {
        // TYPE es el tipo de jugador atacante
        territory.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        territory.war = false;
        

        if(territory.TerritoryStats.Territory.Population == 0)
        {
            if (type == Territory.TYPEPLAYER.PLAYER)
            {
                // SI GANA EL PLAYER
                territory.TerritoryStats.Territory.IsClaimed = false;
                EventManager.instance.AddEvent(territory);
                EventManager.instance.UpdateEventListOption();
                AlertManager.AlertConquered(territory);

            }
            else /*if(type == Territory.TYPEPLAYER.BOT)*/
            {
                // SI GANABA EL BOT
                if (territory.TerritoryStats.Territory.TypePlayer==Territory.TYPEPLAYER.PLAYER)
                {
                    AlertManager.AlertLost(territory);
                }

                MilitarChief newMilitarBoss = new MilitarChief();
                newMilitarBoss.GetMilitarBoss();
                //print(newMilitarBoss.CharacterName);
                territory.TerritoryStats.Territory.MilitarChiefTerritory = newMilitarBoss;
                BotManager.instance.CreateOrAdd(type, territory);
            }
            territory.TerritoryStats.Territory.TypePlayer = type;
            //territory.TerritoryStats.Territory.Population = survivors;
        }
        if (territory == selected)
        {
            SetPeaceMenu();
        }
        InGameMenuHandler.instance.UpdateMenu();
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
        MilitarChief mb = t.TerritoryStats.Territory.MilitarChiefTerritory;
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
        if(t.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            RoyalFamily governor = CharacterManager.instance.Governor;
            governorMod = V * ((float)governor.Militancy / 50f);
        }
      //  float motivationMod = V * ((float)t.TerritoryStats.Territory.SacredPlaceTerritory.Motivation / 10f);
      //  float attackMod = V * ((float)t.TerritoryStats.Territory.ArmoryTerritory.PlusAttack / 50f);
        //print("Ataque: inicial: " + V.ToString() + " estrategia: " + strategyMod.ToString() + " warriorNumberBonus: " + warriorNumberBonus.ToString() + " experiencia: " + experienceMod.ToString() + " governador: " + governorMod.ToString() + " motivacion: " + motivationMod.ToString() + " ataque: " + attackMod.ToString());
        V = V + ((strategyMod + warriorNumberBonus + experienceMod + governorMod/*+motivationMod+attackMod*/)*3);
        
    //    print((warriorNumberBonus + experienceMod + governorMod));
    //    print(V);
        return V;
    }

    public float SetDefenseFormula(TerritoryHandler t)
    {
        float V = initialSpeed;
        
        MilitarChief mb = t.TerritoryStats.Territory.MilitarChiefTerritory;
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
        float warriorNumberBonus = V * ((float)t.TerritoryStats.Territory.Population / 100);
        float experienceMod = V * ((float)mb.Experience / 50);
        float governorMod = 0;
        if (t.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            RoyalFamily governor = CharacterManager.instance.Governor;
            governorMod = V * ((float)governor.Militancy / 50);
        }
   //     float motivationMod = V * ((float)t.TerritoryStats.Territory.SacredPlaceTerritory.Motivation / 10f);
        float defenseMod = V * ((float)t.TerritoryStats.Territory.FortressTerritory.PlusDefense / 50f);
        //print("Defensa: inicial: " + V.ToString() + " estrategia: " + strategyMod.ToString() + " warriorNumberBonus: " + warriorNumberBonus.ToString() + " experiencia: " + experienceMod.ToString() + " governador: " + governorMod.ToString() + " motivacion: " + motivationMod.ToString() + " defensa: " + defenseMod.ToString());
        V = V + ((strategyMod + warriorNumberBonus + experienceMod + governorMod+defenseMod/*+motivationMod*/) * 3);
        
        return V;
    }

    public void SelectTerritory(int _warriorsSword,int _warriorsLance, int _warriorsArch)
    {
        if (TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().TerritoryStats.Territory.Swordsmen.Quantity - _warriorsSword >= 0 ||
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().TerritoryStats.Territory.Lancers.Quantity - _warriorsLance >= 0 ||
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().TerritoryStats.Territory.Axemen.Quantity - _warriorsArch >= 0)
        {
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
        }
    }

    public void SelectTerritory()
    {
        TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
    }

    //    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, int _warriorsSword, int _warriorsLance, int _warriorsAxe)
    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, Troop troopSelect)
    {
        /*
        int warriorsSword = troopSelect.GetNumberUnity("Swordsman");
        int warriorsLance = troopSelect.GetNumberUnity("Lancer");
        int warriorsAxe = troopSelect.GetNumberUnity("Axeman");
        */
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, troopSelect, selected);
        Instantiate(warriorsPrefab, selected.transform.position, Quaternion.identity);
    }
    /*
    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, int _warriorsSword, int _warriorsLance, int _warriorsAxe)
    {
        //selected.TerritoryStats.Territory.Swordsmen.NumbersUnit = selected.TerritoryStats.Territory.Swordsmen.NumbersUnit - _warriorsSword;
        //selected.TerritoryStats.Territory.Lancers.NumbersUnit = selected.TerritoryStats.Territory.Lancers.NumbersUnit - _warriorsLance;
        //selected.TerritoryStats.Territory.Axemen.NumbersUnit = selected.TerritoryStats.Territory.Axemen.NumbersUnit - _warriorsAxe;
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, _warriorsSword, _warriorsLance, _warriorsAxe, selected);
        Instantiate(warriorsPrefab, selected.transform.position, Quaternion.identity);

    }
    */

    public void MoveWarriors(TerritoryHandler otherTerritory,  TerritoryHandler attacker, Troop attackerTroop)
    {
        bool otherTerritoryIsPlayer = otherTerritory.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER;
        Territory.TYPEPLAYER temp = otherTerritory.TerritoryStats.Territory.TypePlayer;
        if (otherTerritory.TerritoryStats.Territory.TypePlayer == attacker.TerritoryStats.Territory.TypePlayer)
        {
            for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
            {
                otherTerritory.TerritoryStats.Territory.GetUnit(attackerTroop.UnitCombats[i].UnitName).Quantity += attackerTroop.UnitCombats[i].Quantity;
            }
        }
        else if (otherTerritory.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            // WASTE
            AlertManager.AlertExpedition();
            EventManager.instance.AddExpedicionEvent(attacker, otherTerritory,attackerTroop);
            EventManager.instance.UpdateEventListOption();
        }
        else if (attacker.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            // VUELVE DEL WASTE
            for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
            {
                otherTerritory.TerritoryStats.Territory.GetUnit(attackerTroop.UnitCombats[i].UnitName).Quantity += attackerTroop.UnitCombats[i].Quantity;
            }
        }
        else
        {
            if(attacker.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER || otherTerritory.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                DateTableHandler.instance.PauseButton();
                battleCanvas.SetActive(true);
               // print("attacker|" + attacker.TerritoryStats.Territory.name);
               // print("other|" + otherTerritory.TerritoryStats.Territory.name);
                Troop playerTroop;
                Troop enemyTroop;
                TerritoryHandler playerTerritory;
                TerritoryHandler enemyTerritory;
                if (attacker.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
                {

                    playerTroop = attackerTroop;
                    enemyTroop = new Troop(otherTerritory.TerritoryStats.Territory.Swordsmen.Quantity,
                                otherTerritory.TerritoryStats.Territory.Lancers.Quantity,
                                otherTerritory.TerritoryStats.Territory.Axemen.Quantity);
                    playerTerritory = attacker;
                    enemyTerritory = otherTerritory;
                }
                else
                {
                    enemyTroop = attackerTroop;
                    playerTroop = new Troop(otherTerritory.TerritoryStats.Territory.Swordsmen.Quantity,
                                otherTerritory.TerritoryStats.Territory.Lancers.Quantity,
                                otherTerritory.TerritoryStats.Territory.Axemen.Quantity);
                    playerTerritory = otherTerritory;
                    enemyTerritory =  attacker;
                }
                /*
                for (int i = 0; i < playerTroop.UnitCombats.Count; i++)
                {
                    print(i + "-" + playerTroop.UnitCombats[i].GetType().ToString());
                }
                */
                CombatManager.instance.StartWar(
                    playerTroop,
                    enemyTroop,
                    playerTerritory,
                    enemyTerritory,
                    otherTerritoryIsPlayer);;
                return;
            }
            int attackPower = attackerTroop.GetAllNumbersUnit();
            if (otherTerritory.war)
            {

                AddMoreWarriors(otherTerritory, attackPower);

            }
            else
            {
                float vAttack = SetAttackFormula(attacker, attackPower);
                float vDef = SetDefenseFormula(otherTerritory);
                float critic1 = 20;
                float critic2 = 20;
                AddWar(attackPower, otherTerritory.TerritoryStats.Territory.Population, vAttack, vDef,critic1,critic2, otherTerritory, attacker.TerritoryStats.Territory.TypePlayer);

                otherTerritory.war = true;
                otherTerritory.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }

        }
    }

    public void ShowCritic(War war, bool side)
    {
        if (selectedWar != war) return;
        if (side)
        {
            InGameMenuHandler.instance.ShowFloatingText("Critic!", "TextFloating", hatAttacker.transform, Color.black);
        }
        else
        {
            InGameMenuHandler.instance.ShowFloatingText("Critic!", "TextFloating", hatDefender.transform, Color.black);
        }
        
    }

}
