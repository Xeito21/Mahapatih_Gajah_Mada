using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
    public int keysCurrency;
    public int finalScore;

    public SerializableDictionary<int, bool> prajurit;
    public SerializableDictionary<int, bool> chestRelik;
    public SerializableDictionary<int, bool> chest;
    public SerializableDictionary<string, bool> skillTree;
    public SerializableDictionary<string, int> bagspace;
    public List<string> equipmentId;
    public SerializableDictionary<string,bool> checkpoints;
    public string closestCheckpointId;
    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public SerializableDictionary<string,float> volumeSettings;


    public GameData()
    {
        this.lostCurrencyX = 0;
        this.lostCurrencyY = 0;
        this.lostCurrencyAmount = 0;
        
        this.currency = 0;
        this.keysCurrency = 0;
        this.finalScore = 0;
        prajurit = new SerializableDictionary<int, bool>();
        chestRelik = new SerializableDictionary<int, bool>();
        chest = new SerializableDictionary<int, bool>();
        skillTree = new SerializableDictionary<string, bool>();
        bagspace = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();

        closestCheckpointId = string.Empty;
        checkpoints = new SerializableDictionary<string, bool>();

        volumeSettings = new SerializableDictionary<string, float>();
    }
}
