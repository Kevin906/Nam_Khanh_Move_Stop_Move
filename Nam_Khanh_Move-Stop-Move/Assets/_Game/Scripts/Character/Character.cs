using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Character : GameUnit, IDamageAble
{
    [SerializeField] private LayerMask groundLayer;

    public Animator anim;
    private string currentAnim;

	public override void OnInit()
	{
        currentAnim = "";
	}

    public override void OnDespawn()
    {
        SimplePool.Despawn(this);
    }

    public Vector3 CheckGround(Vector3 nextPoint)
    {
        RaycastHit hit;

		if (Physics.Raycast(nextPoint + Vector3.up * 0.5f, Vector3.down, out hit, 2f, groundLayer))
		{
			nextPoint.y = hit.point.y + 0.05f;
		}

		return nextPoint;
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

    public void TakeDamage(int damage)
    {
        Die();
    }

    private void Die()
    {

        ChangeAnim("die");

        Invoke(nameof(DespawnSelf), 0.5f);
    }

    private void DespawnSelf()
    {
        SimplePool.Despawn(this);
    }
    public Transform GetTransform()
    {
        return transform;
    }
}
