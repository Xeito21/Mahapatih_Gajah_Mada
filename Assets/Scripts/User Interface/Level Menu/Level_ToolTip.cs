using UnityEngine;
using TMPro;

public class Level_ToolTip : UI_Tooltips
{
    [SerializeField] private TextMeshProUGUI namaLevel;
    public TextMeshProUGUI deskripsiLevel;
    public TextMeshProUGUI jumlahRelik;
    public TextMeshProUGUI jumlahChest;
    public TextMeshProUGUI totalProgress;
    [SerializeField] private float defaultNameFontSize;

    public void ShowToolTip(string _deskripsiLevel, string _namaLevel, string _jumlahRelik, string _jumlahChest, string _totalProgress)
    {
        namaLevel.text = _namaLevel;
        deskripsiLevel.text = _deskripsiLevel;
        jumlahRelik.text = _jumlahRelik;
        jumlahChest.text = _jumlahChest;
        totalProgress.text = _totalProgress;
        AdjustPosition();
        AdjustFontSize(namaLevel);
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        namaLevel.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }
}
