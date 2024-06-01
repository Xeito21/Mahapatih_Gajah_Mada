using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(GajahMada _gajahMada, PlayerStateMachine _stateMachine, string _animBoolName) : base(_gajahMada, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        GameObject.Find("Canvas").GetComponent<User_Interfaces>().SwitchOnEndScreen();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

}
