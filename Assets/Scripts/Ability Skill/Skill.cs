using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float coolDown;
    public float cooldownTimer;

    protected GajahMada player;


    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
        CheckUnlock();
    }
    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkillI()
    {
        if(cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = coolDown;
            return true;
        }

        player.fx.CreatePopUpText("Cooldown");
        return false;
    }

    protected virtual void CheckUnlock()
    {

    }

    public virtual void UseSkill()
    {
        //spesifik skill
    }

    protected virtual Transform FindMusuhTerdekat(Transform _checkTransform)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach(var hit in colliders)
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
                    
            }
        }
        return closestEnemy;
    }
}
