using System;


[Serializable]
public class BagspaceItem
{
    public ItemData data;
    public int jumlahStack;


    public BagspaceItem(ItemData _newItemData)
    {
        data = _newItemData;
        TambahStack();
    }

    public void TambahStack() => jumlahStack++;
    public void KurangStack() => jumlahStack--;
}
