using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SlotCraft : UI_SlotItem
{


    protected override void Start()
    {
        base.Start();
    }


    public void SetupCraftSlot(ItemData_Equipment _data)
    {
        if(_data == null)
            return;
        item.data = _data;

        gambarItem.sprite = _data.icon;
        teksItem.text = _data.namaItem;

        if(teksItem.text.Length > 13)
            teksItem.fontSize = teksItem.fontSize * .7f;
        else
        teksItem.fontSize = 26;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(item.data as ItemData_Equipment);
    }
}
