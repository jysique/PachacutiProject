using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;


public class GroupClassContainer : MonoBehaviour
{
    Animator animator;
    public UnitGroup stats;
    public bool active;

    private void Awake()
    {
        animator = transform.GetChild(2).GetComponent<Animator>();
    }

    public void OpenResume()
    {
        CombatManager.instance.MakeBattleResume(CombatManager.instance.ActualUnit(), stats, this);
    }

    public void ReceiveDamage() {
        StartCoroutine(CombatManager.instance.MakeDamage(CombatManager.instance.ActualUnit(),stats));
    }
    
    

    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(1))
        {
            //print(stats.Active);
            //print(CombatManager.instance.blockScreen);
        }

        if (Input.GetMouseButtonDown(1) && !CombatManager.instance.blockScreen)
        {
            bool interactable = stats.TypePlayer != Territory.TYPEPLAYER.PLAYER || stats.Active != true;
            CombatManager.instance.menu.transform.GetChild(0).gameObject.GetComponent<Button>().interactable = !interactable;
            CombatManager.instance.menu.transform.GetChild(1).gameObject.GetComponent<Button>().interactable = !interactable;
            CombatManager.instance.menu.transform.GetChild(2).gameObject.GetComponent<Button>().interactable = !interactable;

            AudioManager.instance.ReadAndPlaySFX("context_menu");
            CombatManager.instance.selectedUnit = stats;
            Vector3 newpos = stats.UnitsGO.transform.position;
            CombatManager.instance.menu.SetActive(true);
            CombatManager.instance.menu.transform.position = new Vector3(newpos.x + 1, newpos.y - 1, newpos.z);
            CombatManager.instance.isMenu = true;
        }
        else
        {
            //print("no se puede");
        }
    }
    

    public void IdleAnimation()
    {
        animator.SetBool("attack", false);
    }
    public void AttackAnimation()
    {
        animator.SetBool("attack", true);
    }
    public void IsLeft()
    {
        animator.SetBool("isLeft", true);
    }
    public void IsRight()
    {
        animator.SetBool("isLeft", false);
    }
}
