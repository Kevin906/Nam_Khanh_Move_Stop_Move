using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private float attackRange;

    private Character owner;
    private SphereCollider col;

    void Awake()
    {
        owner = GetComponentInParent<Character>();
        col = GetComponent<SphereCollider>();
        col.isTrigger = true;
        col.radius = attackRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.name);
        Character target = other.GetComponent<Character>();
        if (target != null && target != owner)
        {
            owner.AddTarget(target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit: " + other.name);
        Character target = other.GetComponent<Character>();
        if (target != null && target != owner)
        {
            owner.RemoveTarget(target);
        }
    }
}
