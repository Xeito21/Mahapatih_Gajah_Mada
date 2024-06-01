using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class QuizManager : MonoBehaviour
{
    Question[] _questions = null;
    public Question[] Questions { get { return _questions; } }
    public bool isFinish = false;
    [SerializeField] Animator timerAnimator = null;
    [SerializeField] TextMeshProUGUI timerText = null;
    [SerializeField] QuizEvents events = null;

    [Header("Panel")]
    [SerializeField] GameObject quizPanel;
    [SerializeField] GameObject ResolutionPanel;

    private List<AnswerData> PickedAnswers = new List<AnswerData>();
    private List<int> FinishedQuestion = new List<int>();
    private int currentQuestion = 0;
    private int timerStateParaHash = 0;

    [Header(("Text Color Timer"))]
    [SerializeField] private Color timerDefaultColor = Color.white;
    [SerializeField] private Color timerHalfWayColor = Color.yellow;
    [SerializeField] private Color timerAlmostOutColor = Color.red;

    public static QuizManager instance;


    private IEnumerator IE_WaitTillNextRound = null;
    private IEnumerator IE_StartTimer = null;
    private bool isFinished
    {
        get
        {
            return (FinishedQuestion.Count < Questions.Length) ? false : true;
        }
    }

    private void OnEnable()
    {
        events.UpdateQuestionAnswer += UpdateAnswers;
    }

    private void OnDisable()
    {
        events.UpdateQuestionAnswer -= UpdateAnswers;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        events.CurrentFinalScore = 0;
    }

    /* private void Start()
    {
        StartGameQuiz();
    }
    */

    public void StartGameQuiz()
    {
        
        events.StartupHighscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        timerDefaultColor = timerText.color;
        LoadQuestion();
        timerStateParaHash = Animator.StringToHash("TimerState");
        var seed = Random.Range(int.MinValue, int.MaxValue);
        Random.InitState(seed);
        Display();
        quizPanel.SetActive(true);
    }

    public void UpdateAnswers(AnswerData newAnswer)
    {
        if (Questions[currentQuestion].GetAnswerType == Question.AnswerType.Single)
        {
            foreach (var answer in PickedAnswers)
            {
                if (answer != newAnswer)
                {
                    answer.Reset();
                }
            }
            PickedAnswers.Clear();
            PickedAnswers.Add(newAnswer);
        }
        else
        {
            bool alreadyPicked = PickedAnswers.Exists(x => x == newAnswer);
            if (alreadyPicked)
            {
                PickedAnswers.Remove(newAnswer);
            }
            else
            {
                PickedAnswers.Add(newAnswer);
            }
        }
    }

    public void RestartGame()
    {
        quizPanel.gameObject.SetActive(false);
        ResolutionPanel.gameObject.SetActive(false);
    }
    

    public void EraseAnswers()
    {
        PickedAnswers = new List<AnswerData>();
    }
    

    private void Display()
    {
        EraseAnswers();
        var question = GetRandomQuestion();

        if(events.UpdateQuestionUI != null)
        {
            events.UpdateQuestionUI(question);
        }
        else
        {
            Debug.LogWarning("Ups! Something went wrong while trying to display new Question UI Data. QuizEvents.UpdateQuestionUI is null. Issue occured in QuizManger.Display() method.");
        }

        if (question.useTimer)
            UpdateTimer(question.useTimer);
    }

    public void Accept()
    {
        UpdateTimer(false);
        bool isCorrect = CheckAnswers();
        FinishedQuestion.Add(currentQuestion);

        UpdateScore((isCorrect) ? Questions[currentQuestion].AddScore : -Questions[currentQuestion].AddScore);

        if (isFinished)
        {
            AudioManager.instance.StopSFX(33);
            SetHighScore();
        }
        var type = (isFinished) ? QuizUIManager.ResolutionScreenType.Finish : (isCorrect) ? QuizUIManager.ResolutionScreenType.Correct : QuizUIManager.ResolutionScreenType.Incorrect;

        if(events.DisplayResolutionScreen !=null)
        {
            events.DisplayResolutionScreen(type, Questions[currentQuestion].AddScore);
        }
        AudioManager.instance.PlaySFX((isCorrect) ? 30 : 32,null);

        if(type != QuizUIManager.ResolutionScreenType.Finish)
        {
            if (IE_WaitTillNextRound != null)
            {
                StopCoroutine(IE_WaitTillNextRound);
            }
            IE_WaitTillNextRound = WaitTillNextRound();
            StartCoroutine(IE_WaitTillNextRound);
        }


    }

    private IEnumerator WaitTillNextRound()
    {
        yield return new WaitForSeconds(GameUtility.ResolutionDelayTime);
        Display();
    }

    Question GetRandomQuestion()
    {
        var randomIndex = GetRandomQuestionIndex();
        currentQuestion = randomIndex;

        return Questions[currentQuestion];
    }
    int GetRandomQuestionIndex()
    {
        var random = 0;
        if (FinishedQuestion.Count < Questions.Length)
        {
            do
            {
                random = Random.Range(0, Questions.Length);
            }while (FinishedQuestion.Contains(random) || random == currentQuestion);
        }
        return random;
    }

    void LoadQuestion()
    {
        Object[] objs = Resources.LoadAll("Questions", typeof(Question));
        _questions = new Question[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            _questions[i] = (Question)objs[i];
        }
    }

    private void UpdateScore(int add)
    {
        events.CurrentFinalScore += add;
        events.ScoreUpdated?.Invoke();
    }

    private void UpdateTimer(bool state)
    {
        switch (state)
        {
            case true:
                IE_StartTimer = StartTimer();
                StartCoroutine(IE_StartTimer);

                timerAnimator.SetInteger(timerStateParaHash, 2);
                break;

            case false:
                if(IE_StartTimer != null)
                {
                    StopCoroutine(IE_StartTimer);
                }

                timerAnimator.SetInteger(timerStateParaHash, 1);
                break;
        }
    }

    private void SetHighScore()
    {
        var highscore = PlayerPrefs.GetInt(GameUtility.SavePrefKey);
        if(highscore < events.CurrentFinalScore)
        {
            PlayerPrefs.SetInt(GameUtility.SavePrefKey, events.CurrentFinalScore);
        }
    }

    private IEnumerator StartTimer()
    {
        var totalTime = Questions[currentQuestion].Timer;
        var timeLeft = totalTime;

        timerText.color = timerDefaultColor;
        while (timeLeft > 0)
        {
            timeLeft--;
            AudioManager.instance.PlaySFX(33,null);

            if(timeLeft < totalTime / 2 && timeLeft > totalTime / 4)
            {
                timerText.color = timerHalfWayColor;
            }
            if(timeLeft < totalTime / 4)
            {
                timerText.color = timerAlmostOutColor;
            }
            timerText.text = timeLeft.ToString();
            yield return new WaitForSeconds(1.0f);
        }
        Accept();
    }

    private bool CheckAnswers()
    {
        if (!CompareAnswers())
        {
            return false;
        }
        return true;
    }

    private bool CompareAnswers()
    {
        if(PickedAnswers.Count > 0)
        {
            List<int> c = Questions[currentQuestion].GetCorrectAnswers();
            List<int> p = PickedAnswers.Select(x => x.AnswerIndex).ToList();

            var f = c.Except(p).ToList();
            var s = p.Except(c).ToList();

            return !f.Any() && !s.Any();
        }
        return false;
    }
}
