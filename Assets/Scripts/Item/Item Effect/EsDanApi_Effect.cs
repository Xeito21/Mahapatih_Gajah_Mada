using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Ice and Fire Effect", menuName = "Data/Item Effect/Ice And Fire")]
public class EsDanApi_Effect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;

    public override void EksekusiEffect(Transform _respawnPosition)
    {
        GajahMada player = PlayerManager.instance.player;

        bool attackThird = player.primaryAttackState.comboCounter == 2;

        if(attackThird)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, _respawnPosition.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDir, 0);
            Destroy(newIceAndFire, 5);
        }
    }
}
