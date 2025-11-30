using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnim : CharacterAnim, ISubscribers
{
    [SerializeField] private Transform enemy;
    private Enemy enemys;
    private void OnEnable()
    {
        IVariables();
        SubscribeEvent();
    }

    private void OnDisable()
    {
        UnsubscribeEvent();
    }
    public void SubscribeEvent()
    {
        enemys.OnAttack += AttackAnimation;
        enemys.OnRun += RunAnimation;
        enemys.OnIdle += IdleAnimation;
        enemys.OnDeath += DeathAnimation;
        enemys.OnWin += WinAnimation;
        enemys.OnDance += DanceAnimation;
        enemys.OnResetAllTrigger += ResetAllTriggerAnim;
    }

    public void UnsubscribeEvent()
    {
        enemys.OnAttack -= AttackAnimation;
        enemys.OnRun -= RunAnimation;
        enemys.OnIdle -= IdleAnimation;
        enemys.OnDeath -= DeathAnimation;
        enemys.OnWin -= WinAnimation;
        enemys.OnDance -= DanceAnimation;
        enemys.OnResetAllTrigger -= ResetAllTriggerAnim;

    }
    public void IVariables()
    {
        enemys = enemy.GetComponent<Enemy>();
    }
}
