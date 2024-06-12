using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] UI_FadeScreen fadeScreen;
    public Button button;          
    public float delay = 30f;      
    private CanvasGroup canvasGroup;
    private float timer = 0f;

    void Start()
    {
        // Get the CanvasGroup component attached to the button
        canvasGroup = button.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            // If there is no CanvasGroup component, add one
            canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
        }

        // Set the initial alpha to 0 (fully transparent)
        canvasGroup.alpha = 0f;
        // Disable the button interaction initially
        button.interactable = false;
    }

    void Update()
    {
        // Increase the timer
        timer += Time.deltaTime;

        // Check if the delay time has passed
        if (timer >= delay)
        {
            // Set the alpha to 1 (fully visible)
            canvasGroup.alpha = 1f;
            // Enable the button interaction
            button.interactable = true;
        }
    }

    public void NewGame()
    {
        AudioManager.instance.PlaySFX(25, null);
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithEffect(1.5f));
    }

    IEnumerator LoadSceneWithEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}
