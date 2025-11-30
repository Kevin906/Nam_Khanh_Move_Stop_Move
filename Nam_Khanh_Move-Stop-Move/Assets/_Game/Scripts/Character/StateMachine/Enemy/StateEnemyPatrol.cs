using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateEnemyPatrol : IStateEnemy
{
    public void OnEnter(Enemy enemy)
    {
        enemy.FindNextDestination();
        enemy.RestartTimeCounting();
    }
    public void OnExecute(Enemy enemy)
    {
        enemy.OnRun();
        enemy.EnemyMovement();
        enemy.CheckPatroltoAttack();
        enemy.CheckArriveDestination();
    }
    public void OnExit(Enemy enemy)
    {
        enemy.EnemyStopMoving();
        enemy.OnResetAllTrigger();
    }
}
