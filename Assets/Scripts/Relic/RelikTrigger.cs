using UnityEngine;

public class RelikTrigger : MonoBehaviour
{
    public RelikController relikController;
    public int itemIndex; // Index untuk item yang ingin ditampilkan

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(2, null);
            relikController.ShowUI(true);
            relikController.UpdateInfo(itemIndex);
            Destroy(transform.parent.gameObject);
        }
    }
}
