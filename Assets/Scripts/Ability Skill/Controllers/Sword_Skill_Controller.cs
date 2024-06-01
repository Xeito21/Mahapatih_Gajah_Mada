using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private GajahMada player;

    private bool canRotate = true;
    private bool isSwordBack;

    private float returnSpeed = 12;
    private float freezeTimeDuration;

    [Header("Tembus Info")]
    private float jumlahTembus;

    [Header("Bounce Info")]
    private float mentalSpeed;
    private bool isMental;
    private int jumlahMental;
    private List<Transform> enemyTarget;
    private int targetIndex;

    [Header("Memutar Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool isSpinStop;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    private float spinDirection;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, GajahMada _player, float _freezeTimeDuration, float _returnSpeed)
    {
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        if (jumlahTembus <= 0)
            anim.SetBool("Rotation", true);

        spinDirection = Mathf.Clamp(rb.velocity.x, -1, 1);
        Invoke("DestroyMe", 7);
    }

    public void SetupMental(bool _isMental, int _jumlahMental, float _mentalSpeed)
    {
        isMental = _isMental;
        jumlahMental = _jumlahMental;
        mentalSpeed = _mentalSpeed;
        enemyTarget = new List<Transform>();
    }

    public void SetupTembus(int _jumlahTembus)
    {
        jumlahTembus = _jumlahTembus;
    }

    public void SetupMemutar(bool _isMemutar, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        isSpinning = _isMemutar;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isSwordBack = true;

    
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isSwordBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        BounceLogic();
        SpinLogic();

    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !isSpinStop)
            {
                StopKetikaMemutar();
            }

            if (isSpinStop)
            {
                spinTimer -= Time.deltaTime;
                //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDirection, transform.position.y), 1.5f * Time.deltaTime);
                if (spinTimer < 0)
                {
                    isSwordBack = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;
                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());

                    }
                }
            }
        }
    }

    private void StopKetikaMemutar()
    {
        isSpinStop = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void BounceLogic()
    {
        if (isMental && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, mentalSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());
                targetIndex++;
                jumlahMental--;
                if (jumlahMental <= 0)
                {
                    isMental = false;
                    isSwordBack = true;
                }
                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
            AudioManager.instance.PlaySFX(16, null);

        if (isSwordBack)
            return;
        if(collision.GetComponent<Enemy> () != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }


        SetupTargetsForBounce(collision);
        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
        AudioManager.instance.PlaySFX(16,null);

        player.stats.DoDamage(enemyStats);
        if(player.skill.sword.temporalValorUnlocked)
            enemy.DurasiBekuSelama(freezeTimeDuration);

        if(player.skill.sword.shatterstrikeHurlUnlocked)
            enemyStats.MakeShatteredFor(freezeTimeDuration);
        
        ItemData_Equipment equipedAmulet = Bagspace.instance.GetEquipment(TipeEquiment.Kalung);

        if(equipedAmulet !=null)
            equipedAmulet.CallItemEffect(enemy.transform);
    }

    private void SetupTargetsForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isMental && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);
                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (jumlahTembus > 0 && collision.GetComponent<Enemy>() != null)
        {
            jumlahTembus--;
            return;
        }

        if (isSpinning)
        {
            StopKetikaMemutar();
            return;
        }


        canRotate = false;
        cd.enabled = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponentInChildren<ParticleSystem>().Play();
        if (isMental && enemyTarget.Count > 0)
            return;
        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
