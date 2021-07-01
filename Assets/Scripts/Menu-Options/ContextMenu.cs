using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ContextMenu : MonoBehaviour
{
    public static ContextMenu instance;
    [SerializeField] TerritoryHandler clickedTerritory;
    [SerializeField] private GameObject warriorsSword;
    [SerializeField] private GameObject warriorsLance;
    [SerializeField] private GameObject warriorsArcher;

    [SerializeField] Button moveButton;
    [SerializeField] GameObject buttonBlock;
    [SerializeField] TabGroup tabManager;
    [SerializeField] TabButton tabMilitar;
    [SerializeField] TabButton tabTerritory;
    public bool canAttackTerritory = true;
    private void Awake()
    {
        instance = this;
    }
    private TextMeshProUGUI GetWarriorsText(GameObject go)
    {
        List<Transform> tr = GlobalVariables.instance.GetAllChildren(go.transform);
        return tr[2].GetComponent<TextMeshProUGUI>();
    }
    private NumericButton GetWarriorsButton(int i ,GameObject go)
    {
        List<Transform> tr = GlobalVariables.instance.GetAllChildren(go.transform);
        return tr[i].GetComponent<NumericButton>();
    }
    public int WarriorsSword()
    {
        int res = int.Parse(GetWarriorsText(warriorsSword).text);
        return res;
    }
    public int WarriorsLance()
    {
        int res = int.Parse(GetWarriorsText(warriorsLance).text);
        return res;
    }
    public int WarriorsArch()
    {
        int res = int.Parse(GetWarriorsText(warriorsArcher).text);
        return res;
    }
    public int TotalWarriors()
    {
        return WarriorsSword() + WarriorsArch() + WarriorsLance();
    }

    private void Update()
    {
        SetTextToolTip();
        GetWarriorsButton(0, warriorsSword).limit = clickedTerritory.TerritoryStats.Territory.Swordsmen.NumbersUnit;
        GetWarriorsButton(0, warriorsLance).limit = clickedTerritory.TerritoryStats.Territory.Lancers.NumbersUnit;
        GetWarriorsButton(0, warriorsArcher).limit = clickedTerritory.TerritoryStats.Territory.Archer.NumbersUnit;
    }
    public void SetTextToolTip()
    {
        moveButton.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip4A")+"\n" +
                                                          GameMultiLang.GetTraduction("tooltip4B") +"\n" +
                                                          GameMultiLang.GetTraduction("tooltip4C").Replace("W",TotalWarriors().ToString()) +"\n" +
                                                          GameMultiLang.GetTraduction("tooltip4B"));
        if (clickedTerritory.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats1");
        }
        else if (clickedTerritory.TerritoryStats.Territory.IsClaimed == false)
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats2");
        }
        else if (EventManager.instance.GetIsTerritorieIsInPandemic() && EventManager.instance.GetIsTerritorieIsInPandemic(clickedTerritory))
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats3");
        }
        
    }
    public void UpdateNumericButton(Button btn,int limit,bool canAttack)
    {
        btn.interactable = canAttack;
        btn.GetComponent<NumericButton>().limit = limit;
        btn.GetComponent<NumericButton>().lockButton = canAttack;
        btn.GetComponent<NumericButton>().pointerDown = false;
    }
    public void UpdateMenuByUnits(GameObject go, int limit, bool canAttack)
    {
        List<Transform> tr = GlobalVariables.instance.GetAllChildren(go.transform);
        //increase
        UpdateNumericButton(tr[0].GetComponent<Button>(), limit, canAttack);
        //decrease
        UpdateNumericButton(tr[1].GetComponent<Button>(), limit, canAttack);
        tr[2].GetComponent<TextMeshProUGUI>().text = limit.ToString();
    }
    public void UpdateUnits()
    {

    }
    
    public void SetMenu(bool canAttack, bool isWar, TerritoryHandler ta)
    {
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("context_menu");
        }
        //int limit = ta.TerritoryStats.Territory.Population;
        int limit_swords = ta.TerritoryStats.Territory.Swordsmen.NumbersUnit;
        int limit_lancers = ta.TerritoryStats.Territory.Lancers.NumbersUnit;
        int limit_archers = ta.TerritoryStats.Territory.Archer.NumbersUnit;
        canAttackTerritory = canAttack;
        clickedTerritory = ta;
        moveButton.interactable = canAttack;
        if (!canAttack)
        {
            buttonBlock.SetActive(true);
        }
        else
        {
            buttonBlock.SetActive(false);
        }
        UpdateMenuByUnits(warriorsSword, limit_swords, canAttack);
        UpdateMenuByUnits(warriorsLance, limit_lancers, canAttack);
        UpdateMenuByUnits(warriorsArcher, limit_archers, canAttack);
        
        //transform.GetChild(1).GetComponent<Button>().interactable = isWar;
    }
    /*
        increaseSword.interactable = canAttack;
        increaseSword.GetComponent<NumericButton>().limit = limit_swords;
        increaseSword.GetComponent<NumericButton>().lockButton = canAttack;
        increaseSword.GetComponent<NumericButton>().pointerDown = false;

        increaseLance.interactable = canAttack;
        increaseLance.GetComponent<NumericButton>().limit = limit_lancers;
        increaseLance.GetComponent<NumericButton>().lockButton = canAttack;
        increaseLance.GetComponent<NumericButton>().pointerDown = false;

        increaseArch.interactable = canAttack;
        increaseArch.GetComponent<NumericButton>().limit = limit_archers;
        increaseArch.GetComponent<NumericButton>().lockButton = canAttack;
        increaseArch.GetComponent<NumericButton>().pointerDown = false;

        decreaseSword.interactable = canAttack;
        decreaseSword.GetComponent<NumericButton>().limit = limit_swords;
        decreaseSword.GetComponent<NumericButton>().lockButton = canAttack;
        decreaseSword.GetComponent<NumericButton>().pointerDown = false;

        decreaseLance.interactable = canAttack;
        decreaseLance.GetComponent<NumericButton>().limit = limit_lancers;
        decreaseLance.GetComponent<NumericButton>().lockButton = canAttack;
        decreaseLance.GetComponent<NumericButton>().pointerDown = false;

        decreaseArch.interactable = canAttack;
        decreaseArch.GetComponent<NumericButton>().limit = limit_archers;
        decreaseArch.GetComponent<NumericButton>().lockButton = canAttack;
        decreaseArch.GetComponent<NumericButton>().pointerDown = false;
        */

    public void AttackTerritory()
    {
        TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("send_units");
        }
        if (selected.TerritoryStats.Territory.Population > 0)
        {

            WarManager.instance.SelectTerritory(int.Parse(GetWarriorsText(warriorsSword).text), int.Parse(GetWarriorsText(warriorsLance).text), int.Parse(GetWarriorsText(warriorsArcher).text));
            MenuManager.instance.TurnOffBlock();
            TerritoryManager.instance.ChangeStateTerritory(0);
            this.gameObject.SetActive(false);
        }
        

    }

    public void ShowMilitarMenu()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("menu_click");
        }
        tabManager.OnTabSelected(tabMilitar);
        MenuManager.instance.TurnOffBlock();
        TerritoryManager.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }
    public void ShowTerritoryMenu()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("menu_click");
        }
        tabManager.OnTabSelected(tabTerritory);
        MenuManager.instance.TurnOffBlock();
        TerritoryManager.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }

}
