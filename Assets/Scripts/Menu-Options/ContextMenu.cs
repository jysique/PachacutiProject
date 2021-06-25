using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] TerritoryHandler clickedTerritory;
    [SerializeField] Text warriorsCount;
    [SerializeField] Button increase;
    [SerializeField] Button decrease;
    [SerializeField] Button moveButton;
    [SerializeField] GameObject buttonBlock;
    [SerializeField] TabGroup tabManager;
    [SerializeField] TabButton tabMilitar;
    [SerializeField] TabButton tabTerritory;

    public int WarriorsCount()
    {
        int res = int.Parse(warriorsCount.text);
        return res;
    }

    private void Update()
    {
        SetTextToolTip();
        
    }
    public void SetTextToolTip()
    {
        moveButton.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip4A")+"\n" +
                                                          GameMultiLang.GetTraduction("tooltip4B") +"\n" +
                                                          GameMultiLang.GetTraduction("tooltip4C").Replace("W",WarriorsCount().ToString()) +"\n" +
                                                          GameMultiLang.GetTraduction("tooltip4B"));
        if (clickedTerritory.territoryStats.territory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats1");
        }
        else if (clickedTerritory.territoryStats.territory.IsClaimed == false)
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats2");
        }
        else if (EventManager.instance.GetIsTerritorieIsInPandemic() && EventManager.instance.GetIsTerritorieIsInPandemic(clickedTerritory))
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats3");
        }
        
    }
    public void SetMenu(bool canAttack, bool isWar, TerritoryHandler ta)
    {
        
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("context_menu");
        }
        int limit = ta.territoryStats.territory.Population;
        clickedTerritory = ta;
        warriorsCount.text = limit.ToString();
        moveButton.interactable = canAttack;
        if (!canAttack)
        {
            buttonBlock.SetActive(true);
        }
        else
        {
            buttonBlock.SetActive(false);
        }
        increase.interactable = canAttack;
        increase.GetComponent<NumericButton>().limit = limit;
        increase.GetComponent<NumericButton>().lockButton = canAttack;
        increase.GetComponent<NumericButton>().pointerDown = false;
        decrease.interactable = canAttack;
        decrease.GetComponent<NumericButton>().limit = limit;
        decrease.GetComponent<NumericButton>().lockButton = canAttack;
        decrease.GetComponent<NumericButton>().pointerDown = false;
        //transform.GetChild(1).GetComponent<Button>().interactable = isWar;
    }
    public void AttackTerritory()
    {
        TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("send_units");
        }
        if (selected.territoryStats.territory.Population > 0)
        {

            WarManager.instance.SelectTerritory(int.Parse(warriorsCount.text));
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
