using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Shock Strike Effect", menuName = "Data/Item Effect/Shock Strike")]
public class ShockStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject shockStrikePrefab;

    public override void EksekusiEffect(Transform _enemyPosition)
    {
        GameObject newShockStrike = Instantiate(shockStrikePrefab, _enemyPosition.position, Quaternion.identity);
        Destroy(newShockStrike, 1);
    }
}
