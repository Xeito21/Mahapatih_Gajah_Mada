using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerData : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI infoTextObject = null;
    [SerializeField] Image toggle = null;

    [Header("Textures")]
    [SerializeField] Sprite uncheckToggle = null;
    [SerializeField] Sprite checkToggle = null;

    [Header("References")]
    [SerializeField] QuizEvents events = null;

    private RectTransform _rect = null;
    public RectTransform Rect
    {
        get
        {
            if(_rect == null)
            {
                _rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }
            return _rect;
        }
    }

    private int _answerIndex = -1;
    public int AnswerIndex { get { return _answerIndex; } }

    private bool Checked = false;

    public void UpdateData(string info, int index)
    {
        infoTextObject.text = info;
        _answerIndex = index;
    }

    public void Reset()
    {
        Checked = false;
        UpdateUI();
    }

    public void SwitchState()
    {
        Checked = !Checked;
        AudioManager.instance.PlaySFX(29,null);
        UpdateUI();

        if(events.UpdateQuestionAnswer != null)
        {
            events.UpdateQuestionAnswer(this);
        }

    }

    public void UpdateUI()
    {
        if (toggle == null)
            return;
        toggle.sprite = (Checked) ? checkToggle : uncheckToggle;
    }
}
