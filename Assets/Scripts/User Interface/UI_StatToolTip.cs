using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_Tooltips
{
    [SerializeField] private TextMeshProUGUI descriptionStat;

    public void ShowStatToolTip(string _text)
    {
        descriptionStat.text = _text;

        gameObject.SetActive(true);
    }

    public void HideStatToolTip()
    {
        descriptionStat.text = "";
        gameObject.SetActive(false);
    }
}
