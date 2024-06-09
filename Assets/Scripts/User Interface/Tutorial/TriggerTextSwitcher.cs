using Ink.Parsed;
using UnityEngine;

public class TriggerTextSwitcher : MonoBehaviour
{
    // Reference to the TextSwitcher script
    public TextSwitcher textSwitcher;
   [SerializeField] private GameObject PopupWindow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PopupWindow.gameObject.SetActive(true);
            textSwitcher.DisplayNextText();
        }
    }
}
