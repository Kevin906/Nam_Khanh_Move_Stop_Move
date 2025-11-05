using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
	[SerializeField] private float patrolRadius = 10f;
	private Vector3 startingPosition;
	private Vector3 destination;
	public NavMeshAgent agent;

	public bool IsAtDestination =>
		agent.isOnNavMesh &&
		!agent.pathPending &&
		agent.remainingDistance <= agent.stoppingDistance;

	public override void OnInit()
	{
		base.OnInit();

		agent.enabled = true;
		startingPosition = TF.position;

		StartCoroutine(InitAfterBakeReady());
	}

	private IEnumerator InitAfterBakeReady()
	{
		yield return new WaitForSeconds(0.2f);

		if (agent.isOnNavMesh)
		{
			SetRandomDestination();
			ChangeAnim("run");
		}
		else
		{
			Debug.LogWarning($"{name} not on NavMesh yet. Will retry...");
			StartCoroutine(RetrySetDestination());
		}
	}

	private IEnumerator RetrySetDestination()
	{
		yield return new WaitForSeconds(1f);
		if (agent.isOnNavMesh)
		{
			SetRandomDestination();
			ChangeAnim("run");
		}
	}

	private void Update()
	{
		if (!agent.enabled || !agent.isOnNavMesh) return;

		if (IsAtDestination)
		{
			SetRandomDestination();
		}

		ChangeAnim(agent.velocity.magnitude > 0.1f ? "run" : "idle");
	}

	private void SetRandomDestination()
	{
		if (!agent.isOnNavMesh)
		{
			Debug.LogWarning($"{name} tried to move but is not on NavMesh.");
			return;
		}

		Vector3 randomDirection = Random.insideUnitSphere * patrolRadius + startingPosition;
		randomDirection.y = 0;

		if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
		{
			destination = hit.position;
			agent.SetDestination(destination);
		}
	}

	public override void OnDespawn()
	{
		agent.enabled = false;
	}
}
