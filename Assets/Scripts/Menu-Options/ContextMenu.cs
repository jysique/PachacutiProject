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
        if (clickedTerritory.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            if (EventManager.instance.GetIsTerritoriesIsInPandemic() && EventManager.instance.GetIsTerritoriesIsInPandemic(clickedTerritory))
            {
                buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats3");
            }else if (!clickedTerritory.Territory.IsClaimed)
            {
                moveButton.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("territoryStats2"));
            }
            else
            {
                moveButton.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip4").Replace("W", "4"));
            }
        }
        else
        {
            buttonBlock.transform.Find("Text").transform.GetComponent<Text>().text = GameMultiLang.GetTraduction("territoryStats1");
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
        //int limit = ta.Territory.Population;

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
        if (selected.Territory.IsClaimed == false)
        {
            GlobalVariables.instance.CenterCameraToTerritory(selected, true);
            if (!MenuManager.instance.IsOpen)
                MenuManager.instance.OpenMenuLearp();
            MenuManager.instance.AccessTabMilitar();
            //Invoke("Dummy", 2.0f);
            this.gameObject.SetActive(false);
            return;
        }
        if (selected.Territory.Population > 0)
        {
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
        if (!MenuManager.instance.IsOpen)
            MenuManager.instance.OpenMenuLearp();
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
        if (!MenuManager.instance.IsOpen)
            MenuManager.instance.OpenMenuLearp();
        MenuManager.instance.AccessTabTerritory();
        MenuManager.instance.TurnOffBlock();
        TerritoryManager.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }

}
