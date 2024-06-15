using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

[Serializable()]
public struct QuizUIManagerParamters
{
    [Header("Answers Options")]
    [SerializeField] float margins;
    public float Margins { get { return margins; } }

    [Header("Resolution Screen Options")]
    [SerializeField] Color correctBGColor;
    public Color CorrectBGColor { get { return correctBGColor; } }
    [SerializeField] Color incorrectBGColor;
    public Color IncorrectBGColor { get { return incorrectBGColor; } }
    [SerializeField] Color finalBGColor;
    public Color FinalBGColor { get { return finalBGColor; } }
}
[Serializable()]
public struct UIElements
{
    [SerializeField] RectTransform answersContentArea;
    public RectTransform AnswerContentArea { get { return answersContentArea; } }
    [SerializeField] TextMeshProUGUI questionInfoTextObject;
    public TextMeshProUGUI QuestionInfoTextObject { get {  return questionInfoTextObject; } }

    [SerializeField] TextMeshProUGUI scoreText;
    public TextMeshProUGUI ScoreText { get { return scoreText; } }

    [Space]
    [SerializeField] Animator resolutionScreenAnimator;
    public Animator ResolutionScreenAnimator { get { return resolutionScreenAnimator; } }

    [SerializeField] Image resolutionBG;
    public Image ResolutionBG { get { return resolutionBG; } }

    [SerializeField] TextMeshProUGUI resolutionStateInfoText;
    public TextMeshProUGUI ResolutionStateInfoText { get { return resolutionStateInfoText; } }

    [SerializeField] TextMeshProUGUI resolutionScoreText;
    public TextMeshProUGUI ResolutionScoreText { get { return resolutionScoreText; } }

    [Space]
    [SerializeField] TextMeshProUGUI highScoreText;
    public TextMeshProUGUI HighScoreText { get { return highScoreText; } }
    [SerializeField] CanvasGroup mainCanvasGroup;
    public CanvasGroup MainCanvasGroup { get { return mainCanvasGroup; } }
    [SerializeField] RectTransform finishUIElements;
    public RectTransform FinishUIElements { get { return finishUIElements; } }
}
public class QuizUIManager : MonoBehaviour, ISaveManager
{
    public enum ResolutionScreenType { Correct, Incorrect, Finish }

    [Header("References")]
    [SerializeField] QuizEvents events = null;

    [Header("UI Elements (Prefab)")]
    [SerializeField] AnswerData answerPrefab = null;
    [SerializeField] UIElements uiElements = new UIElements();

    [Header("Panel")]
    [SerializeField] private GameObject resolutionWindow;

    [Space]
    [SerializeField] QuizUIManagerParamters parameters = new QuizUIManagerParamters();
    List<AnswerData> currentAnswer = new List<AnswerData>();
    private int resStateParaHash = 0;
    private IEnumerator IE_DisplayTimeResolution = null;
    public static QuizUIManager instance;

    private void OnEnable()
    {
        events.UpdateQuestionUI += UpdateQuestionUI;
        events.DisplayResolutionScreen += DisplayResolution;
        events.ScoreUpdated += UpdateScoreUI;
    }

    private void OnDisable()
    {
        events.UpdateQuestionUI -= UpdateQuestionUI;
        events.DisplayResolutionScreen -= DisplayResolution;
        events.ScoreUpdated -= UpdateScoreUI;
    }

    private void Start()
    {
        UpdateScoreUI();
        resStateParaHash = Animator.StringToHash("ScreenState");
    }

    private void DisplayResolution(ResolutionScreenType type, int score)
    {
        UpdateResUI(type, score);
        uiElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 2);
        uiElements.MainCanvasGroup.blocksRaycasts = false;

        if(type != ResolutionScreenType.Finish)
        {
            if(IE_DisplayTimeResolution != null)
            {
                StopCoroutine(IE_DisplayTimeResolution);
            }
            IE_DisplayTimeResolution = DisplayTimeResolution();
            StartCoroutine(IE_DisplayTimeResolution);
        }
    }

    private IEnumerator DisplayTimeResolution()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        uiElements.ResolutionScreenAnimator.SetInteger(resStateParaHash, 1);
        uiElements.MainCanvasGroup.blocksRaycasts = true;
    }

    private void UpdateResUI(ResolutionScreenType type, int score)
    {
        switch (type)
        {
            case ResolutionScreenType.Correct:
                uiElements.ResolutionBG.color = parameters.CorrectBGColor;

                uiElements.ResolutionStateInfoText.text = "Benar";
                uiElements.ResolutionScoreText.text = "+" + score;
                break;
            case ResolutionScreenType.Incorrect:
                uiElements.ResolutionBG.color = parameters.IncorrectBGColor;

                uiElements.ResolutionStateInfoText.text = "Salah";
                uiElements.ResolutionScoreText.text = "-" + score;
                break;
            case ResolutionScreenType.Finish:
                QuizManager.instance.isFinish = true;
                uiElements.FinishUIElements.gameObject.SetActive(true);
                resolutionWindow.gameObject.SetActive(false);
                uiElements.ResolutionBG.color = parameters.FinalBGColor;
                uiElements.ResolutionStateInfoText.gameObject.SetActive(false);
                uiElements.ResolutionScoreText.gameObject.SetActive(false);
                break;
        }
    }

    /* private IEnumerator CalculateScore()
    {
        var scoreValue = 0;
        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue++;
            uiElements.ResolutionScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }
    */

    private void UpdateQuestionUI(Question question)
    {
        uiElements.QuestionInfoTextObject.text = question.Info;
        CreateAnswers(question);
    }

    private void CreateAnswers(Question question)
    {
        EraseAnswers();

        float offset = 0 - parameters.Margins;
        for (int i = 0; i < question.Answers.Length; i++)
        {
            AnswerData newAnswer = (AnswerData)Instantiate(answerPrefab, uiElements.AnswerContentArea);
            newAnswer.UpdateData(question.Answers[i].Info, i);

            newAnswer.Rect.anchoredPosition = new Vector2(0, offset);

            offset -= (newAnswer.Rect.sizeDelta.y + parameters.Margins);
            uiElements.AnswerContentArea.sizeDelta = new Vector2(uiElements.AnswerContentArea.sizeDelta.x, offset * -1);

            currentAnswer.Add(newAnswer);
        }
    }

    private void EraseAnswers()
    {
        foreach (var answer in currentAnswer)
        {
            Destroy(answer.gameObject);
        }
        currentAnswer.Clear();
    }

    private void UpdateScoreUI()
    {
        uiElements.ScoreText.text = "Score: " + events.CurrentFinalScore;
    }

    public int GetScoreFinal() => events.CurrentFinalScore;

    public void LoadData(GameData _data)
    {
        this.events.CurrentFinalScore = _data.finalScore;
    }

    public void SaveData(ref GameData _data)
    {
        _data.finalScore = this.events.CurrentFinalScore;
    }
}
