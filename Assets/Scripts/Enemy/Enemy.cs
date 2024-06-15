using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(EntityFX))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity, ISaveManager
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Stun Info")]
    public float stunDuration = 1;
    public Vector2 stunDirection = new Vector2(8,8);
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    public int soldierID;
    public bool prajurtisDead = false;

    [Header("Move Info")]
    public float moveSpeed = 1.8f;
    public float idleTime = 2;
    public float battleTime = 7;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance = 1.12f;
    public float attackCooldown;
    public float minAttackCooldown = 0.35f;
    public float maxAttackCooldown = 0.55f;

    [HideInInspector] public float lastTimeAttack;
    public EnemyStateMachine stateMachine { get; private set; }
    public EntityFX fx { get; private set; }
    public string lastAnimBoolName {get; private set;}



    protected override void Awake()
    {
        base.Awake();
        stateMachine = gameObject.AddComponent<EnemyStateMachine>();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFX>();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }


    public virtual void FreezeTime(bool _timeFroze)
    {
        {
            if (_timeFroze)
            {
                moveSpeed = 0;
                anim.speed = 0;
            }
            else
            {
                moveSpeed = defaultMoveSpeed;
                anim.speed = 1;
            }
        }
    }

    public virtual void DurasiBekuSelama(float _duration) => StartCoroutine(FreezeTimeFor(_duration));

    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public virtual void AnimationSpecialAttackTrigger()
    {
        
    }

  
    public virtual RaycastHit2D isPlayerDetected()
    {
        float playerDistanceCheck = 50;
        RaycastHit2D playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, playerDistanceCheck, whatIsPlayer);
        RaycastHit2D wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,playerDistanceCheck + 1, WhatisGround);

        if(wallDetected)
        {
            if(wallDetected.distance < playerDetected.distance)
                return default(RaycastHit2D);
        }

        return playerDetected;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

    public void LoadData(GameData _data)
    {
        if (_data.prajurit.TryGetValue(soldierID, out bool value))
        {
            prajurtisDead = value;
            if (prajurtisDead)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (_data.prajurit.TryGetValue(soldierID, out bool value))
        {
            _data.prajurit.Remove(soldierID);
            _data.prajurit.Add(soldierID, prajurtisDead);
        }
        else
            _data.prajurit.Add(soldierID, prajurtisDead);
    }
}
