using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryStats : MonoBehaviour
{
    [SerializeField] private Image timerBar1;
    [SerializeField] private Image timerBar2;
    [SerializeField] private Image timerBar3;
    [SerializeField] public GameObject container;

   // [SerializeField] private Text populationTxt;
    [SerializeField] private Text nameTerritoryTxt;
    [SerializeField] private Image imageTerritory;
    [SerializeField] private GameObject iconsContainer;

    [Header("New system")]
    [SerializeField] private Text swordmanTxt;
    [SerializeField] private Text archerTxt;
    [SerializeField] private Text lancerTxt;

    private Territory territory;

    public Territory Territory
    {
        get { return territory; }
        set { territory = value; }
    }

    Text[] allText;
    private void Start()
    {
        
        //imageTerritory.color = new Color( Random.Range(0f, 1f), Random.Range(0f, 1f),Random.Range(0f, 1f));
        nameTerritoryTxt.text = territory.name;
        //allText = iconsContainer.gameObject.transform.GetComponentsInChildren<Text>();
        
    }
    public void CanPopulate()
    {
        CorroboratePopulation(swordmanTxt,"S", timerBar1, territory.Swordsmen);
        CorroboratePopulation(lancerTxt,"L",timerBar2, territory.Lancers);
        CorroboratePopulation(archerTxt,"A",timerBar3,  territory.Archer);
    }
    private void FixedUpdate()
    {
        CanPopulate();
        /*
        allText[0].text = territory.IrrigationChannelTerritory.Level.ToString();
        allText[1].text = territory.GoldMineTerritory.Level.ToString();
        allText[2].text = territory.SacredPlaceTerritory.Level.ToString();
        allText[3].text = territory.FortressTerritory.Level.ToString();
        allText[4].text = territory.BarracksTerritory.Level.ToString();
        */
    }
    public void CorroboratePopulation(Text populationTxt,string a, Image _timerBar, UnitCombat unitCombat)
    {
        populationTxt.text = a+"|" + unitCombat.NumbersUnit.ToString() + " / " + territory.GetLimit(unitCombat).ToString();
        if (territory.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
            populationTxt.color = Color.white;
            if (unitCombat.NumbersUnit < territory.GetLimit(unitCombat))
            {
                IncresementUnit(_timerBar,unitCombat);
                _timerBar.enabled = true;
            }
            else if (unitCombat.NumbersUnit < territory.GetLimit(unitCombat))
            {
                populationTxt.color = Color.red;
                DecreasementPopulation();
            }
        }
        else
        {
            _timerBar.enabled = false;
        }
    }
    /*
    private void IncresementPopulation()
    {
        if (timeLeftP <= maxTimeCount)
        {
            timeLeftP += Time.deltaTime * territory.VelocityPopulation * GlobalVariables.instance.timeModifier;
            timerBar.fillAmount = timeLeftP / maxTimeCount;
        }
        else
        {
            territory.Population++;
            timeLeftP = 0;
        }
    }
    */
    private void IncresementUnit(Image _timerBar, UnitCombat _subordinate)
    {
        if (_subordinate._timeLeft <= _subordinate._totalTime)
        {
            _subordinate._timeLeft += Time.deltaTime * territory.GetSpeed(_subordinate) * GlobalVariables.instance.timeModifier;
            _timerBar.fillAmount = _subordinate._timeLeft / _subordinate._totalTime;
            
        }
        else
        {
            _subordinate.NumbersUnit++;
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
        territory.FoodReward+= territory.IrrigationChannelTerritory.WorkersChannel / territory.PerPeople;
    }
   
}

