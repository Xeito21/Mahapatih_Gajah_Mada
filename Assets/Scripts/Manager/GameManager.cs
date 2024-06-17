using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour,ISaveManager
{
    public static GameManager instance;
    private RelikController relikController;

    private Transform player;
    [SerializeField] private Checkpoint[] checkpoints;
    [SerializeField] private string closestCheckpointLoaded;

    [Header("Lost Currency Gobog")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;


    private void Awake()
    {
        if(instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }
    private void Start()
    {
        relikController = FindObjectOfType<RelikController>();
        checkpoints = FindObjectsOfType<Checkpoint>();
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void BackToMenuScene()
    {
        SaveManager.instance.SaveGame();
        string previousSceneName = "MainMenu";
        SceneManager.LoadScene(previousSceneName);
    }

    public void LevelCompleted()
    {
        SaveManager.instance.SaveGame();
        string previousSceneName = "MainMenu";
        SceneManager.LoadScene(previousSceneName);
    }

    public void LoadData(GameData _data)
    {
        StartCoroutine(LoadWithDelay(_data));
    }

    private void LoadCheckPoints(GameData _data)
    {
        foreach (KeyValuePair<string, bool> pair in _data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.checkpointId == pair.Key && pair.Value == true)
                    checkpoint.ActivateCheckPoint();
            }
        }
    }

    private void LoadLostGobogCurrency(GameData _data)
    {
        lostCurrencyAmount = _data.lostCurrencyAmount;
        lostCurrencyX = _data.lostCurrencyX;
        lostCurrencyY = _data.lostCurrencyY;

        if(lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX,lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
        }

        lostCurrencyAmount = 0;
    }


    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(.1f);
        LoadCheckPoints(_data);
        LoadClosestCheckPoint(_data);
        LoadLostGobogCurrency(_data);
    }

    private void LoadClosestCheckPoint(GameData _data)
    {
        if(_data.closestCheckpointId == null)
            return;

        closestCheckpointLoaded = _data.closestCheckpointId;
        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (closestCheckpointLoaded == checkpoint.checkpointId)
            {
                player.position = checkpoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.lostCurrencyAmount = lostCurrencyAmount;
        _data.lostCurrencyX = player.position.x;
        _data.lostCurrencyY = player.position.y;

        if(FindClosestCheckpoint() != null)
            _data.closestCheckpointId = FindClosestCheckpoint().checkpointId;

            
        _data.checkpoints.Clear();
        foreach(Checkpoint checkpoint in checkpoints)
        {
            _data.checkpoints.Add(checkpoint.checkpointId, checkpoint.checkPointStatus);
        }
    }


    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach(var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkpoint.transform.position);

            if(distanceToCheckpoint < closestDistance && checkpoint.checkPointStatus == true)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    public void PauseGame(bool _pause)
    {
        if(_pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
