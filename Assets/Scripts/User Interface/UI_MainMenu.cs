using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueBtn;
    [SerializeField] private TextMeshProUGUI buttonText;
    //[SerializeField] private ScrollView[] scrollView;
    [SerializeField] UI_FadeScreen fadeScreen;

    private CursorUI setCursor;

    private void Start()
    {
        setCursor = GetComponent<CursorUI>();
        if (SaveManager.instance.HasSavedData() == false)
            continueBtn.SetActive(false);
    }


    public void ContinueGame()
    {
        AudioManager.instance.PlaySFX(25, null);
        StartCoroutine(LoadSceneWithEffect(1.5f));
    }

    public void NewGame()
    {
        AudioManager.instance.PlaySFX(25, null);
        SaveManager.instance.DeleteSavedData();
        StartCoroutine(LoadSceneWithEffect(1.5f));
    }

    public void LevelMenu()
    {
        StartCoroutine(LoadSceneLevelMenu(1.5f));
    }

    public void RelikMenu()
    {
        StartCoroutine(LoadSceneRelikMenu(1.5f));
    }

    public void IntroCutscene()
    {
        StartCoroutine(LoadSceneWithEffect2(1.5f));
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        StartCoroutine(LoadSceneWithEffect2(1.5f));
        SceneManager.LoadScene(0);
    }
    public void ExitGame()
    {
        AudioManager.instance.PlaySFX(28, null);
        Application.Quit();
    }

    IEnumerator LoadSceneWithEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneWithEffect2(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
    }

    IEnumerator LoadSceneLevelMenu(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(5);
    }

    IEnumerator LoadSceneRelikMenu(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(4);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(28, null);
        setCursor.SetCursorPoint();
        if (buttonText != null)
        {
            buttonText.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        setCursor.SetCustomCursor();
    }
}

