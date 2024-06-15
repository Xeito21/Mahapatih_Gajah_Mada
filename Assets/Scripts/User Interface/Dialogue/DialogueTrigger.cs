using UnityEngine;

[System.Serializable]
public class DialogueCharacter : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Emote Animator")]
    [SerializeField] private Animator emoteAnimator;

    [Header("Ink Json")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange;
    private bool hasFinishedConversation;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
        hasFinishedConversation = false;
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && !hasFinishedConversation)
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON, emoteAnimator);
                hasFinishedConversation = true; // Set conversation as finished
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
