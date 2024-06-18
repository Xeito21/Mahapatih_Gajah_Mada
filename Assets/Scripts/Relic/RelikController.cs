using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RelikController : MonoBehaviour, ISaveManager
{
    [System.Serializable]
    public class InfoItem
    {
        public int relikID;
        public Sprite image;
        public string title;
        [TextArea]
        public string description;
        public bool hasItem = false;
    }

    public List<InfoItem> infoItems = new List<InfoItem>();

    [SerializeField] private GameObject uiPanel;
    [SerializeField] private Image displayImage;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Start()
    {
        uiPanel.SetActive(false);
        if (infoItems.Count > 0)
        {
            DisplayInfo(infoItems[0]);
        }
        CheckItemStatus();
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
            infoItems[itemIndex].hasItem = true;
            CheckItemStatus();

        }
    }

    public void ShowUI(bool show)
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(show);
        }
    }

    public void LoadData(GameData _data)
    {
        foreach (var item in infoItems)
        {
            if (_data.relikOwnership.TryGetValue(item.title, out bool value))
            {
                item.hasItem = value;
            }
        }
        CheckItemStatus(); 
    }

    public void SaveData(ref GameData _data)
    {
        foreach (var item in infoItems)
        {
            if (_data.relikOwnership.ContainsKey(item.title))
            {
                _data.relikOwnership[item.title] = item.hasItem;
            }
            else
            {
                _data.relikOwnership.Add(item.title, item.hasItem);
            }
        }
    }

    private void CheckItemStatus()
    {
        foreach (var item in infoItems)
        {
            if (item.hasItem)
            {
                Debug.Log($"Item {item.title} telah dimiliki.");
                
            }
        }
    }
}
