using System.Collections;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_Tooltips
{
    [SerializeField] private TextMeshProUGUI skillTeks;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private float defaultNameFontSize;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowToolTip(string _skillDescription, string _skillName, int _price)
    {
        skillName.text = _skillName;
        skillTeks.text = _skillDescription;
        skillCost.text = "Cost : " + _price;
        AdjustPosition();
        AdjustFontSize(skillName);
        gameObject.SetActive(true);


    }

    public void HideToolTip()
    {
        skillName.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
