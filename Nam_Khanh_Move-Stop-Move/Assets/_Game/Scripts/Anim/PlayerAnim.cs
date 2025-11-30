using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : CharacterAnim,ISubscribers,IVariable
{
    [SerializeField] private Transform Player;
    private Player players;
    
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
        players.OnAttack += AttackAnimation;
        players.OnDance += DanceAnimation;
        players.OnIdle += IdleAnimation;
        players.OnDeath += DeathAnimation;
        players.OnWin += WinAnimation;
        players.OnRun += RunAnimation;
        players.OnResetAllTrigger += ResetAllTriggerAnim;
    }
    public void UnsubscribeEvent()
    {
        players.OnAttack -= AttackAnimation;
        players.OnDance -= DanceAnimation;
        players.OnIdle -= IdleAnimation;
        players.OnDeath -= DeathAnimation;
        players.OnWin -= WinAnimation;
        players.OnRun -= RunAnimation;
        players.OnResetAllTrigger -= ResetAllTriggerAnim;
    }
    public void IVariables()
    {
        players = Player.GetComponent<Player>();
    }
}
