using UnityEngine;

public class TriggerTextSwitcher : MonoBehaviour
{
    // Reference to the TextSwitcher script
    public TextSwitcher textSwitcher;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            textSwitcher.DisplayNextText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && textSwitcher == null)
        {
            return;
        }
    }

}
