using UnityEngine;

[System.Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;

    public static Quest quest;
    public string itemToGather;

    public bool isReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public void QuestType()
    {
        if (goalType == GoalType.Kill || goalType == GoalType.Gathering)
        {
            currentAmount++;

            if (goalType == GoalType.Kill)
                Debug.Log("Enemy killed! Current amount: " + currentAmount);
            else if (goalType == GoalType.Gathering)
                Debug.Log("Item Gathered! Current amount: " + currentAmount);

            UpdateUIQuest();
            QuestGiver.instance.DisplayPopUpText(goalType == GoalType.Kill ? "Prajurit Terbunuh " : "Barang Terkumpul ", currentAmount.ToString() + "/" + requiredAmount);
        }
    }



    public void UpdateUIQuest()
    {
        if (QuestGiver.instance != null && QuestGiver.instance.TextAmount != null)
        {
            QuestGiver.instance.TextAmount.text = $"{currentAmount}/{requiredAmount}";
        }
    }
}



    public enum GoalType
{
    Kill,
    Gathering
}