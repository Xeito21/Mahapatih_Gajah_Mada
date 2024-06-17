using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadRelik : MonoBehaviour
{
    void Start()
    {
        string dataDirPath = "C:/Users/Xei/AppData/LocalLow/NusantaraLegends/Mahapatih_GajahMada/";
        string dataFileName = "data.Mahapatih";
        bool encryptData = false;

        FileDataHandler dataHandler = new FileDataHandler(dataDirPath, dataFileName, encryptData);
        GameData gameData = dataHandler.Load();

        if (gameData != null)
        {
            Debug.Log("Currency: " + gameData.currency);
            Debug.Log("Keys Currency: " + gameData.keysCurrency);
            Debug.Log("Final Score: " + gameData.finalScore);
        }
        else
        {
            Debug.LogError("Failed to load game data.");
        }
    }
}
