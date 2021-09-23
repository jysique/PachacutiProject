using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UCTempOption : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI stats1;
    [SerializeField] TextMeshProUGUI stats2;
    [SerializeField] Toggle toggle;
    UnitCombat uc;
    public UnitCombat UnitCombat
    {
        get { return uc; }
    }
    public void InitOption(UnitCombat unitCombat)
    {
        uc = unitCombat;
        image.sprite = unitCombat.Picture;
        description.text = unitCombat.UnitName + "\nQUANTITY: " + unitCombat.Quantity;
        stats1.text = "ATAQUE: " + unitCombat.Attack + "\n" +
            "DEFENSA: " + unitCombat.Defense + "\n" +
            "RANGO: " + unitCombat.Range;
        stats2.text = "EVASION: " + unitCombat.Evasion + "\n" +
            "PRECISION: " + unitCombat.Precision + "\n";
        toggle.isOn = false;
    }
    public bool IsSelected()
    {
        return (toggle.isOn == true);
    }
}
