using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SlotEquipment : UI_SlotItem
{
    public TipeEquiment slotType;


    private void OnValidate()
    {
        gameObject.name = "Equipment slot -" + slotType.ToString();
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        if(item == null || item.data == null)
            return;
        Bagspace.instance.UnequipItem(item.data as ItemData_Equipment);
        AudioManager.instance.PlaySFX(22,null);
        Bagspace.instance.MenambahkanItem(item.data as ItemData_Equipment);

        ui.itemToolTips.HideToolTip();
        CleanUpSlot();
    }
}
