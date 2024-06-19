using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;
    private DialogueVariables dialogueVariables;
    private InkExternalFunctions inkExternalFunctions;

    [Header("Typing Speed")]
    [SerializeField] private float typingSpeed = 0.02f;
    private Coroutine displayLineCoroutine;
    private bool canContinueToNextLine = false;

    [Header("Load Globals ink file")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator potraitAnimator;
    [SerializeField] private GameObject continueIcon;
    private Animator layoutAnimator;

    [Header("Audio")]
    [SerializeField] private DialogueAudioInfoSO defaultAudioInfo;
    [SerializeField] private DialogueAudioInfoSO[] audioInfos;
    [SerializeField] private bool makePredictable;
    private DialogueAudioInfoSO currentAudioInfo;
    private Dictionary<string, DialogueAudioInfoSO> audioInfoDictionary;
    private AudioSource audioSource;

    [Header("Choice UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;
    private const string SPEAKER_TAG = "speaker";
    private const string POTRAIT_TAG = "potrait";
    private const string LAYOUT_TAG = "layout";
    private const string AUDIO_TAG = "audio";
    public bool dialogueIsPlaying { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one dialogue manager in the scene");
        }
        instance = this;
        dialogueVariables = new DialogueVariables(loadGlobalsJSON);
        inkExternalFunctions = new InkExternalFunctions();
        currentAudioInfo = defaultAudioInfo;
        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);

        layoutAnimator = dialogueBox.GetComponent<Animator>();

        //choice text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
        InitializeAudioInfoDictionary();
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (canContinueToNextLine && currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON, Animator emoteAnimator)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueBox.SetActive(true);
        dialogueVariables.StartListening(currentStory);
        inkExternalFunctions.Bind(currentStory, emoteAnimator);
        displayNameText.text = "???";
        potraitAnimator.Play("default");
        layoutAnimator.Play("left");
        ContinueStory();
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {

            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();
            if (nextLine.Equals("") && !currentStory.canContinue)
            {
                StartCoroutine(ExitDialogueMode());
            }
            else
            {
                //handle tags
                HandleTags(currentStory.currentTags);
                displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            }
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, DialogueAudioInfoSO>();
        audioInfoDictionary.Add(defaultAudioInfo.id, defaultAudioInfo);
        foreach(DialogueAudioInfoSO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.id, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        DialogueAudioInfoSO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if(audioInfo != null)
        {
            this.currentAudioInfo = audioInfo;
        }
        else
        {
            Debug.LogWarning("failed to find audio info for id:" + id);
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        continueIcon.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yield return null; 
            }

            if (letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(typingSpeed);
            }
        }

        dialogueText.maxVisibleCharacters = line.Length;
        canContinueToNextLine = true;
        continueIcon.SetActive(true);
    }



    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;
        if(currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }

            AudioClip soundClip = null;

            if(makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();
                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictableIndex];
                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int) (maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;

                if(pitchRangeInt != 0)
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
                else
                {
                    audioSource.pitch = minPitch;
                }
            }
            else
            {
                int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];
                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }
            audioSource.PlayOneShot(soundClip);
        }
    }

    private void HideChoices()
    {
        foreach (GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriatel parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //switch tag
            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case POTRAIT_TAG:
                    potraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled:" + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        //enable and initialice the choices

        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueVariables.StopListening(currentStory);
        inkExternalFunctions.Unbind(currentStory);
        
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        dialogueText.text = "";
        SetCurrentAudioInfo(defaultAudioInfo.id);
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink variable was found to be null:" + variableName);
        }

        return variableValue;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    public void OnApplicationQuit()
    {
        if(dialogueVariables != null)
        {
            dialogueVariables.SaveVariables();
        }
    }
}
