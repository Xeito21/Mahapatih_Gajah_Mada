using System.Collections;
using System.Collections.Generic;


public class PlayerMoveState : PlayerGroundState
{

    public PlayerMoveState(GajahMada _gajahMada, PlayerStateMachine _stateMachine, string _animBoolName) : base(_gajahMada, _stateMachine, _animBoolName)
    {
    }



    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(0,null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(0);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed,rb.velocity.y);

        if (xInput == 0 || player.isWallDetected())
            stateMachine.ChangeState(player.idleState);
    }


}
