using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
    private float crystallExistTimer;
    private GajahMada player;
    private Animator anim => GetComponent<Animator>();
    private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private bool canExplode;
    private bool canMove;
    private float moveSpeed;

    private bool canGrow;
    private float growSpeed = 5;
    private Transform closestTarget;
    [SerializeField] private LayerMask whatIsEnemy;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget, GajahMada _player)
    {
        player = _player;
        crystallExistTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
    }

    public void ChooseRandomEnemy()
    {
        float radius = SkillManager.instance.lighthole.GetLightholeRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);
        if(colliders.Length > 0)
            closestTarget = colliders[Random.Range(0,colliders.Length)].transform;
    }

    private void Update()
    {
        crystallExistTimer -= Time.deltaTime;
        if(crystallExistTimer < 0)
        {
            CrystalHasGone();
        }

        if(canMove)
        {
            if(closestTarget == null)
                return;
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, closestTarget.position) < .5f)
            {
                CrystalHasGone();
                canMove = false;
            }
                
        }


        if(canGrow)
        transform.localScale =Vector2.Lerp(transform.localScale, new Vector2(5,5), growSpeed * Time.deltaTime);

    }

    private void AnimationExplodeEvent()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,cd.radius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                 hit.GetComponent<Entity>().SetupKnockBackDir(transform);
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());
                ItemData_Equipment equipedAmulet = Bagspace.instance.GetEquipment(TipeEquiment.Kalung);

                if(equipedAmulet !=null)
                    equipedAmulet.CallItemEffect(hit.transform);
            }
        }
    }

    public void CrystalHasGone()
    {
        if (canExplode)
        {
            canGrow = true;
            anim.SetTrigger("Explode");
        }

        else
            SelfDestroy();
    }

    public void SelfDestroy() => Destroy(gameObject);
}
