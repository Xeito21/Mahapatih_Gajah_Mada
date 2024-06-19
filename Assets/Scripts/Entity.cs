using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius = 0.75f;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 1.06f;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 1.57f;
    [SerializeField] protected LayerMask WhatisGround;
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public SpriteRenderer sr { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    [Header("KnockBack Info")]
    [SerializeField] protected Vector2 knockbackDirection = new Vector2(7, 12);
    [SerializeField] protected Vector2 knockbackOffset = new Vector2(.5f, 2);
    [SerializeField] protected float knockBackDuration = .07f;
    protected bool isKnocked;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;
    public int knockBackDir { get; private set; }

    public System.Action onFlipped;
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact() => StartCoroutine("HitKnockBack");

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * knockBackDir, knockbackDirection.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
        SetupZeroKnockbackPower();
    }

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
    #region Collision

    public virtual bool isGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, WhatisGround);
    public virtual bool isWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, WhatisGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    #endregion
    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion



    public virtual void Die()
    {

    }

    public virtual void SetupKnockBackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
            knockBackDir = -1;
        else if (_damageDirection.position.x < transform.position.x)
            knockBackDir = 1;
    }

    protected virtual void SetupZeroKnockbackPower()
    {

    }

    public void SetupKnockBackPower(Vector2 _knockbackPower) => knockbackDirection = _knockbackPower;
}
