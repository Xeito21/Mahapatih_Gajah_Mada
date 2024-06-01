using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Bagspace : MonoBehaviour, ISaveManager
{
    public static Bagspace instance;
    public List<ItemData> startingItems;
    public List<BagspaceItem> equipment;
    public Dictionary<ItemData_Equipment, BagspaceItem> equipmentDictionary;
    public List<BagspaceItem> bagspace;
    public Dictionary<ItemData, BagspaceItem> bagspaceDictionary;
    public List<BagspaceItem> stash;
    public Dictionary<ItemData, BagspaceItem> stashDictionary;
    [Header("Bagspace UI")]
    [SerializeField] private Transform bagspaceSlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_SlotItem[] BagspaceItemSlot;
    private UI_SlotItem[] stashItemSlot;
    private UI_SlotEquipment[] slotEquipment;
    private UI_StatSlot[] statSlot;

    [Header("Item Cooldown")]
    private float lastTimeUsedRamuan;
    private float lastTimeUsedPelindung;
    public float ramuanCooldown { get; private set; }
    private float pelindungCooldown;

    [Header("Database Items")]
    public List<ItemData> itemDatabase;
    public List<BagspaceItem> loadedItems;
    public List<ItemData_Equipment> loadedEquipment;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        bagspace = new List<BagspaceItem>();
        bagspaceDictionary = new Dictionary<ItemData, BagspaceItem>();

        stash = new List<BagspaceItem>();
        stashDictionary = new Dictionary<ItemData, BagspaceItem>();

        equipment = new List<BagspaceItem>();
        equipmentDictionary = new Dictionary<ItemData_Equipment, BagspaceItem>();

        BagspaceItemSlot = bagspaceSlotParent.GetComponentsInChildren<UI_SlotItem>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_SlotItem>();
        slotEquipment = equipmentSlotParent.GetComponentsInChildren<UI_SlotEquipment>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();
    }

    private void AddStartingItems()
    {

        foreach (ItemData_Equipment item in loadedEquipment)
        {
            EquipItem(item);
        }

        if (loadedItems.Count > 0)
        {
            foreach (BagspaceItem item in loadedItems)
            {
                for (int i = 0; i < item.jumlahStack; i++)
                {
                    MenambahkanItem(item.data);
                }
            }

            return;
        }



        for (int i = 0; i < startingItems.Count; i++)
        {
            if (startingItems[i] != null)
                MenambahkanItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equipment newEquipment = _item as ItemData_Equipment;
        BagspaceItem newItem = new BagspaceItem(newEquipment);
        ItemData_Equipment oldEquipment = null;

        foreach (KeyValuePair<ItemData_Equipment, BagspaceItem> item in equipmentDictionary)
        {
            if (item.Key.tipeEquiment == newEquipment.tipeEquiment)
                oldEquipment = item.Key;
        }
        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            MenambahkanItem(oldEquipment);
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.PenambahanStats();
        MenghapusItem(_item);
        UpdateSlotUI();
    }

    public void UnequipItem(ItemData_Equipment ItemRemoved)
    {
        if (equipmentDictionary.TryGetValue(ItemRemoved, out BagspaceItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(ItemRemoved);
            ItemRemoved.PenghapusanStats();
        }
    }

    private void UpdateSlotUI()
    {
        for (int i = 0; i < slotEquipment.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equipment, BagspaceItem> item in equipmentDictionary)
            {
                if (item.Key.tipeEquiment == slotEquipment[i].slotType)
                    slotEquipment[i].UpdateSlot(item.Value);
            }
        }

        for (int i = 0; i < BagspaceItemSlot.Length; i++)
        {
            BagspaceItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }


        for (int i = 0; i < bagspace.Count; i++)
        {
            BagspaceItemSlot[i].UpdateSlot(bagspace[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    public void MenambahkanItem(ItemData _item)
    {
        if (_item.itemType == TipeItem.Equipment && CanAddItem())
            PenambahanItemBagspace(_item);
        else if (_item.itemType == TipeItem.Material)
            PenambahanItemStash(_item);

        UpdateSlotUI();
    }

    private void PenambahanItemStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out BagspaceItem value))
        {
            value.TambahStack();
        }
        else
        {
            BagspaceItem newItem = new BagspaceItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void PenambahanItemBagspace(ItemData _item)
    {
        if (bagspaceDictionary.TryGetValue(_item, out BagspaceItem value))
        {
            value.TambahStack();
        }
        else
        {
            BagspaceItem newItem = new BagspaceItem(_item);
            bagspace.Add(newItem);
            bagspaceDictionary.Add(_item, newItem);
        }
    }

    public void MenghapusItem(ItemData _item)
    {
        if (bagspaceDictionary.TryGetValue(_item, out BagspaceItem value))
        {
            if (value.jumlahStack <= 1)
            {
                bagspace.Remove(value);
                bagspaceDictionary.Remove(_item);
            }
            else
            {
                value.KurangStack();
            }
        }

        if (stashDictionary.TryGetValue(_item, out BagspaceItem stashValue))
        {
            if (stashValue.jumlahStack <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.KurangStack();
            }
        }
        UpdateSlotUI();
    }


    public bool CanAddItem()
    {
        if (bagspace.Count >= BagspaceItemSlot.Length)
        {

            return false;
        }
        return true;
    }

    public bool CanCraft(ItemData_Equipment _itemToCraft, List<BagspaceItem> _requiredMaterials)
    {
        //mengecek jika player mempunyai bahan
        foreach (var requiredItem in _requiredMaterials)
        {
            if (stashDictionary.TryGetValue(requiredItem.data, out BagspaceItem stashItem))
            {
                if (stashItem.jumlahStack < requiredItem.jumlahStack)
                {
                    User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Bahan tidak cukup!"));
                    AudioManager.instance.PlaySFX(35,null);
                    return false;
                }
            }
            else
            {
                User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Bahan tidak ada di tas!"));
                AudioManager.instance.PlaySFX(35,null);
                return false;
            }
        }

        //jika mempunyai bahan maka bahan akan berkurang dengan jumlah yang ditentukan

        foreach (var requiredMaterial in _requiredMaterials)
        {
            for (int i = 0; i < requiredMaterial.jumlahStack; i++)
            {
                MenghapusItem(requiredMaterial.data);
            }
        }
        MenambahkanItem(_itemToCraft);
        User_Interfaces.instance.StartCoroutine(User_Interfaces.instance.DisplayPopupText("Craft telah berhasil!"));
        AudioManager.instance.PlaySFX(37,null);
        return true;
    }

    public List<BagspaceItem> getEquipmentList() => equipment;

    public List<BagspaceItem> GetStashList() => stash;

    public ItemData_Equipment GetEquipment(TipeEquiment _type)
    {
        ItemData_Equipment equipedItemData = null;

        foreach (KeyValuePair<ItemData_Equipment, BagspaceItem> item in equipmentDictionary)
        {
            if (item.Key.tipeEquiment == _type)
                equipedItemData = item.Key;
        }


        return equipedItemData;
    }


    public void MenggunakanRamuan()
    {
        ItemData_Equipment currentRamuan = GetEquipment(TipeEquiment.Ramuan);
        if (currentRamuan == null)
            return;

        bool canUseRamuan = Time.time > lastTimeUsedRamuan + currentRamuan.itemCooldown;

        if (canUseRamuan)
        {
            ramuanCooldown = currentRamuan.itemCooldown;
            currentRamuan.CallItemEffect(null);
            lastTimeUsedRamuan = Time.time;
        }
        else
        {
            Debug.Log("Masih Cooldown");
        }
    }

    public bool CanUsePelindung()
    {
        ItemData_Equipment currentArmor = GetEquipment(TipeEquiment.Pelindung);
        if (Time.time > lastTimeUsedPelindung + pelindungCooldown)
        {
            pelindungCooldown = currentArmor.itemCooldown;
            lastTimeUsedPelindung = Time.time;
            return true;
        }

        Debug.Log("Freeze effect on cooldown");

        return false;
    }

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, int> pair in _data.bagspace)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && item.itemID == pair.Key)
                {
                    loadedItems.Add(new BagspaceItem(item) { jumlahStack = pair.Value });
                }
            }
        }

        foreach (string itemID in _data.equipmentId)
        {
            foreach (var item in itemDatabase)
            {
                if (item != null && itemID == item.itemID)
                {
                    loadedEquipment.Add(item as ItemData_Equipment);
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.bagspace.Clear();
        _data.equipmentId.Clear();

        foreach (KeyValuePair<ItemData, BagspaceItem> pair in bagspaceDictionary)
        {
            _data.bagspace.Add(pair.Key.itemID, pair.Value.jumlahStack);
        }

        foreach (KeyValuePair<ItemData, BagspaceItem> pair in stashDictionary)
        {
            _data.bagspace.Add(pair.Key.itemID, pair.Value.jumlahStack);
        }

        foreach (KeyValuePair<ItemData_Equipment, BagspaceItem> pair in equipmentDictionary)
        {
            _data.equipmentId.Add(pair.Key.itemID);
        }
    }




#if UNITY_EDITOR
    [ContextMenu("Fill Up item database")]
    private void FillUpItemDatabase() => itemDatabase = new List<ItemData>(GetItemDataBase());
    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Scripts/Data/Items" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
#endif
}
