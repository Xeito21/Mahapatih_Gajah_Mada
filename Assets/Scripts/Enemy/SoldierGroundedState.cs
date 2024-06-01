using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierGroundedState : EnemyState
{
    protected Enemy_Soldier enemy;
    protected Transform player;
    public SoldierGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Soldier enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemy.isPlayerDetected() || Vector2.Distance(enemy.transform.position, player.position) < 2)
            stateMachine.ChangeState(enemy.battleState);
    }
}
