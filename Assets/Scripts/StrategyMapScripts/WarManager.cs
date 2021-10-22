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
            warriorsCount2.text = selectedWar.TerritoryWar.Territory.Population.ToString();
            if (selectedWar.TerritoryWar != null)
            {
                warriorsCount2.text = selectedWar.TerritoryWar.Territory.Population.ToString();
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
            if (w.TerritoryWar == selected)
            //  if (w.GetTerritory() == selected)
            {
                peaceContainer.SetActive(false);
                warContainer.SetActive(true);
                //  SetWarriorAnimation(w.AttackerType, hatAttacker, attackColor ,empire);
                //  SetWarriorAnimation(w.TerritoryWar.Territory.TypePlayer, hatDefender, deffendColor, empireD);
                SetWarriorAnimation(w.TerritoryAttacker, hatDefender, deffendColor, empireD);
                SetWarriorAnimation(w.TerritoryWar, hatDefender, deffendColor, empireD);
                title.text = GameMultiLang.GetTraduction("BattleOf") + selected.Territory.name;

                power.text = (w.SpeedAttackers*10).ToString();
                powerD.text = (w.SpeedDefender*10).ToString();

                territorySprite.sprite = selected.SpriteRender.sprite;

                selectedWar = w;

                
            }
        }
        
    }
    private void SetWarriorAnimation(TerritoryHandler territory, Image hat, Image background, Text empire)
    {
        hat.sprite = territory.Territory.Civilization.Hat1;
        empire.text = territory.Territory.Civilization.Name;
        background.color = territory.Territory.Civilization.ColorBackground;
        
    }

    private void SetPeaceMenu()
    {
        hatP1.sprite = selected.Territory.Civilization.Hat2;
        hatP2.sprite = selected.Territory.Civilization.Hat2;
        territorySprite.sprite = selected.SpriteRender.sprite;
        warContainer.SetActive(false);
        title.text = selected.Territory.name;
        peaceContainer.SetActive(true);
    }

    
    
    public void FinishWar(TerritoryHandler defendingTerritory, TerritoryHandler attackerterritory, bool isAttackerWin)
    {
        // TYPE es el tipo de jugador atacante
        
        defendingTerritory.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        defendingTerritory.war = false;

        Territory.TYPEPLAYER attackerType = attackerterritory.Territory.TypePlayer;
        Territory.TYPEPLAYER defenderType = attackerterritory.Territory.TypePlayer;

        Civilization attackerCivilization = attackerterritory.Territory.Civilization;

        if (isAttackerWin)
        {
            if (attackerType == Territory.TYPEPLAYER.PLAYER)
            {
                defendingTerritory.Territory.IsClaimed = false;
                EventManager.instance.AddEvent(defendingTerritory);
                EventManager.instance.UpdateEventListOption();
                AlertManager.AlertConquered(defendingTerritory);
            }
            else
            {
                if (defenderType == Territory.TYPEPLAYER.PLAYER)
                {
                    AlertManager.AlertLost(defendingTerritory);
                }
                MilitarChief newMilitarBoss = new MilitarChief();
                newMilitarBoss.GetMilitarBoss();
                defendingTerritory.Territory.MilitarChiefTerritory = newMilitarBoss;
                BotManager.instance.CreateOrAdd(attackerType, defendingTerritory);
            }
            defendingTerritory.Territory.TypePlayer =attackerType;
            defendingTerritory.Territory.Civilization = attackerCivilization;
        }
        if (defendingTerritory == selected)
        {
            SetPeaceMenu();
        }
        
        InGameMenuHandler.instance.UpdateMenu();
    }
    public void AddMoreWarriors(TerritoryHandler otherTerritory, Troop _troop)
    {
        foreach (War w in warList)
        {
            if (w.TerritoryWar == otherTerritory)
            //if (w.GetTerritory() == otherTerritory)
            {
                for (int i = 0; i < _troop.UnitCombats.Count; i++)
                {
                    w.attackerTroop.AddUnitCombat(_troop.UnitCombats[i]);
                }
                
            }
        }
    }
    public float SetAttackFormula(TerritoryHandler t, int warriorsNumber)
    {
        float V = initialSpeed;
        MilitarChief mb = t.Territory.MilitarChiefTerritory;
        float strategyMod = 0;
        switch (mb.StrategyType)
        {
            case MilitarChief.TYPESTRAT.AGGRESSIVE:
                strategyMod = V * 0.08f;
                break;
            case MilitarChief.TYPESTRAT.TERRAIN_MASTER:
                strategyMod = V * -0.06f;
                break;
            case MilitarChief.TYPESTRAT.DEFENSIVE:
                strategyMod = V * -0.02f;
                break;
            case MilitarChief.TYPESTRAT.SACRED_WARRIOR:
                strategyMod = V * 0.05f;
                break;
            case MilitarChief.TYPESTRAT.SIEGE_EXPERT:
                strategyMod = V * 0.15f;
                break;
            case MilitarChief.TYPESTRAT.BRAVE:
                strategyMod = V * 0.15f;
                break;
            case MilitarChief.TYPESTRAT.ORGANIZER:
                strategyMod = V * 0.15f;
                break;
            default:
                break;
        }
        float warriorNumberBonus = V * ((float)warriorsNumber / 100f);
        //print(warriorNumberBonus);
        float experienceMod = V * ((float)mb.Experience/50f);
        float governorMod = 0;
        if(t.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
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
        
        MilitarChief mb = t.Territory.MilitarChiefTerritory;
        float strategyMod = 0;
        switch (mb.StrategyType)
        {
            case MilitarChief.TYPESTRAT.AGGRESSIVE:
                strategyMod = V * -0.02f;
                break;
            case MilitarChief.TYPESTRAT.TERRAIN_MASTER:
                strategyMod = V * 0.15f;
                break;
            case MilitarChief.TYPESTRAT.DEFENSIVE:
                strategyMod = V * 0.08f;
                break;
            case MilitarChief.TYPESTRAT.SACRED_WARRIOR:
                strategyMod = V * 0.05f;
                break;
            case MilitarChief.TYPESTRAT.SIEGE_EXPERT:
                strategyMod = V * -0.06f;
                break;
            case MilitarChief.TYPESTRAT.BRAVE:
                strategyMod = V * -0.06f;
                break;
            case MilitarChief.TYPESTRAT.ORGANIZER:
                strategyMod = V * -0.06f;
                break;
            default:
                break;
        }
        float warriorNumberBonus = V * ((float)t.Territory.Population / 100);
        float experienceMod = V * ((float)mb.Experience / 50);
        float governorMod = 0;
        if (t.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            RoyalFamily governor = CharacterManager.instance.Governor;
            governorMod = V * ((float)governor.Militancy / 50);
        }
        float defenseMod = V * ((float)t.Territory.FortressTerritory.PlusDefense / 50f);
        V = V + ((strategyMod + warriorNumberBonus + experienceMod + governorMod+defenseMod/*+motivationMod*/) * 3);
        
        return V;
    }



    public void SelectTerritory()
    {
        TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
    }

    public void SendWarriors(TerritoryHandler attackerTerritory, TerritoryHandler defenderTerritory, Troop troopSelect)
    {
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(defenderTerritory,attackerTerritory, troopSelect);
        Instantiate(warriorsPrefab, attackerTerritory.transform.position, Quaternion.identity);
    }

    public void MoveWarriors( TerritoryHandler attackerTerritory, TerritoryHandler defenderTerritory, Troop attackerTroop)
    {
        Territory.TYPEPLAYER attackerType = attackerTerritory.Territory.TypePlayer;
        Territory.TYPEPLAYER defenderType = defenderTerritory.Territory.TypePlayer;
        //if (otherTerritory.Territory.TypePlayer == attackerTerritory.Territory.TypePlayer)
        // SI SON DE IGUAL TIPO DE JUGADOR, AÑADE MAS UNIDADES
        if (defenderType == attackerType)
        {
            for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
            {
                defenderTerritory.Territory.ListUnitCombat.AddUnitCombat(attackerTroop.UnitCombats[i]);
            }
        }
        else if (attackerType == Territory.TYPEPLAYER.PLAYER && defenderType == Territory.TYPEPLAYER.WASTE)
        {
            // VA WASTE
            AlertManager.AlertExpedition();
            EventManager.instance.AddExpedicionEvent(attackerTerritory, defenderTerritory,attackerTroop);
            EventManager.instance.UpdateEventListOption();
        }
        else if (attackerType == Territory.TYPEPLAYER.WASTE)
        {
            for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
            {
                defenderTerritory.Territory.ListUnitCombat.AddUnitCombat(attackerTroop.UnitCombats[i]);
            }
        }
        else if (attackerType != Territory.TYPEPLAYER.PLAYER && defenderType != Territory.TYPEPLAYER.PLAYER)
        {
            if (defenderTerritory.war)
            {
                AddMoreWarriors(defenderTerritory, attackerTroop);
            }
            else
            {
                float vAttack = SetAttackFormula(attackerTerritory, attackerTroop.GetAllNumbersUnit());
                float vDef = SetDefenseFormula(defenderTerritory);
                float critic1 = 20;
                float critic2 = 20;

                AddWar(defenderTerritory, attackerTerritory, attackerTroop, vAttack, vDef, critic1, critic2);
                defenderTerritory.war = true;
                defenderTerritory.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else
        {
            TutorialController.instance.CanSelectTroops = true;
            DateTableHandler.instance.PauseTime();
            battleCanvas.SetActive(true);
            Troop new_defenderTroop = new Troop();
            Troop new_attackerTroop = new Troop();

            Troop defenderTroop = new Troop();
            defenderTroop = defenderTerritory.Territory.ListUnitCombat;
            
            for (int i = 0; i < defenderTroop.UnitCombats.Count; i++)
            {
                defenderTroop.UnitCombats[i].PositionInBattle = i;
                new_defenderTroop.AddUnitCombat(defenderTroop.UnitCombats[i]);
            }
            
            for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
            {
                attackerTroop.UnitCombats[i].PositionInBattle = i;
                new_attackerTroop.AddUnitCombat(attackerTroop.UnitCombats[i]);
            }
            
            defenderTerritory.Territory.MoveUnits(new_defenderTroop);

            CombatManager.instance.StartCombat(defenderTerritory, attackerTerritory, new_defenderTroop, new_attackerTroop);
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
