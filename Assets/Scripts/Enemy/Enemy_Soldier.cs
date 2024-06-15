using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Soldier : Enemy
{
      #region States
    public SoldierIdleState idleState { get; private set; }
    public SoldierMoveState moveState { get; private set; }
    public SoldierBattleState battleState { get; private set; }
    public SoldierAttackState attackState { get; private set; }

    public SoldierStunState stunState { get; private set; }
    public SoldierDeadState deadState {get; private set;}
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new SoldierIdleState(this, stateMachine, "Idle", this);
        moveState = new SoldierMoveState(this, stateMachine, "Move", this);
        battleState = new SoldierBattleState(this, stateMachine, "Battle", this);
        attackState = new SoldierAttackState(this, stateMachine, "Attack", this);
        stunState = new SoldierStunState(this, stateMachine, "Stunned", this);
        deadState = new SoldierDeadState(this,stateMachine,"Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
        
    }


}
