using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot_Relik : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Relik_UI ui;
    [SerializeField] public string itemName;
    [TextArea]
    [SerializeField] private string deskripsiRelik;
    private Image ikonRelik;

    void Start()
    {
        ikonRelik = GetComponentInChildren<Image>();
        ui = GetComponentInParent<Relik_UI>();

        LoadGame();
    }

    private void LoadGame()
    {
        string fileName = "data.Mahapatih";
        bool encryptData = false;

        // Menggunakan Application.persistentDataPath untuk path direktori
        string dataDirPath = Application.persistentDataPath;
        FileDataHandler dataHandler = new FileDataHandler(dataDirPath, fileName, encryptData);
        GameData gameData = dataHandler.Load();

        if (gameData != null)
        {
            if (gameData.relikOwnership.ContainsKey(itemName) && gameData.relikOwnership[itemName])
            {
                ShowItem(true);
            }
            else
            {
                ShowItem(false);
            }
        }
        else
        {
            Debug.LogError("Failed to load game data.");
        }
    }

    public void ShowItem(bool hasItem)
    {
        if (hasItem)
        {
            Sprite itemSprite = Resources.Load<Sprite>("Sprites/" + itemName);
            if (itemSprite != null)
            {
                ikonRelik.sprite = itemSprite;
                ikonRelik.color = Color.white;
            }
            else
            {
                Debug.LogError("Sprite for item " + itemName + " not found in Resources.");
            }
        }
        else
        {
            ikonRelik.color = Color.black;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (ui != null && ui.relikTooltip != null)
        {
            if (HasItem())
            {
                ui.relikTooltip.ShowToolTip(deskripsiRelik, itemName);
            }
            else
            {
                ui.relikTooltip.ShowToolTip("Relik belum ditemukan", "???");
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ui != null && ui.relikTooltip != null)
        {
            ui.relikTooltip.HideToolTip();
        }
    }

    private bool HasItem()
    {
        string fileName = "data.Mahapatih";
        bool encryptData = false;
        string dataDirPath = Application.persistentDataPath;
        FileDataHandler dataHandler = new FileDataHandler(dataDirPath, fileName, encryptData);
        GameData gameData = dataHandler.Load();

        if (gameData != null && gameData.relikOwnership.ContainsKey(itemName))
        {
            return gameData.relikOwnership[itemName];
        }

        return false;
    }

}