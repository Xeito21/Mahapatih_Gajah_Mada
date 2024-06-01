using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTips : UI_Tooltips
{
    [SerializeField] private TextMeshProUGUI itemNametext;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescriptiontext;
    [SerializeField] private int defaultFontTitleSize = 38;
    public void ShowToolTip(ItemData_Equipment item)
    {

        if(item == null)
            return;
        itemNametext.text = item.namaItem;
        itemTypeText.text = item.tipeEquiment.ToString();
        itemDescriptiontext.text = item.GetDescription();

        AdjustFontSize(itemNametext);
        AdjustPosition();

        gameObject.SetActive(true);

    }   

    public void HideToolTip()
    {
        itemNametext.fontSize = defaultFontTitleSize;
        gameObject.SetActive(false);
    }
}
