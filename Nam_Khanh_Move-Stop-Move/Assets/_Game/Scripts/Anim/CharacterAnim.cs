using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    [HideInInspector] public enum CharacterAnimState { Attack, Dance, Idle, Death, Run, Win }
    public CharacterAnimState lastState;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        IVariables();
    }

    private void IVariables()
    {
        animator = GetComponent<Animator>();
        lastState = CharacterAnimState.Idle;
    }

    public void SetAnim(CharacterAnimState _CharacterAnimation)
    {
        if (_CharacterAnimation != lastState)
        {
            switch (_CharacterAnimation)
            {
                case CharacterAnimState.Attack:
                    animator.SetTrigger("attack");
                    break;
                case CharacterAnimState.Dance:
                    animator.SetTrigger("dance");
                    break;
                case CharacterAnimState.Idle:
                    animator.SetTrigger("idle");
                    break;
                case CharacterAnimState.Death:
                    animator.SetTrigger("death");
                    break;
                case CharacterAnimState.Run:
                    animator.SetTrigger("run");
                    break;
                case CharacterAnimState.Win:
                    animator.SetTrigger("win");
                    break;
            }
            lastState = _CharacterAnimation;
        }
    }


    public void AttackAnimation()
    {
        SetAnim(CharacterAnimState.Attack);
    }
    public void DanceAnimation()
    {
        SetAnim(CharacterAnimState.Dance);
    }
    public void IdleAnimation()
    {
        SetAnim(CharacterAnimState.Idle);
    }
    public void DeathAnimation()
    {
        SetAnim(CharacterAnimState.Death);
    }
    public void RunAnimation()
    {
        SetAnim(CharacterAnimState.Run);
    }
    public void WinAnimation()
    {
        SetAnim(CharacterAnimState.Win);
    }
    public void ResetAllTriggerAnim()
    {
        animator.ResetTrigger("attack");
        animator.ResetTrigger("dance");
        animator.ResetTrigger("idle");
        animator.ResetTrigger("death");
        animator.ResetTrigger("run");
        animator.ResetTrigger("win");
    }
}
