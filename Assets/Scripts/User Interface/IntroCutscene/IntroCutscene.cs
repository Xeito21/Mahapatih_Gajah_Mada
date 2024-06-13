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
        canvasGroup = button.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            canvasGroup = button.gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f;
        button.interactable = false;
    }

    void Update()
    {
        // Increase the timer
        timer += Time.deltaTime;

        if (timer >= delay)
        {
            canvasGroup.alpha = 1f;
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
