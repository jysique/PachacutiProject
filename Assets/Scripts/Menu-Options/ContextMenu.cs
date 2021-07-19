using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ContextMenu : MonoBehaviour
{
    public static ContextMenu instance;
    [SerializeField] TerritoryHandler clickedTerritory;
    [SerializeField] Button moveButton;
    [SerializeField] GameObject buttonBlock;
    
    private void Awake()
    {
        instance = this;
    }
        
    private void Update()
    {
        SetTextToolTip();
    }
    public void SetTextToolTip()
    {
        moveButton.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip4").Replace("W",/*TotalWarriors().ToString()*/ "4"));
        if (clickedTerritory.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats1");
        }
        else if (clickedTerritory.TerritoryStats.Territory.IsClaimed == false)
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats2");
        }
        else if (EventManager.instance.GetIsTerritoriesIsInPandemic() && EventManager.instance.GetIsTerritoriesIsInPandemic(clickedTerritory))
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats3");
        }
        
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
    }
    public void AttackTerritory()
    {
        TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("send_units");
        }
        if (selected.TerritoryStats.Territory.Population > 0)
        {
            //int sword = 2;
            //int lance = 2;
            //int axe = 2;
            WarManager.instance.SelectTerritory();
            
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
        //   tabManager.OnTabSelected(tabMilitar);
        MenuManager.instance.AccessTabMilitar();
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
      //  tabManager.OnTabSelected(tabTerritory);
        MenuManager.instance.AccessTabTerritory();
        MenuManager.instance.TurnOffBlock();
        TerritoryManager.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }

}
