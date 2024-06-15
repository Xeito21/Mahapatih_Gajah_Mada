using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Quest quest;
    public PlayerManager playerManager;
    public GameObject questWindow;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI questOnProgressText;
    [SerializeField] private TextMeshProUGUI textQuestDiambil;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI gobogText;
    public TextMeshProUGUI keysText;
    public TextMeshProUGUI TextAmount;
    public GameObject acceptBtn;
    public GameObject closeBtn;
    [SerializeField] private GameObject gatheringItemPrefab;

    [Header("Pop-up Text")]
    public TextMeshProUGUI popUpText;
    public float popUpTextDuration = 2f;

    public static QuestGiver instance;

    private void Awake()
    {
        instance = this;
    }

    public void OpenQuestWindow()
    {
        questWindow.SetActive(true);
        titleText.text = quest.title;

        if (quest.goal.goalType == GoalType.Kill)
        {
            descriptionText.text = quest.description;
            if (quest.isActive)
            {
                TextAmount.text = "Mengalahkan Prajurit " + quest.goal.currentAmount + "/" + quest.goal.requiredAmount;
            }
        }
        else if (quest.goal.goalType == GoalType.Gathering)
        {
            descriptionText.text = quest.description;
            if (quest.isActive)
                TextAmount.text = "Gathered: " + quest.goal.currentAmount + "/" + quest.goal.requiredAmount;
        }

        gobogText.text = "Reward: " + quest.gobogReward.ToString();
        keysText.text = "Reward: " + quest.keysReward.ToString();

        Debug.Log($"Quest isCompleted: {quest.isCompleted}");
        Debug.Log($"Quest isActive: {quest.isActive}");

        // Check if the quest is completed and hide the accept button if true
        if (quest.isCompleted)
        {
            acceptBtn.SetActive(false);
            TextAmount.text = "Quest sudah kamu selesaikan";
            questOnProgressText.text = "<color=green>Quest Completed!</color>";
        }
        else if (quest.isActive)
        {
            acceptBtn.SetActive(false);
            questOnProgressText.text = "<color=green>Sedang Dikerjakan!</color>";
        }
        else
        {
            acceptBtn.SetActive(true);
        }
    }

    public void DisplayPopUpText(string title, string amount)
    {
        StartCoroutine(FadeOutPopUpText(title, amount));
    }

    private IEnumerator FadeOutPopUpText(string title, string amount)
    {
        popUpText.gameObject.SetActive(true);
        popUpText.text = $"{title}\n{amount}";

        yield return new WaitForSeconds(popUpTextDuration);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            popUpText.color = new Color(popUpText.color.r, popUpText.color.g, popUpText.color.b, 1 - elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        popUpText.gameObject.SetActive(false);
    }

    private IEnumerator DisplayTextForDuration()
    {
        textQuestDiambil.gameObject.SetActive(true);
        textQuestDiambil.text = quest.title + " Telah diambil!";

        yield return new WaitForSeconds(popUpTextDuration);

        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            textQuestDiambil.color = new Color(textQuestDiambil.color.r, textQuestDiambil.color.g, textQuestDiambil.color.b, 1 - elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textQuestDiambil.gameObject.SetActive(false);
    }

    public void AcceptQuest()
    {
        if (!quest.isActive && !quest.isCompleted)
        {
            AudioManager.instance.PlaySFX(39, null);
            StartCoroutine(DisplayTextForDuration());
            questWindow.SetActive(false);
            quest.isActive = true;
            playerManager.quest = quest;
            quest.goal.UpdateUIQuest();
            if (gatheringItemPrefab != null && quest.goal.goalType == GoalType.Gathering)
            {
                gatheringItemPrefab.SetActive(true);
            }
        }
        else
        {
            acceptBtn.SetActive(false);
            questOnProgressText.text = "<color=green>Sedang Dikerjakan!</color>";
        }
    }

    public void InitializeQuest(Quest quest)
    {
        this.quest = quest;
    }

    public void CloseBtnQuestWindow()
    {
        questWindow.SetActive(false);
    }
}
