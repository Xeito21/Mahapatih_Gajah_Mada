using UnityEngine;

public class RelikTrigger : MonoBehaviour
{
    public RelikController relikController;
    public int itemIndex; // Index untuk item yang ingin ditampilkan

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Panggil metode UpdateInfo dari GameController dengan indeks item yang sesuai
            relikController.ShowUI(true);
            relikController.UpdateInfo(itemIndex);
        }
    }
}
