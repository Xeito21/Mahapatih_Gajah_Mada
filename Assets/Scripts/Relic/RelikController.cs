using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RelikController : MonoBehaviour
{
    [System.Serializable]
    public class InfoItem
    {
        public Sprite image;
        public string title;
        public string description;
    }

    public List<InfoItem> infoItems = new List<InfoItem>();

    [SerializeField] private GameObject uiPanel; // Panel UI yang akan diaktifkan/dinonaktifkan
    [SerializeField] private Image displayImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Start()
    {
        // Pastikan UI dimulai dalam keadaan tidak aktif
        uiPanel.SetActive(false);

        // Tampilkan item pertama jika diperlukan
        if (infoItems.Count > 0)
        {
            DisplayInfo(infoItems[0]);
        }
    }

    public void DisplayInfo(InfoItem item)
    {
        if (displayImage != null && titleText != null && descriptionText != null)
        {
            displayImage.sprite = item.image;
            titleText.text = item.title;
            descriptionText.text = item.description;
        }
    }

    public void UpdateInfo(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < infoItems.Count)
        {
            DisplayInfo(infoItems[itemIndex]);
            ShowUI(true);
        }
    }

    public void ShowUI(bool show)
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(show);
        }
    }
}
