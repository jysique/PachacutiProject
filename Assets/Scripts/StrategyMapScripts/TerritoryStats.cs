using UnityEngine;
using TMPro;
public class TerritoryStats : MonoBehaviour
{
    [SerializeField] private TextMeshPro populationTxt;
    [SerializeField] private TextMeshPro nameTxt;
    public void InitStats(Territory territory)
    {
        nameTxt.text = territory.name;
        if (territory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            //populationTxt.enabled = false;
            populationTxt.gameObject.SetActive(false);
        }

    }
    public void UpdatePopulation(Territory territory)
    {
        populationTxt.text = territory.Population + "/" + territory.LimitPopulation;
    }
}

