using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
    private Transform sword;
    public PlayerCatchSwordState(GajahMada _gajahMada, PlayerStateMachine _stateMachine, string _animBoolName) : base(_gajahMada, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;
        player.fx.PlayDustFX();
        player.fx.ScreenShake(player.fx.shakeSwordImpact);

        if (player.transform.position.x > sword.position.x && player.facingDir == 1)
            player.Flip();
        else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
            player.Flip();

        rb.velocity = new Vector2(player.impactPedangKembali * -player.facingDir, rb.velocity.y);
        AudioManager.instance.PlaySFX(27,null);
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .5f);
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
