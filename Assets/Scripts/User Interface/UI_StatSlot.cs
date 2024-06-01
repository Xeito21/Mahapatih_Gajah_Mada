using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    private User_Interfaces ui;
    [SerializeField] private string statName;
    [SerializeField] private tipeStat statType;
    [SerializeField] TextMeshProUGUI statValueText;
    [SerializeField] TextMeshProUGUI statNameText;
    [TextArea]
    [SerializeField] private string statDescription;

    void OnValidate()
    {
        gameObject.name = "Stat -" + statName;

        if(statName != null)
            statNameText.text = statName;
    }

    private void Start()
    {
        UpdateStatValueUI();
        ui = GetComponentInParent<User_Interfaces>();
    }


    public void UpdateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        if(playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).GetValue().ToString();



            if(statType == tipeStat.maxHealth)
                statValueText.text = playerStats.GetMaxHealthValue().ToString();

            if(statType == tipeStat.damage)
                statValueText.text = (playerStats.damage.GetValue() + playerStats.ketangkasan.GetValue()).ToString();


            if(statType == tipeStat.critPower)
                statValueText.text = (playerStats.critPower.GetValue() +playerStats.ketangkasan.GetValue()).ToString();

            if(statType == tipeStat.critChance)
                statValueText.text = (playerStats.critChance.GetValue() + playerStats.kelincahan.GetValue()).ToString();

            if(statType == tipeStat.penghindar)
                statValueText.text = (playerStats.penghindar.GetValue() + playerStats.kelincahan.GetValue()).ToString();

            if(statType == tipeStat.magicResistance)
                statValueText.text = (playerStats.magicResistance.GetValue() + (playerStats.kecerdasan.GetValue() * 3)).ToString();

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
