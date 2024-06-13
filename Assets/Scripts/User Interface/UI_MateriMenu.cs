using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_MateriMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private string sceneName = "MainMenu";
    private CursorUI setCursor;

    void Start()
    {
        setCursor = GetComponent<CursorUI>();
    }
    public void BackToMenu()
    {
        StartCoroutine(LoadSceneWithEffect(1.5f));
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

    IEnumerator LoadSceneWithEffect(float _delay)
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }
}
