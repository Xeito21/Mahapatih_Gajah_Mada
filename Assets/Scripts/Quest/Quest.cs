using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public bool isCompleted;
    public int questID;
    public string title;
    public string description;
    public int gobogReward;
    public int keysReward;
    public int questIndex;

    public QuestGoal goal;


    public void Complete()
    {
        isActive = false;
        isCompleted = true;
        goal.currentAmount = 0; // reset currentAmount
        Debug.Log(title + " was Complete");
        goal.UpdateUIQuest();
    }

}
