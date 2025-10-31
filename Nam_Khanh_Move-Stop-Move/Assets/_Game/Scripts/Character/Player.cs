using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform model;

    private bool isMoving = false;
    private Coroutine attackDelayRoutine;
    void Update()
    {
        Vector3 direction = JoystickControl.direct;
        isMoving = direction.sqrMagnitude > 0.01f;

        if (isMoving)
        {
            if (attackDelayRoutine != null)
            {
                StopCoroutine(attackDelayRoutine);
                attackDelayRoutine = null;
            }

            Vector3 nextPoint = TF.position + direction * speed * Time.deltaTime;
            TF.position = Move(nextPoint);
            model.forward = direction;
            ChangeAnim("run");
        }
        else
        {
            if (attackDelayRoutine == null)
                attackDelayRoutine = StartCoroutine(CheckAttackAfterDelay(0.2f));
        }
    }

    IEnumerator CheckAttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (HasTargetInRange())
            ChangeAnim("attack");
        else
            ChangeAnim("idle");

        attackDelayRoutine = null;
    }
}
