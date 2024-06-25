using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackStates : PlayerState
{
    public int comboCounter {get; private set;}
    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PlayerPrimaryAttackStates(GajahMada _gajahMada, PlayerStateMachine _stateMachine, string _animBoolName) : base(_gajahMada, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //AudioManager.instance.PlaySFX(5);
        xInput = 0;
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter", comboCounter);

        #region choose Attack Direction
        float attackDir = player.facingDir;
        if (xInput != 0)
            attackDir = xInput;


        #endregion


        //player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .15f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            player.SetZeroVelocity();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
