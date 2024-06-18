using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_LevelMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private string sceneName = "MainScene";
    private CursorUI setCursor;

    void Start()
    {
        setCursor = GetComponent<CursorUI>();
    }
    public void BackToMenu()
    {
        StartCoroutine(LoadSceneMainMenu(1.5f));
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

    IEnumerator LoadSceneMainMenu(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlaySFX(28, null);
        setCursor.SetCursorPoint();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        setCursor.SetCustomCursor();
    }
}