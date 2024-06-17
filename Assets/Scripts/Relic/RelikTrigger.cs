using UnityEngine;

public class RelikTrigger : MonoBehaviour
{
    public RelikController relikController;
    public int itemIndex;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX(2, null);
            relikController.ShowUI(true);
            relikController.UpdateInfo(itemIndex);
            SaveManager.instance.SaveGame();
            Destroy(transform.parent.gameObject);
        }
    }
}
