using TMPro;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI currentAmountText;
    public TextMeshProUGUI requiredAmountText;
    public TextMeshProUGUI gobogText;

    public void UpdateQuestLog(Quest quest)
    {
        titleText.text = quest.title;
        descriptionText.text = quest.description;
        gobogText.text = "Reward: " + quest.gobogReward.ToString();

        if (quest.goal.goalType == GoalType.Kill)
        {
            currentAmountText.text = "Mengalahkan Prajurit " + quest.goal.currentAmount + "/" + quest.goal.requiredAmount;
            requiredAmountText.text = "";
        }
        else if (quest.goal.goalType == GoalType.Gathering)
        {
            currentAmountText.text = "Current: " + quest.goal.currentAmount;
            requiredAmountText.text = "Required: " + quest.goal.requiredAmount;
        }
    }
}
