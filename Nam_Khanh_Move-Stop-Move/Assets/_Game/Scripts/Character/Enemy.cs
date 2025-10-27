using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
	[SerializeField] private float patrolRadius = 10f;

    private Vector3 startingPosition;
	private Vector3 destination;
	public NavMeshAgent agent;
	public bool IsAtDestination => !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;

	public override void OnInit()
    {
        base.OnInit();
		agent.enabled = true;
		startingPosition = TF.position;
		SetRandomDestination();
		ChangeAnim("run");
    }

	private void Update()
	{
        if (IsAtDestination)
        {
			SetRandomDestination();
        }
		if(agent.velocity.magnitude > 0.1f)
		{
			ChangeAnim("run");
		}
		else
		{
			ChangeAnim("idle");
		}
    }

	private void SetRandomDestination()
	{
		Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
		randomDirection += startingPosition;
		randomDirection.y = 0;

		NavMeshHit hit;
		if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
		{
			destination = hit.position;
			agent.SetDestination(destination);
		}
		else
		{
			SetRandomDestination();
		}
	}
	public void SetDestination(Vector3 position)
	{
		agent.enabled = true;
		destination = position;
		destination.y = 0;
		agent.SetDestination(position);
	}
	public override void OnDespawn()
    {
        throw new System.NotImplementedException();
    }
}
