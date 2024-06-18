using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Relik_Tooltip : UI_Tooltips
{
    [SerializeField] private TextMeshProUGUI judulRelik;
    [SerializeField] private TextMeshProUGUI deskripsiRelik;
    [SerializeField] private float defaultNameFontSize;

    public void ShowToolTip(string _deskripsiRelik, string _judulRelik)
    {
        judulRelik.text = _judulRelik;
        deskripsiRelik.text = _deskripsiRelik;
        AdjustPosition();
        AdjustFontSize(judulRelik);
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        judulRelik.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
