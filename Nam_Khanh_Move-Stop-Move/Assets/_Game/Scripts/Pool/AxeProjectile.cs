using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AxeProjectile : MonoBehaviour
{
    public float AutoDespawnTime = 5f;
    private float MoveSpeed = 2f;
    public int Damage = 5;
    public Rigidbody Rb;

    private string DISABLE_METHOD_NAME = "Disable";

    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        Invoke(DISABLE_METHOD_NAME, AutoDespawnTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageAble>(out IDamageAble damageAble))
        {
            damageAble.TakeDamage(Damage);
        }
        Disable();
    }

    private void Disable()
    {
        CancelInvoke(DISABLE_METHOD_NAME);
        Rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }
}
