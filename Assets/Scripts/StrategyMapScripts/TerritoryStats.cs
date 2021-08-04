using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryStats : MonoBehaviour
{
    [SerializeField] public GameObject container;

   [SerializeField] private Text populationTxt;
    [SerializeField] private Text nameTerritoryTxt;
    [SerializeField] private Image imageTerritory;
    [SerializeField] private GameObject iconsContainer;

    [Header("New system")]

    private Territory territory;

    public Territory Territory
    {
        get { return territory; }
        set { territory = value; }
    }
    private void Start()
    {
       
        nameTerritoryTxt.text = territory.name;
        if (territory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            populationTxt.enabled = false;
        }

    }
    public void CanPopulate()
    {
        populationTxt.text = territory.Population + "/" + (territory.AcademyTerritory.LimitSwordsmen + territory.BarracksTerritory.LimitLancer + territory.CastleTerritory.LimitAxemen);
        CorroboratePopulation(territory.Swordsmen);
        CorroboratePopulation(territory.Lancers);
        CorroboratePopulation(territory.Axemen);
        CorroboratePopulation(territory.Scouts);
        CorroboratePopulation(territory.Archers);

    }
    private void FixedUpdate()
    {
        CanPopulate();
    }
    public void CorroboratePopulation(UnitCombat unitCombat, Image _timerBar = null)
    {
      //  populationTxt.text = a+"|" + unitCombat.NumbersUnit.ToString() + " / " + territory.GetLimit(unitCombat).ToString();
        if (territory.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
        //    populationTxt.color = Color.white;
            if (unitCombat.Quantity < territory.GetLimit(unitCombat))
            {
                IncresementUnit(_timerBar,unitCombat);
              //  _timerBar.enabled = true;
            }
            else if (unitCombat.Quantity < territory.GetLimit(unitCombat))
            {
          //      populationTxt.color = Color.red;
                //DecreasementPopulation();
            }
        }
        else
        {
          //  _timerBar.enabled = false;
        }
    }

    private void IncresementUnit(Image _timerBar, UnitCombat _subordinate)
    {
        if (_subordinate._timeLeft <= _subordinate._totalTime)
        {
            _subordinate._timeLeft += Time.deltaTime * territory.GetSpeed(_subordinate) * GlobalVariables.instance.timeModifier;
           // _timerBar.fillAmount = _subordinate._timeLeft / _subordinate._totalTime;  
        }
        else
        {

            _subordinate.Quantity++;
            _subordinate._timeLeft = 0;
        }
        
    }
    /*
     if (territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
               // print("bar||" + _timerBar.name + " ||" + _timerBar.isActiveAndEnabled);
               // print("type||" + _subordinate.GetType().ToString());
               // print("timeleft||" + _subordinate._totalTime);
               // print("speed||" + territory.GetSpeed(_subordinate));
            }
     */
    /*
        if (_timeLeft <= _maxTime)
        {
            _timeLeft += Time.deltaTime * territory.GetSpeed(_building) * GlobalVariables.instance.timeModifier;
            _timerBar.fillAmount = _timeLeft / _maxTime;
            if (territory.TypePlayer==Territory.TYPEPLAYER.PLAYER)
            {
                print("timeleft||"+_timeLeft);
                print("||" + territory.GetSpeed(_building));
            }
        }
        else
        {
            territory.GetUnit(_subordinate).NumbersUnit++;
            _timeLeft = 0;
        }
        */
    public void IncresementGold()
    {
        territory.Gold+= territory.GoldMineTerritory.WorkersMine / territory.PerPeople;
    }
    public void IncresementFood()
    {
        territory.FoodReward+= territory.FarmTerritory.WorkersChannel / territory.PerPeople;
    }
   
}

