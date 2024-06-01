using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    private bool canCreateClone;
    public PlayerCounterAttackState(GajahMada _gajahMada, PlayerStateMachine _stateMachine, string _animBoolName) : base(_gajahMada, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuksesCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Arrow_Controller>() != null)
            {
                hit.GetComponent<Arrow_Controller>().FlipArrow();
                SuksesCounterAttack();
            }



            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                    {
                        SuksesCounterAttack();

                        player.skill.parry.UseSkill();
                        if (canCreateClone)
                        {
                            canCreateClone = false;
                            player.skill.parry.MirageRiposteOnParry(hit.transform);
                        }

                    }
                }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    private void SuksesCounterAttack()
    {
        stateTimer = 10;
        player.anim.SetBool("SuksesCounterAttack", true);
    }
}
