using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerPatrol : IStatePlayer
{
    public void OnEnter(Player player)
    {
        player.OnRun();
    }
    public void OnExecute(Player player)
    {
        player.move();
        player.CheckPatrolToIdle();
    }
    public void OnExit(Player player)
    {
        player.OnResetAllTrigger();
    }
}
