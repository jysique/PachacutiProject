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
    /*
    //hats
    [SerializeField] private Sprite incaHat;
    [SerializeField] private Sprite chancaHat;
    [SerializeField] private Sprite mochicaHat;
    [SerializeField] private Sprite chavinHat;
    [SerializeField] private Sprite chancaHat2;
    */
    //
    [SerializeField] private Image hatAttacker;
    [SerializeField] private Image hatDefender;
    [SerializeField] private Image hatP1;
    [SerializeField] private Image hatP2;
    [SerializeField] private Image hatP3;
    //
    [SerializeField] private Animator anim1;
    [SerializeField] private Animator anim2;
    [SerializeField] private Animator anim3;
    [SerializeField] private Animator anim4;
    [SerializeField] private Animator anim5;

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

    public void AddWar(TerritoryHandler territoryDefender, TerritoryHandler territoryAttacker, Troop troopAttacker, float speedAttacker, float speedDefender, float critAttacker, float critDefend)
    {
        War w = new War(territoryDefender, territoryAttacker, troopAttacker, speedAttacker, speedDefender, critAttacker, critDefend);
        warList.Add(w);
    }
    /*
    public void AddWar(TerritoryHandler territoryDefender, Territory.TYPEPLAYER typeAttacker,Troop troopAttacker,  float speedAttacker, float speedDefender, float critAttacker, float critDefend)
    {
        War w = new War(troopAttacker, speedAttacker, speedDefender, critAttacker, critDefend, territoryDefender, typeAttacker);
        warList.Add(w);
    }
    */
    private void FixedUpdate()
    {

        foreach(War w in warList)
        {
            w.UpdateStatus();
        }
        if(status)
        {
            // warriorsCount1.text = selectedWar.warriors1Count.ToString();
            warriorsCount1.text = selectedWar.attackerTroop.GetAllNumbersUnit().ToString();
            warriorsCount2.text = selectedWar.TerritoryWar.TerritoryStats.Territory.Population.ToString();
            if (selectedWar.TerritoryWar != null)
            {
                warriorsCount2.text = selectedWar.TerritoryWar.TerritoryStats.Territory.Population.ToString();
            }
            
        }
        anim1.speed = GlobalVariables.instance.timeModifier;
        anim2.speed = GlobalVariables.instance.timeModifier;
        anim3.speed = GlobalVariables.instance.timeModifier;
        anim4.speed = GlobalVariables.instance.timeModifier;
        anim5.speed = GlobalVariables.instance.timeModifier;

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
                //  SetWarriorAnimation(w.AttackerType, hatAttacker, attackColor ,empire);
                //  SetWarriorAnimation(w.TerritoryWar.TerritoryStats.Territory.TypePlayer, hatDefender, deffendColor, empireD);
                SetWarriorAnimation(w.TerritoryAttacker, hatDefender, deffendColor, empireD);
                SetWarriorAnimation(w.TerritoryWar, hatDefender, deffendColor, empireD);
                title.text = GameMultiLang.GetTraduction("BattleOf") + selected.TerritoryStats.Territory.name;

                power.text = (w.SpeedAttackers*10).ToString();
                powerD.text = (w.SpeedDefender*10).ToString();

                territorySprite.sprite = selected.SpriteRender.sprite;

                selectedWar = w;

                
            }
        }
        
    }
    private void SetWarriorAnimation(TerritoryHandler territory, Image hat, Image background, Text empire)
    {
        hat.sprite = territory.TerritoryStats.Territory.Civilization.Hat1;
        empire.text = territory.TerritoryStats.Territory.Civilization.Name;
        background.color = territory.TerritoryStats.Territory.Civilization.ColorBackground;
        /*
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
        */
    }

    /*
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
    */
    private void SetPeaceMenu()
    {
        hatP1.sprite = selected.TerritoryStats.Territory.Civilization.Hat2;
        hatP2.sprite = selected.TerritoryStats.Territory.Civilization.Hat2;
        hatP2.sprite = selected.TerritoryStats.Territory.Civilization.Hat2;
        /*
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
        */
        territorySprite.sprite = selected.SpriteRender.sprite;
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
            if (type == Territory.TYPEPLAYER.PLAYER) // SI GANA EL PLAYER
            {
                territory.TerritoryStats.Territory.IsClaimed = false;
                EventManager.instance.AddEvent(territory);
                EventManager.instance.UpdateEventListOption();
                AlertManager.AlertConquered(territory);
            }
            else
            {
                if (territory.TerritoryStats.Territory.TypePlayer==Territory.TYPEPLAYER.PLAYER)
                {
                    AlertManager.AlertLost(territory);
                }
                MilitarChief newMilitarBoss = new MilitarChief();
                newMilitarBoss.GetMilitarBoss();
                territory.TerritoryStats.Territory.MilitarChiefTerritory = newMilitarBoss;
                BotManager.instance.CreateOrAdd(type, territory);
            }
            territory.TerritoryStats.Territory.TypePlayer = type;
        }
        if (territory == selected)
        {
            SetPeaceMenu();
        }
       // TerritoryManager.instance.UpdateUnitsDeffend(territory.TerritoryStats.Territory);
        InGameMenuHandler.instance.UpdateMenu();
    }

    public void AddMoreWarriors(TerritoryHandler otherTerritory, Troop _troop)
    {
        foreach (War w in warList)
        {
            if (w.GetTerritory() == otherTerritory)
            {
                w.attackerTroop.AddMoreWarriors(_troop);
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
        V = V + ((strategyMod + warriorNumberBonus + experienceMod + governorMod/*+motivationMod+attackMod*/)*3);
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
        float defenseMod = V * ((float)t.TerritoryStats.Territory.FortressTerritory.PlusDefense / 50f);
        V = V + ((strategyMod + warriorNumberBonus + experienceMod + governorMod+defenseMod/*+motivationMod*/) * 3);
        
        return V;
    }



    public void SelectTerritory()
    {
        TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
    }

    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, Troop troopSelect)
    {
        //TerritoryManager.instance.UpdateUnitsDeffend(territory.TerritoryStats.Territory);
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, troopSelect, selected);
        Instantiate(warriorsPrefab, selected.transform.position, Quaternion.identity);
    }

    public void MoveWarriors(TerritoryHandler otherTerritory,  TerritoryHandler attacker, Troop attackerTroop)
    {
        //otherTerritoryIsPlayer is true si player es defensor
        //otherTerritoryIsPlayer is false si player es atacante
        bool otherTerritoryIsPlayer = otherTerritory.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER;
        Territory.TYPEPLAYER temp = otherTerritory.TerritoryStats.Territory.TypePlayer;
        if (otherTerritory.TerritoryStats.Territory.TypePlayer == attacker.TerritoryStats.Territory.TypePlayer)
        {
            for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
            {
                otherTerritory.TerritoryStats.Territory.ListUnitCombat.GetFirstUnitCombat(attackerTroop.UnitCombats[i].UnitName).Quantity += attackerTroop.UnitCombats[i].Quantity;
            }
        }
        else if (attacker.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER && 
            otherTerritory.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            // VA WASTE
            AlertManager.AlertExpedition();
            EventManager.instance.AddExpedicionEvent(attacker, otherTerritory,attackerTroop);
            EventManager.instance.UpdateEventListOption();
        }
        else if (attacker.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            // VUELVE DEL WASTE
            for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
            {
                otherTerritory.TerritoryStats.Territory.ListUnitCombat.GetFirstUnitCombat(attackerTroop.UnitCombats[i].UnitName).Quantity += attackerTroop.UnitCombats[i].Quantity;
            }
        }
        else
        {
            
            if (attacker.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER || otherTerritory.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                TutorialController.instance.CanSelectTroops = true;
                DateTableHandler.instance.PauseTime();
                battleCanvas.SetActive(true);
                Troop playerTroop;
                Troop enemyTroop;
                TerritoryHandler playerTerritory;
                TerritoryHandler enemyTerritory;
                if (attacker.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
                {
                    
                    playerTroop = attackerTroop;
                    enemyTroop = otherTerritory.TerritoryStats.Territory.ListUnitCombat;
                    //TODO cambiar si es posible
                    playerTerritory = attacker;
                    enemyTerritory = otherTerritory;
                }
                else
                {
                    enemyTroop = attackerTroop;
                    playerTroop = otherTerritory.TerritoryStats.Territory.ListUnitCombat;
                    playerTerritory = otherTerritory;
                    enemyTerritory =  attacker;
                }
                CombatManager.instance.StartWar(
                    playerTroop,
                    enemyTroop,
                    playerTerritory,
                    enemyTerritory,
                    otherTerritoryIsPlayer);;
                return;
            }
            
            if (otherTerritory.war)
            {
                AddMoreWarriors(otherTerritory, attackerTroop);
            }
            else
            {
                float vAttack = SetAttackFormula(attacker, attackerTroop.GetAllNumbersUnit());
                float vDef = SetDefenseFormula(otherTerritory);
                float critic1 = 20;
                float critic2 = 20;

                //AddWar(otherTerritory, attacker.TerritoryStats.Territory.TypePlayer, attackerTroop, vAttack, vDef,critic1,critic2);
                AddWar(otherTerritory, attacker, attackerTroop, vAttack, vDef, critic1, critic2);
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
