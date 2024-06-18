using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot_Relik : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Relik_UI ui;
    [SerializeField] private string itemName;
    [TextArea]
    [SerializeField] private string deskripsiRelik;
    private Image ikonRelik;

    void Start()
    {
        ikonRelik = GetComponentInChildren<Image>();
        ui = GetComponentInParent<Relik_UI>();
        string dataDirPath = "C:/Users/Xei/AppData/LocalLow/NusantaraLegends/Mahapatih_GajahMada/";
        string dataFileName = "data.Mahapatih";
        bool encryptData = false;

        FileDataHandler dataHandler = new FileDataHandler(dataDirPath, dataFileName, encryptData);
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

    private void ShowItem(bool hasItem)
    {
        if (hasItem)
        {
            // Jika pemain memiliki item, tampilkan ikon putih
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
            // Jika pemain tidak memiliki item, ubah warna ikon menjadi hitam
            ikonRelik.color = Color.black;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Tampilkan tooltip sesuai kondisi pemain memiliki atau tidak memiliki item
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
        // Sembunyikan tooltip
        if (ui != null && ui.relikTooltip != null)
        {
            ui.relikTooltip.HideToolTip();
        }
    }

    private bool HasItem()
    {
        // Periksa apakah pemain memiliki item tersebut
        string dataDirPath = "C:/Users/Xei/AppData/LocalLow/NusantaraLegends/Mahapatih_GajahMada/";
        string dataFileName = "data.Mahapatih";
        bool encryptData = false;

        FileDataHandler dataHandler = new FileDataHandler(dataDirPath, dataFileName, encryptData);
        GameData gameData = dataHandler.Load();

        if (gameData != null && gameData.relikOwnership.ContainsKey(itemName))
        {
            return gameData.relikOwnership[itemName];
        }

        return false; // Defaultnya tidak memiliki item
    }
}
