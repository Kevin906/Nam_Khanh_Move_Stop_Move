using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AttackRange attackRange;

    public Animator anim;
    private string currentAnim;

    public List<Character> targets = new List<Character>();

    public override void OnInit()
	{
        currentAnim = "";
        targets.Clear();
    }
     
    public Vector3 Move(Vector3 nextPoint)
    {
        RaycastHit hit;

		if (Physics.Raycast(nextPoint + Vector3.up * 0.5f, Vector3.down, out hit, 2f, groundLayer))
		{
			nextPoint.y = hit.point.y + 0.05f;
		}

		return nextPoint;
    }

    public override void OnDespawn()
	{
        gameObject.SetActive(false);
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

    public void AddTarget(Character target)
    {
        if (!targets.Contains(target))
            targets.Add(target);
    }

    public void RemoveTarget(Character target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    protected bool HasTargetInRange()
    {
        return targets.Count > 0;
    }

}
