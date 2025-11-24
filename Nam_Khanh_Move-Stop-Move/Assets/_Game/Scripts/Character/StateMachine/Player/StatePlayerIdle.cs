using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerIdle : IState
{
    private Player player;

    public StatePlayerIdle(Player p)
    {
        player = p;
    }

    public void OnEnter()
    {
        player.ChangeAnim("idle");
    }

    public void OnExecute()
    {
        if (player.currentTarget != null)
        {
            player.stateMachine.ChangeState(player.attackState);
            return;
        }

        Vector3 dir = JoystickControl.direct;
        if (dir != Vector3.zero)
            player.stateMachine.ChangeState(player.moveState);
    }

    public void OnExit() { }
}
