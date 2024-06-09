using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextSwitcher : MonoBehaviour
{
    public string[] texts;
    public TextMeshProUGUI displayText;
    public Button nextButton;

    private int currentIndex = 0;

    void Start()
    {
        if (texts.Length >= 0)
        {
            displayText.text = texts[currentIndex];
        }
        else
        {
            // Jika tidak ada teks di array, nonaktifkan game object
            gameObject.SetActive(false);
        }

        nextButton.onClick.AddListener(OnNextButtonClicked);
    }

    void OnNextButtonClicked()
    {
        DisplayNextText();
    }

    public void DisplayNextText()
    {
        currentIndex++;
        if (currentIndex < texts.Length)
        {
            displayText.text = texts[currentIndex];
        }
        else
        {
            // Jika sudah mencapai akhir array, nonaktifkan game object
            gameObject.SetActive(false);
        }
    }
}
