using System.Collections;
using TMPro;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PopupTextReq;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerManager.instance.keysCurrency == 3)
            SpawnQuiz();
        else if (collision.CompareTag("Player"))
            StartCoroutine(User_Interfaces.instance.DisplayPopupText("Membutuhkan tiga kunci, " + "Kunci kamu hanya " + PlayerManager.instance.keysCurrency));
    }

    private void SpawnQuiz()
    {
        QuizManager.instance.StartGameQuiz();
    }


}
