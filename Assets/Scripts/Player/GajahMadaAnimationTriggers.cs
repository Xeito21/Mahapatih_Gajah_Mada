using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GajahMadaAnimationTriggers : MonoBehaviour
{
    private GajahMada player => GetComponentInParent<GajahMada>();
    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        AudioManager.instance.PlaySFX(5, null);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                EnemyStats _target = hit.GetComponent<EnemyStats>();
                if(_target != null)
                    player.stats.DoDamage(_target);
                
                ItemData_Equipment weaponData = Bagspace.instance.GetEquipment(TipeEquiment.Senjata);
                if(weaponData != null)
                    weaponData.CallItemEffect(_target.transform);
            }

        }
    }

    private void ThrowSword()
    {
        AudioManager.instance.PlaySFX(12, null);
        SkillManager.instance.sword.CreateSword();
    }
}
