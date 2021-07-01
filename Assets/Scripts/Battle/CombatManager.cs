using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    public static CombatManager instance;
    [SerializeField] private Text turnsCounter;
    [SerializeField]private List<UnitGroup> units = new List<UnitGroup>();
    private GameObject canvas;
    Vector3[] positions = new []{
        new Vector3( 0.5f, -0.7f, 90.0f ), //espada
        new Vector3( -4.2f, -2.1f, 90.0f ), //lanza
        new Vector3(0.5f, -3.8f, 90.0f) , //hacha
        new Vector3( 4.8f, -0.7f, 90.0f ), //espada2
        new Vector3( 8.5f, -2.1f, 90.0f ), //lanza2
        new Vector3(4.8f, -3.8f, 90.0f)  //hacha2
    };

    [SerializeField] private Sprite swordGuy;
    [SerializeField] private Sprite spearGuy;
    [SerializeField] private Sprite axeGuy;
    [SerializeField] private GameObject unitGroupPrefab;
    [SerializeField] private GameObject menu;


    [SerializeField] private int turns;
    [SerializeField] private int c;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        turns = 10;
        c = -1;
        canvas = GameObject.Find("Canvas");
        StartWar(150,150,150,150,150,150,Territory.TYPEPLAYER.PLAYER, Territory.TYPEPLAYER.BOT);
    }

    void Update()
    {
        
    }

    void SortList(List<UnitGroup> alpha)
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            UnitGroup temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    private void InstantiateUnit(int number, Sprite _sprite, int pos, Territory.TYPEPLAYER _type, UnitGroup.TYPE weapontype)
    {
        
        var pref = Instantiate(unitGroupPrefab);
        pref.transform.SetParent(canvas.transform, false);
        pref.GetComponent<RectTransform>().anchoredPosition = new Vector3(positions[pos].x * 100  , positions[pos].y * 100  , positions[pos].z); ;
        pref.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
        pref.GetComponent<Image>().sprite = _sprite;
        if(_type != Territory.TYPEPLAYER.PLAYER)
        {
            Vector3 scale = pref.transform.localScale;
            scale.x *= -1;
            pref.transform.localScale = scale;
            Vector3 scale2 = pref.transform.GetChild(0).localScale;
            scale2.x *= -1;
            pref.transform.GetChild(0).localScale = scale2;
        }
        UnitGroup unit = new UnitGroup(_type, weapontype, number, pref);
        
        units.Add(unit);
        pref.GetComponent<GroupClassContainer>().stats = unit;
    }
    public void StartWar(int swordWarriors, int spearWarriors, int axeWarriors, int swordWarriors2, int spearWarriors2, int axeWarriors2, Territory.TYPEPLAYER attacker, Territory.TYPEPLAYER defender)
    {
        
        InstantiateUnit(swordWarriors, swordGuy, 0, attacker, UnitGroup.TYPE.SWORD);
        InstantiateUnit(spearWarriors, spearGuy, 1, attacker, UnitGroup.TYPE.SPEAR);
        InstantiateUnit(axeWarriors, axeGuy, 2, attacker, UnitGroup.TYPE.AXE);
        InstantiateUnit(swordWarriors2, swordGuy, 3, defender, UnitGroup.TYPE.SWORD);
        InstantiateUnit(spearWarriors2, spearGuy, 4, defender, UnitGroup.TYPE.SPEAR);
        InstantiateUnit(axeWarriors2, axeGuy, 5, defender, UnitGroup.TYPE.AXE);

        SortList(units);
        MakeMovement();

    }

    public void MakeMovement()
    {
        c++;
        if (c >= units.Count) c = 0;
        units[c].Defense = false;
        units[c].UnitsGO.GetComponent<Image>().color = Color.white;
        


        Vector3 newpos = units[c].UnitsGO.transform.position;
        //if(units[c].TypePlayer == Territory.TYPEPLAYER.PLAYER)
        menu.transform.position = new Vector3(newpos.x+1,newpos.y-1,newpos.z);
        //else
        //{

        //}
        turns--;
        turnsCounter.text = "Turnos restantes: "+ turns.ToString();
        
    }

    public void Attack()
    {
        foreach(UnitGroup u in units)
        {
            //if(u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            if (u != units[c])
            {
                u.UnitsGO.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }
    public void Defend()
    {
        units[c].UnitsGO.GetComponent<Image>().color = Color.blue;
        units[c].Defense = true;
    }



    public void MakeDamage(UnitGroup defenseGroup)
    {
        UnitGroup attackGroup = units[c];
        int damage = 10;
        if (WeaponTriangle(attackGroup.Type, defenseGroup.Type))
        {
            damage *= 2;
        }
        else
        {
            damage /= 2;
        }
        int critic = Random.Range(1,100);
        if (critic < 10) damage *= 2;
        if (defenseGroup.Defense) damage /= 2;
        print(defenseGroup.Quantity);
        print(damage);
        defenseGroup.Quantity = defenseGroup.Quantity - damage;
        
        defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = defenseGroup.Quantity.ToString();
        TurnOffButtons();

    }

    private void TurnOffButtons()
    {
        foreach (UnitGroup u in units)
        {
            //if(u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            
            u.UnitsGO.transform.GetChild(1).gameObject.SetActive(false);
            
        }
    }

    private bool WeaponTriangle(UnitGroup.TYPE attackWeapon, UnitGroup.TYPE defenseWeapon)
    {
        if(attackWeapon == UnitGroup.TYPE.SWORD)
        {
            if(defenseWeapon == UnitGroup.TYPE.SPEAR)
            {
                return false;
            }
            if (defenseWeapon == UnitGroup.TYPE.AXE)
            {
                return true;
            }
        }
        if (attackWeapon == UnitGroup.TYPE.SPEAR)
        {
            if (defenseWeapon == UnitGroup.TYPE.AXE)
            {
                return false;
            }
            if (defenseWeapon == UnitGroup.TYPE.SWORD)
            {
                return true;
            }
        }
        if (attackWeapon == UnitGroup.TYPE.AXE)
        {
            if (defenseWeapon == UnitGroup.TYPE.SWORD)
            {
                return false;
            }
            if (defenseWeapon == UnitGroup.TYPE.SPEAR)
            {
                return true;
            }
        }
        return true;
    }
}
