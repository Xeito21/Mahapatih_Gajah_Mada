using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Objek item - " + itemData.namaItem;
    }


    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {
        itemData = _itemData;
        rb.velocity = _velocity;

        SetupVisuals();
    }

    public void AmbilItem()
    {
        if(!Bagspace.instance.CanAddItem() && itemData.itemType == TipeItem.Equipment)
        {
            rb.velocity = new Vector2(0,7);
            PlayerManager.instance.player.fx.CreatePopUpText("Bagspace Penuh!");
            return;
        }
        AudioManager.instance.PlaySFX(2,null);
        Bagspace.instance.MenambahkanItem(itemData);
        Destroy(gameObject);
    }
}
