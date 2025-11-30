using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyAttack : IStateEnemy
{
    public void OnEnter(Enemy enemy)
    {
        enemy.OnResetAllTrigger();
        enemy.attack();
        enemy.EnemyStopMoving();
        enemy.RestartTimeCounting();
    }
    public void OnExecute(Enemy enemy)
    {
        enemy.CheckIfAttackIsDone();
    }
    public void OnExit(Enemy enemy)
    {
        enemy.OnResetAllTrigger();
    }
}
