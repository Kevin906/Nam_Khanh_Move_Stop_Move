using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerIdle : IStatePlayer
{
    public void OnEnter(Player player)
    {
        player.OnIdle();
    }
    public void OnExecute(Player player)
    {
        player.CheckIdleToPatrol();
        player.CheckIdletoAttack();
    }
    public void OnExit(Player player)
    {
        player.OnResetAllTrigger();
    }
}
