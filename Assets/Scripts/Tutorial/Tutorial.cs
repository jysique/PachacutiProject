using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Tutorial
{
    [SerializeField]private int order;
    [SerializeField] private string nameT;
    public int Order
    {
        get { return order; }
        set { order = value;}
    }
    public virtual void CheckTutorial() { }
    public void CompletedTutorial()
    {
        TutorialController.instance.CanSpeech = true;
        TutorialController.instance.CompletedTutorial();
    }
}

public class TutorialMissions:Tutorial
{
    public TutorialMissions(int _order)
    {
        this.Order = _order;

    }
    public override void CheckTutorial()
    {
        if (TutorialController.instance.CanMoveUnits == true)
        {
            //Debug.Log("cumpli tutorial mission");
            this.CompletedTutorial();
        }
    }
}

public class TutorialMoveTroops : Tutorial
{
    public TutorialMoveTroops(int _order)
    {
        this.Order = _order;
    }
    public override void CheckTutorial()
    {
        if (TutorialController.instance.CanSelectTroops == true)
        {
            this.CompletedTutorial();
        }
    }
}

public class TutorialMoveSword : Tutorial
{
    public TutorialMoveSword(int _order)
    {
        this.Order = _order;
    }
    public override void CheckTutorial()
    {
        TutorialController.instance.TurnBattle1Indication(true);
        SquareType square = CombatManager.instance.Squares.transform.GetChild(1).gameObject.GetComponent<SquareType>();
        UnitGroup ug = square.unitGroup;
        
        if (ug != null && ug.UnitCombat.UnitName == "Swordsman" && ug.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            TutorialController.instance.TurnBattle1Indication(false);
            TutorialController.instance.TurnBattle2Indication(true);
            square = CombatManager.instance.Squares.transform.GetChild(4).gameObject.GetComponent<SquareType>();
            ug = square.unitGroup;
            if (ug != null && ug.UnitCombat.UnitName == "Archer")
            {
                TutorialController.instance.TurnBattle2Indication(false);
                this.CompletedTutorial();
            }
        }
    }
}

public class TutorialDefendSword : Tutorial
{
    int x;
    public TutorialDefendSword(int _order)
    {
        this.Order = _order;
    }
    public override void CheckTutorial()
    {
        SquareType square1 = CombatManager.instance.Squares.transform.GetChild(1).gameObject.GetComponent<SquareType>();
        UnitGroup ug1 = square1.unitGroup;
        if (ug1 != null && ug1.UnitCombat.UnitName == "Swordsman" && ug1.Defense)
        {
            x = 1;
        }
        SquareType square2 = CombatManager.instance.Squares.transform.GetChild(4).gameObject.GetComponent<SquareType>();
        UnitGroup ug2 = square2.unitGroup;
        if (ug2 != null && ug2.UnitCombat.UnitName == "Archer" && ug2.Defense)
        {
            x = 2;
        }
        if (x==2)
        {
            this.CompletedTutorial();
        }
        
    }
}
public class TutorialAttackArcher : Tutorial
{
    public TutorialAttackArcher(int _order)
    {
        this.Order = _order;
    }
    public override void CheckTutorial()
    {
        if (TutorialController.instance.IsAttacking == true)
        {
            Debug.Log("cumpli ataque archer");
            this.CompletedTutorial();
        }
    }
}

[System.Serializable]
public class TutorialDummy : Tutorial
{
    public TutorialDummy(int _order)
    {
        this.Order = _order;
    }
    public override void CheckTutorial()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Debug.Log("C");
            this.CompletedTutorial();
        }
    }
}