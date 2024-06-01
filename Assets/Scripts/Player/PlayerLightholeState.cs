using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightholeState : PlayerState
{
    private float flyTime = .2f;
    private bool skillUsed;
    private float defaultGravity;
    public PlayerLightholeState(GajahMada _gajahMada, PlayerStateMachine _stateMachine , string _animBoolName) : base(_gajahMada, _stateMachine,  _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = player.rb.gravityScale;
        skillUsed = false;
        stateTimer = flyTime;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer > 0)
        rb.velocity = new Vector2(0,15);

        if(stateTimer <0)
        {
            rb.velocity = new Vector2(0, -.1f);

            if(!skillUsed)
            {
                if(player.skill.lighthole.CanUseSkillI())
                    skillUsed = true;
            }
        }


        if(player.skill.lighthole.SkillComplete())
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit()
    {
        base.Exit();
        player.rb.gravityScale = defaultGravity;
        player.fx.MakeTransparent(false);
    }
}
