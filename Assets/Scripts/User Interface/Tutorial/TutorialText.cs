using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    private static TutorialText instance;
    public string[] texts;
    public string[] animationTriggers;
    public TextMeshProUGUI displayText;
    public Button nextButton;
    public float typingSpeed = 0.05f;
    public float nextButtonDelay = 2.0f;
    public Animator guideAnimator;
    public bool isInteractingWithTutorial { get; private set; }

    private int currentIndex = -1;
    private Coroutine typingCoroutine;
    private bool isTyping = false;

    [SerializeField] private GameObject PopupWindow;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;
    }
    void Start()
    {
        isInteractingWithTutorial = false;
        currentIndex = -1;
        nextButton.gameObject.SetActive(false);

        if (texts.Length > 0)
        {
            DisplayNextText();
        }
        else
        {
            gameObject.SetActive(false);
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    void OnNextButtonClicked()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            displayText.text = texts[currentIndex];
            isTyping = false;
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            DisplayNextText();
        }

        PlayAnimationIfAny();
    }

    public void DisplayNextText()
    {
        PopupWindow.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);

        bool waitForAnimation = false;
        if (currentIndex < texts.Length - 1)
        {
            currentIndex++;
            if (!string.IsNullOrEmpty(animationTriggers[currentIndex]))
            {
                waitForAnimation = true;
            }
            StartTypingText(texts[currentIndex]);
        }
        else
        {
            PopupWindow.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        if (!waitForAnimation)
        {
            isTyping = true;
        }

        // Set interaction state
        isInteractingWithTutorial = true;
    }

    private void PlayAnimationIfAny()
    {
        if (currentIndex < animationTriggers.Length && !string.IsNullOrEmpty(animationTriggers[currentIndex]))
        {
            guideAnimator.SetTrigger(animationTriggers[currentIndex]);
        }
    }

    private void StartTypingText(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    private IEnumerator TypeText(string text)
    {
        displayText.text = "";
        isTyping = true;
        foreach (char letter in text.ToCharArray())
        {
            displayText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;

        StartCoroutine(ShowNextButtonAfterDelay());

        if (currentIndex < animationTriggers.Length && !string.IsNullOrEmpty(animationTriggers[currentIndex]))
        {
            guideAnimator.SetTrigger(animationTriggers[currentIndex]);
        }
    }

    private IEnumerator ShowNextButtonAfterDelay()
    {
        yield return new WaitForSeconds(nextButtonDelay);
        nextButton.gameObject.SetActive(true);
    }

    public static TutorialText GetInstance()
    {
        return instance;
    }
}
