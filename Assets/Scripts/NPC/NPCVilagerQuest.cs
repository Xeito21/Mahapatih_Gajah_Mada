using UnityEngine;

public class NPCVilagerQuest : MonoBehaviour
{
    [SerializeField] private GameObject visualCue;
    [SerializeField] private NPCQuestData npcQuestData;

    private bool isPlayerInside = false;

    private void Start()
    {
        // Instantiate a new quest for each NPC
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            visualCue.SetActive(true);
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            visualCue.SetActive(false);
            isPlayerInside = false;
        }
    }

    private void Update()
    {
        if (isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            QuestGiver.instance.InitializeQuest(npcQuestData.quest);
            QuestGiver.instance.OpenQuestWindow();
        }
    }

}
