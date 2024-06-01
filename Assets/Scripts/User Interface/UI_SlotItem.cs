using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SlotItem : MonoBehaviour , IPointerDownHandler, IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] protected Image gambarItem;
    [SerializeField] protected TextMeshProUGUI teksItem;

    public BagspaceItem item;
    protected User_Interfaces ui;

    protected virtual void Start()
    {
        ui = GetComponentInParent<User_Interfaces>();
    }

    public void UpdateSlot(BagspaceItem _newItem)
    {
        item = _newItem;

        gambarItem.color = Color.white;
        if (item != null)
        {
            gambarItem.sprite = item.data.icon;

            if (item.jumlahStack > 1)
            {
                teksItem.text = item.jumlahStack.ToString();
            }
            else
            {
                teksItem.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;
        gambarItem.sprite = null;
        gambarItem.color = Color.clear;

        teksItem.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if(item == null)
            return;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            Bagspace.instance.MenghapusItem(item.data);
            AudioManager.instance.PlaySFX(23,null);
            return;
        }
        if(item.data.itemType == TipeItem.Equipment)
        {
            AudioManager.instance.PlaySFX(21,null);
            Bagspace.instance.EquipItem(item.data);
        }

        ui.itemToolTips.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(item == null)
            return;

        /*Vector2 mousePosition = Input.mousePosition;

        float xOffset = 0;
        float yOffset = 0;

        if(mousePosition.x > 600)
            xOffset = -100;
        else
            xOffset = 100;

        if(mousePosition.y > 320)
            yOffset = -100;
        else
            yOffset = 100;
        */
        

        ui.itemToolTips.ShowToolTip(item.data as ItemData_Equipment);
        //ui.itemToolTips.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item == null)
            return;
        ui.itemToolTips.HideToolTip();
    }
}
