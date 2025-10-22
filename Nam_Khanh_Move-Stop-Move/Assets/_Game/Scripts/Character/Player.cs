using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform model;
    [SerializeField] private float attackCooldown = 1.0f;
    [SerializeField] private float attackHitDelay = 0.25f;

    void Update()
    {
        Vector3 direction = JoystickControl.direct;

        if (direction != Vector3.zero)
        {
            Vector3 nextPoint = TF.position + direction * speed * Time.deltaTime;
            TF.position = CheckGround(nextPoint);
            model.forward = direction;

            ChangeAnim("run");
        }
        else
        {
            ChangeAnim("idle");
        }
    }
}
