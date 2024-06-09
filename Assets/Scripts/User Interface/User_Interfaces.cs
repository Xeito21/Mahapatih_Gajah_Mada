using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class User_Interfaces : MonoBehaviour,ISaveManager
{
    [Header("Game Over Screen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject restartBtn;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] public GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] public GameObject hudUI;
    [SerializeField] private GameObject WinUI;
    [SerializeField] private GameObject popupUI;

    public TextMeshProUGUI PopupTextInfo;


    public UI_ItemToolTips itemToolTips;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;
    public UI_SkillToolTip skillToolTip;
    public UI_FinishStage finishWindow;

    public static User_Interfaces instance;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;


    void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
            
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
    }
    void Start()
    {
        SwitchTo(hudUI);
        popupUI.SetActive(true);
        itemToolTips.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);
        if(Input.GetKeyDown(KeyCode.V))
            SwitchWithKeyTo(craftUI);
        if(Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(skillTreeUI);
        if(Input.GetKeyDown(KeyCode.N))
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            if(AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(14,null);
            }
            _menu.SetActive(true);

        }

        if(GameManager.instance != null)
        {
            if(_menu == hudUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(false);
        }
    }

        public void SwitchToActive(GameObject _menu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if(_menu != null)
        {
            if(AudioManager.instance != null)
            {
                AudioManager.instance.PlaySFX(14,null);
            }
            _menu.SetActive(true);

        }

        if(GameManager.instance != null)
        {
            if(_menu == hudUI)
                GameManager.instance.PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if(_menu != null)
        {
            if(_menu.activeSelf)
            {
                _menu.SetActive(false);
                CheckForInHUD();
                return;
            }

            SwitchTo(_menu);
        }
    }

    private void CheckForInHUD()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(hudUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());

    }

    IEnumerator EndScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        gameOverText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartBtn.SetActive(true);
    }

    public IEnumerator DisplayPopupText(string text)
    {
        PopupTextInfo.text = text;
        PopupTextInfo.color = Color.white;

        PopupTextInfo.gameObject.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        float duration = 1.0f;
        float timer = 0f;
        Color originalColor = PopupTextInfo.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            PopupTextInfo.color = Color.Lerp(originalColor, Color.clear, timer / duration);
            yield return null;
        }

        PopupTextInfo.color = originalColor;
        PopupTextInfo.gameObject.SetActive(false);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string,float> pair in _data.volumeSettings)
        {
            foreach(UI_VolumeSlider item in volumeSettings)
            {
                if(item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach(UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}