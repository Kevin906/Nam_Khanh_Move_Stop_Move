using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttack : IState
{
    private Player player;

    public StatePlayerAttack(Player p)
    {
        player = p;
    }

    public void OnEnter()
    {
        if (player.currentTarget == null)
        {
            player.stateMachine.ChangeState(player.idleState);
            return;
        }

        player.LookAt(player.currentTarget);
        player.ChangeAnim("attack");
        player.StartCoroutine(OnEndAttack());
    }

    private IEnumerator OnEndAttack()
    {
        yield return new WaitForSeconds(player.attackDuration);
        if (player.currentTarget != null)
        {
            player.stateMachine.ChangeState(player.attackState);
        }
        else
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public void OnExecute() { }
    public void OnExit() { }
}