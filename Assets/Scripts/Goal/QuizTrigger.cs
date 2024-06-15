using System.Collections;
using TMPro;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI PopupTextReq;
    [SerializeField] private float enemyCheckRadius = 5f;
    [SerializeField] private LayerMask enemyLayerMask;
    private bool isEnemyNearby = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerManager.instance.keysCurrency == 3)
        {
            if(isEnemyNearby)
            {
                User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Kalahkan prajurit terlebih dahulu!"));
            }
            else
            {
                SpawnQuiz();
            }
        }
        else if (collision.CompareTag("Player"))
            StartCoroutine(User_Interfaces.instance.DisplayPopupText("Membutuhkan tiga kunci, " + "Kunci kamu hanya " + PlayerManager.instance.keysCurrency));
    }

    private void SpawnQuiz()
    {
        QuizManager.instance.StartGameQuiz();
    }

    private void OnDrawGizmosSelected()
    {
        // Menggambar gizmo berbentuk lingkaran untuk deteksi musuh
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyCheckRadius);
    }
}
