using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Level_Pertama : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Level_ToolTip levelToolTip;
    private int totalReliks = 6;
    private int totalChest = 7;
    [SerializeField] private string namaLevel;
    [TextArea]
    [SerializeField] private string deskripsiLevel;

    private string relikText;
    private string chestText;
    private string progressText;
    private float targetProgress;

    void Start()
    {
        string dataDirPath = "C:/Users/Xei/AppData/LocalLow/NusantaraLegends/Mahapatih_GajahMada/";
        string dataFileName = "data.Mahapatih";
        bool encryptData = false;
        FileDataHandler dataHandler = new FileDataHandler(dataDirPath, dataFileName, encryptData);
        GameData gameData = dataHandler.Load();

        if (gameData != null)
        {
            Pengecekan(gameData);
        }
        else
        {
            Debug.LogError("Failed to load game data.");
        }
    }

    private void Pengecekan(GameData gameData)
    {
        int ownedRelikCount = CountOwnedReliks(gameData);
        int openedChestCount = CountChestOpened(gameData);

        relikText = "Relik: " + ownedRelikCount + " / " + totalReliks;
        chestText = "Chest: " + openedChestCount + " / " + totalChest;
        targetProgress = CalculateTotalProgress(ownedRelikCount, openedChestCount);
        progressText = "Progress: " + targetProgress.ToString("F0") + "%";
    }

    private int CountOwnedReliks(GameData gameData)
    {
        int count = 0;
        foreach (bool hasRelik in gameData.relikOwnership.Values)
        {
            if (hasRelik)
            {
                count++;
            }
        }
        return count;
    }

    private int CountChestOpened(GameData gameData)
    {
        int count = 0;
        foreach (bool hasOpened in gameData.chest.Values)
        {
            if (hasOpened)
            {
                count++;
            }
        }
        return count;
    }

    private float CalculateTotalProgress(int relikCount, int chestCount)
    {
        int totalCollected = relikCount + chestCount;
        int totalItems = totalReliks + totalChest;
        return (float)totalCollected / totalItems * 100f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        levelToolTip.ShowToolTip(deskripsiLevel, namaLevel, relikText, chestText, progressText);
        StartCoroutine(UpdateProgressBar(targetProgress));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        levelToolTip.HideToolTip();
        StopAllCoroutines(); // Hentikan animasi progres saat tooltip disembunyikan
    }

    private IEnumerator UpdateProgressBar(float targetValue)
    {
        float currentValue = 0;
        float duration = 1.0f; // Durasi animasi dalam detik
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(0, targetValue, elapsedTime / duration);
            levelToolTip.totalProgress.text = "Progress: " + currentValue.ToString("F0") + "%";
            yield return null;
        }

        // Pastikan nilai akhir sesuai target
        levelToolTip.totalProgress.text = "Progress: " + targetValue.ToString("F0") + "%";
    }
}
