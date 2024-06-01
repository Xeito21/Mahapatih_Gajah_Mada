using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Quest : MonoBehaviour
{
    public QuestGoal questGoal;
    public static UI_Quest instance;


    void Awake()
    {
        instance = this;
    }
    public void UpdateQuest()
    {
        if (QuestGiver.instance != null && QuestGiver.instance.TextAmount != null)
        {
            QuestGiver.instance.TextAmount.text = "Current: " + questGoal.currentAmount;
        }
    }
}
