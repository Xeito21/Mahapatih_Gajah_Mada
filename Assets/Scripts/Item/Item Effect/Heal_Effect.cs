using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0f,1f)]
    [SerializeField] private float healPercentage;
    public override void EksekusiEffect(Transform _enemyPosition)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        int healAmount = Mathf.RoundToInt(playerStats.GetMaxHealthValue() * healPercentage);
        playerStats.IncreaseHealthBy(healAmount);
        
    }
}
