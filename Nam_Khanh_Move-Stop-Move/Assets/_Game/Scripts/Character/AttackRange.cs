using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttackRange : MonoBehaviour
{
    protected List<IDamageAble> damageAbleList = new List<IDamageAble>();
    public int Damage = 10;
    public float AttackDelay = 0.5f;
    public delegate void AttackEventHandler(IDamageAble target);
    public AttackEventHandler OnAttack;
    protected Coroutine attackCoroutine;

    protected void Awake()
    {
        Collider collider = GetComponent<Collider>();
    }

    protected void OnTriggerEnter(Collider other)
    {
        IDamageAble damageAble = other.GetComponent<IDamageAble>();
        if (damageAble != null)
        {
            damageAbleList.Add(damageAble);
            if(attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Attack());
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        IDamageAble damageAble = other.GetComponent<IDamageAble>();
        if (damageAble != null)
        {
            damageAbleList.Remove(damageAble);
            if(damageAbleList.Count == 0 && attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    protected IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(AttackDelay);
        yield return wait;

        IDamageAble closetTarget = null;
        float closetDistance = float.MaxValue;

        while(damageAbleList.Count > 0)
        {
            for (int i=0; i<damageAbleList.Count; i++)
            {
                Transform damageableTranform = damageAbleList[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTranform.position);

                if(distance < closetDistance)
                {
                    closetDistance = distance;
                    closetTarget = damageAbleList[i];
                }
            }
            if(closetTarget != null)
            {
                closetTarget.TakeDamage(Damage);
                OnAttack?.Invoke(closetTarget);
            }

            closetTarget = null;
            closetDistance = float.MaxValue;

            yield return wait;

            damageAbleList.RemoveAll(DisableDamageables);
        }

        attackCoroutine = null;
    }

    protected bool DisableDamageables(IDamageAble damageAble)
    {
        return damageAble != null && !damageAble.GetTransform().gameObject.activeSelf;
    }
}
