
using UnityEngine;
using UnityEngine.UI;

public class Slot_Relik : MonoBehaviour
{
    private Image ikonRelik;
    [SerializeField] private string itemName;
    void Start()
    {
        ikonRelik = GetComponentInChildren<Image>();
        string dataDirPath = "C:/Users/Xei/AppData/LocalLow/NusantaraLegends/Mahapatih_GajahMada/";
        string dataFileName = "data.Mahapatih";
        bool encryptData = false;

        FileDataHandler dataHandler = new FileDataHandler(dataDirPath, dataFileName, encryptData);
        GameData gameData = dataHandler.Load();

        if (gameData != null)
        {
            if (gameData.relikOwnership.ContainsKey(itemName) && gameData.relikOwnership[itemName])
            {
                ShowItem(itemName, true);
            }
            else
            {
                ShowItem(itemName, false);
            }
        }
        else
        {
            Debug.LogError("Failed to load game data.");
        }
    }


    private void ShowItem(string itemName, bool hasItem)
    {
        Sprite itemSprite = Resources.Load<Sprite>("Sprites/" + itemName);
        if (itemSprite != null)
        {
            ikonRelik.sprite = itemSprite;

            if (hasItem)
            {
                ikonRelik.color = Color.white;
            }
            else
            {
                ikonRelik.color = Color.black;
            }
        }
        else
        {
            Debug.LogError("Sprite for item " + itemName + " not found in Resources.");
        }
    }
}
