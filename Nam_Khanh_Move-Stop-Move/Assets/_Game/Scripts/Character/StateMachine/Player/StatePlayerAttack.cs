using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerAttack : IStatePlayer
{
    public void OnEnter(Player player)
    {
        player.OnAttack();
    }
    public void OnExecute(Player player)
    {
        player.attack();
        player.CheckIdleToPatrol();
    }
    public void OnExit(Player player)
    {
        player.OnResetAllTrigger();
    }
}
