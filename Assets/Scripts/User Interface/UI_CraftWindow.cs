using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDesc;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftBtn;
    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftBtn.onClick.RemoveAllListeners();

        for(int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if(_data.craftingMaterials.Count > materialImage.Length)
                Debug.Log("You have more materials");

            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterials[i].jumlahStack.ToString();
            materialSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.namaItem;
        itemDesc.text = _data.GetDescription();

        craftBtn.onClick.AddListener( () => Bagspace.instance.CanCraft(_data, _data.craftingMaterials));
    }
}
