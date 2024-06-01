using System.Collections.Generic;
using UnityEngine;

public enum TipeEquiment
{
    Senjata,
    Pelindung,
    Kalung,
    Ramuan
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public TipeEquiment tipeEquiment;

    [Header("Unik efek")]
    public float itemCooldown;
    public ItemEffect[] itemEfek;
    [Header("Stats")]
    public int ketangkasan;
    public int kelincahan;
    public int kecerdasan;
    public int energi;

    [Header("Offensive Stats")]
    public int damage;
    public int critChance;
    public int critPower;

    [Header("Defense Stats")]
    public int maxHealth;
    public int pelindung;
    public int penghindar;
    public int magicResistance;

    [Header("Magic Stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    [Header("Craft Requirements")]
    public List<BagspaceItem> craftingMaterials;


    private int descriptionLength;


    public void PenambahanStats()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.ketangkasan.AddModifier(ketangkasan);
        playerStats.kelincahan.AddModifier(kelincahan);
        playerStats.kecerdasan.AddModifier(kecerdasan);
        playerStats.energi.AddModifier(energi);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.pelindung.AddModifier(pelindung);
        playerStats.penghindar.AddModifier(penghindar);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
        
    }

    public void PenghapusanStats()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.ketangkasan.RemoveModifier(ketangkasan);
        playerStats.kelincahan.RemoveModifier(kelincahan);
        playerStats.kecerdasan.RemoveModifier(kecerdasan);
        playerStats.energi.RemoveModifier(energi);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.pelindung.RemoveModifier(pelindung);
        playerStats.penghindar.RemoveModifier(penghindar);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public void CallItemEffect(Transform _enemyPosition)
    {
        foreach(var item in itemEfek)
        {
            item.EksekusiEffect(_enemyPosition);
        }
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        descriptionLength = 0;
        AddItemDesc(ketangkasan, "Ketangkasan");
        AddItemDesc(kelincahan, "Kelincahan");
        AddItemDesc(kecerdasan, "Kecerdasan");
        AddItemDesc(energi, "energi");

        AddItemDesc(damage, "Damage");
        AddItemDesc(critChance, "Crit.Chance");
        AddItemDesc(critPower, "Crit.Power");

        AddItemDesc(maxHealth, "Health");
        AddItemDesc(pelindung, "Pelindung");
        AddItemDesc(penghindar, "Penghindar");
        AddItemDesc(magicResistance, "Magic Res");

        AddItemDesc(fireDamage, "Fire Damage");
        AddItemDesc(iceDamage, "Ice Damage");
        AddItemDesc(lightingDamage, "Lightning Damage");

        for(int i = 0; i <itemEfek.Length; i++)
        {
            if(itemEfek[i].effectDesc.Length > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Unique: " + itemEfek[i].effectDesc);
                descriptionLength++;
            }
        }
    

        if(descriptionLength < 5)
        {
            for(int i = 0; i < 5 - descriptionLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        

        return sb.ToString();
    }

    private void AddItemDesc(int _value, string _name)
    {
        if(_value != 0)
        {
            if(sb.Length > 0)
                sb.AppendLine();

            if(_value > 0)
                sb.Append("+ " + _value + " " + _name);

            descriptionLength++;
        }
    }


}


