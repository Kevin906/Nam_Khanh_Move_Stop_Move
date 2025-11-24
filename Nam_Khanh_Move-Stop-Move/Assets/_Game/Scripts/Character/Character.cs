using System.Collections;
using UnityEngine;

public class Character : GameUnit
{
    [Header("Stats")]
    public int Health = 100;
    public float attackDuration = 0.6f;
    public bool canAttackWhileMoving = false;

    [Header("Components")]
    public Animator anim;
    public AttackRange attackRange;
    public Transform model;
    public LayerMask groundLayer;

    protected bool isAttacking;
    protected bool isMoving;
    private Coroutine lookCoroutine;
    private string currentAnim;

    protected virtual void Awake()
    {
        if (!attackRange)
            attackRange = GetComponentInChildren<AttackRange>();

        if (attackRange)
            attackRange.OnAttack += HandleAttack;
    }

    public override void OnInit()
    {
        currentAnim = "";
    }

    public override void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    public void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(currentAnim);
            currentAnim = animName;
            anim.SetTrigger(currentAnim);
        }
    }

    private void HandleAttack(IDamageAble target)
    {
        if (isAttacking) return;
        if (isMoving && !canAttackWhileMoving) return;

        isAttacking = true;
        StopMovement();
        LookAt(target.GetTransform());

        ChangeAnim("attack");
        StartCoroutine(EndAttack());
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        ResumeMovement();
    }

    public void LookAt(Transform target)
    {
        if (lookCoroutine != null)
            StopCoroutine(lookCoroutine);

        lookCoroutine = StartCoroutine(LookAtSmooth(target));
    }

    private IEnumerator LookAtSmooth(Transform target)
    {
        Quaternion targetRot = Quaternion.LookRotation(target.position - transform.position);
        float t = 0f;

        while (t < 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, t);
            t += Time.deltaTime * 5;
            yield return null;
        }

        transform.rotation = targetRot;
    }

    public Vector3 CheckGround(Vector3 point)
    {
        if (Physics.Raycast(point + Vector3.up, Vector3.down, out RaycastHit hit, 2f, groundLayer))
            point.y = hit.point.y;

        return point;
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        ChangeAnim("die");
        OnDead();
    }

    protected virtual void OnDead() { }

    protected virtual void StopMovement()
    {
        isMoving = false;
    }

    protected virtual void ResumeMovement()
    {
        // override n?u mu?n
    }

    protected virtual void OnAttack(IDamageAble target) { }
}
