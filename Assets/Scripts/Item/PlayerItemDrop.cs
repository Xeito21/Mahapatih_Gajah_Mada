using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player's Drop")]
    [SerializeField] private float chanceKehilanganItems;
    [SerializeField] private float chanceKehilanganMaterials;

    public override void GenerateDrop()
    {
        Bagspace bagspace = Bagspace.instance;


        List<BagspaceItem> itemsToUnequip = new List<BagspaceItem>();
        List<BagspaceItem> kehilanganMaterials = new List<BagspaceItem>();

        foreach(BagspaceItem item in bagspace.getEquipmentList())
        {
            if(Random.Range(0, 100) <= chanceKehilanganItems)
            {
                DropItem(item.data);
                itemsToUnequip.Add(item);
            }
        }
        for (int i = 0; i < itemsToUnequip.Count; i++ )
        {
            bagspace.UnequipItem(itemsToUnequip[i].data as ItemData_Equipment);
        }

        foreach(BagspaceItem item in bagspace.GetStashList())
        {
            if(Random.Range(0, 100) <= chanceKehilanganItems)
            {
                DropItem(item.data);
                kehilanganMaterials.Add(item);
            }
        }

        for (int i = 0; i < kehilanganMaterials.Count; i++)
        {
            bagspace.MenghapusItem(kehilanganMaterials[i].data);
        }
                
    }
}
