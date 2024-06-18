using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Level_ToolTip : UI_Tooltips
{
    [SerializeField] private TextMeshProUGUI namaLevel;
    [SerializeField] private Image[] stars;
    [SerializeField] private float defaultNameFontSize;
    public TextMeshProUGUI deskripsiLevel;
    public TextMeshProUGUI jumlahRelik;
    public TextMeshProUGUI jumlahChest;
    public TextMeshProUGUI totalProgress;

    public void ShowToolTip(string _deskripsiLevel, string _namaLevel, string _jumlahRelik, string _jumlahChest, string _totalProgress, int finalScore)
    {
        namaLevel.text = _namaLevel;
        deskripsiLevel.text = _deskripsiLevel;
        jumlahRelik.text = _jumlahRelik;
        jumlahChest.text = _jumlahChest;
        totalProgress.text = _totalProgress;
        AdjustPosition();
        AdjustFontSize(namaLevel);
        UpdateStars(finalScore);
        gameObject.SetActive(true);
    }

    public void HideToolTip()
    {
        namaLevel.fontSize = defaultNameFontSize;
        gameObject.SetActive(false);
    }

    private void UpdateStars(int finalScore)
    {
        int starCount = CalculateStars(finalScore);
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].enabled = i < starCount;
        }
    }

    private int CalculateStars(int score)
    {
        if (score >= 2500)
        {
            return 3;
        }
        else if (score >= 1000)
        {
            return 2;
        }
        else if (score >= 800)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
