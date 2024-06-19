using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(GajahMada _gajahMada, PlayerStateMachine _stateMachine, string _animBoolName) : base(_gajahMada, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Exit()
    {
        base.Exit();
  
    }

    public override void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
            return;
        base.Update();
        
        if(Input.GetKeyDown(KeyCode.R) && player.skill.lighthole.vortexUnlocked)
        {
            if(player.skill.lighthole.cooldownTimer > 0)
            {
                player.fx.CreatePopUpText("Cooldown");
                return;
            }

            stateMachine.ChangeState(player.lightholeState);
        }

            
        if (Input.GetKeyDown(KeyCode.Q) && TidakMemegangPedang() && player.skill.sword.swordUnlocked)
            stateMachine.ChangeState(player.aimSwordState);

        if (Input.GetMouseButtonDown(1) && player.skill.parry.resoluteParryUnlocked)
            stateMachine.ChangeState(player.counterAttackState);

        if (Input.GetMouseButtonDown(0))
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }

        if (!player.isGroundDetected())
            stateMachine.ChangeState(player.airState);
        if (Input.GetKeyDown(KeyCode.Space) && player.isGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    private bool TidakMemegangPedang()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
