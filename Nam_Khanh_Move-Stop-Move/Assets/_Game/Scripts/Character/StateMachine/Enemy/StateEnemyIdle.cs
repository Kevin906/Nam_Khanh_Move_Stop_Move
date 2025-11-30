using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyIdle : IStateEnemy
{
    public void OnEnter(Enemy enemy)
    {
        enemy.OnResetAllTrigger();
        enemy.OnIdle();
        enemy.EnemyStopMoving();
        enemy.RestartTimeCounting();
    }
    public void OnExecute(Enemy enemy)
    {
        enemy.CheckIdletoAttack();
        enemy.CheckIdletoPatrol();
    }
    public void OnExit(Enemy enemy)
    {
        enemy.OnResetAllTrigger();
    }
}
