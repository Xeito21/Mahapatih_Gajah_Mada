using UnityEngine;

[CreateAssetMenu(fileName = "Freeze Effect", menuName = "Data/Item Effect/Freeze Enemies")]
public class FreezeEnemies_Effect : ItemEffect
{
    [SerializeField] private float duration;

    public override void EksekusiEffect(Transform _transform)
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if(playerStats.currentHealth > playerStats.GetMaxHealthValue() * .1f)
            return;

        if(!Bagspace.instance.CanUsePelindung())
            return;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position,2);

        foreach (var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.DurasiBekuSelama(duration);
        }
    }
}
