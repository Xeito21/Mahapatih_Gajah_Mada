
using System.Text;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public enum TipeItem
{
    Material,
    Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public TipeItem itemType;
    public string namaItem;
    public Sprite icon;
    public string itemID;
    [Range(0,100)]
    public float dropChance;

    protected StringBuilder sb = new StringBuilder();


    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual string GetDescription()
    {
        return "";
    }
}

