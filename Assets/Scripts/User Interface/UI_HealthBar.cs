using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private CharacterStats myStats;
    private RectTransform myTransform;
    private Slider slider;

    protected virtual void Start()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();
        UpdateHealthUI();
        myStats.onHealthChanged += UpdateHealthUI;
        if (myTransform == null)
        {
            Debug.LogError("RectTransform is not found!");
            return;
        }

        if (slider == null)
        {
            Debug.LogError("Slider is not found in children!");
            return;
        }

        if (myStats == null)
        {
            Debug.LogError("CharacterStats component is not found in parent!");
            return;
        }
        
        slider.value = myStats.GetMaxHealthValue();
    }

    private void UpdateHealthUI()
    {
        if (myStats != null && slider != null)
        {
            slider.maxValue = myStats.GetMaxHealthValue();
            slider.value = myStats.currentHealth;
        }
    }

    private void FlipUI()
    {
        if (myTransform != null)
        {
            myTransform.Rotate(0, 180, 0);
        }
    }

    private void OnDisable()
    {
        if (myStats != null)
        {
            FlipUI();
            myStats.onHealthChanged -= UpdateHealthUI;
        }
    }
}
