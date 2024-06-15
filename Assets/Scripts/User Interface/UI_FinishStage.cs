using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UI_FinishStage : MonoBehaviour
{
    private float finalScore;
    [SerializeField] QuizEvents events = null;
    [SerializeField] int increaseRate = 100;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject[] stars;

    void Update()
    {
        StartCoroutine(CalculateScore());
        finalScore = events.CurrentFinalScore;
        finalScoreText.text = ("Score : " + events.CurrentFinalScore).ToString();
        SummaryStars();
    }

    private void SummaryStars()
    {
        if (finalScore > 2500)
        {
            DisplayStars(3, Color.white);
        }
        else if (finalScore > 2000)
        {
            DisplayStars(2, Color.white);
            stars[2].GetComponent<Image>().color = Color.black;
        }
        else if (finalScore > 1000)
        {
            DisplayStars(1, Color.white);
            stars[1].GetComponent<Image>().color = Color.black;
            stars[2].GetComponent<Image>().color = Color.black;
        }
        else
        {
            DisplayStars(0, Color.black);
        }
    }

    private void DisplayStars(int numStars, Color color)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < numStars)
            {
                stars[i].SetActive(true);
                stars[i].GetComponent<Image>().color = color;
            }
            else
            {
                stars[i].SetActive(false);
            }
        }
    }

    private IEnumerator CalculateScore()
    {
        var scoreValue = 0;
        int updateFrequency = Mathf.Max(events.CurrentFinalScore / increaseRate, 5);
        while (scoreValue < events.CurrentFinalScore)
        {
            scoreValue += updateFrequency; 
            if (scoreValue > events.CurrentFinalScore)
                scoreValue = events.CurrentFinalScore;

            finalScoreText.text = scoreValue.ToString();

            yield return null;
        }
    }
}

