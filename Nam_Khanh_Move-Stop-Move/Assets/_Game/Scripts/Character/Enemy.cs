using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    [SerializeField] private float patrolRadius = 10f;

    private Vector3 startingPosition;
    private Vector3 destination;
    public NavMeshAgent agent;


    public bool IsAtDestination =>
        agent != null && agent.isOnNavMesh && !agent.pathPending &&
        agent.remainingDistance <= agent.stoppingDistance + 0.1f;

    public override void OnInit()
    {
        base.OnInit();

        if (agent == null) agent = GetComponent<NavMeshAgent>();

        if (agent != null && NavMesh.SamplePosition(TF.position, out NavMeshHit hit, 1f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            agent.enabled = true;
            startingPosition = agent.transform.position;
            SetRandomDestination();
            ChangeAnim("run");
        }
        else
        {
            // Nếu không có NavMesh ở vị trí spawn → disable agent để tránh lỗi
            if (agent != null) agent.enabled = false;
            ChangeAnim("idle");
            Debug.LogWarning($"{name}: not on NavMesh at {TF.position}");
        }
    }


    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh || !agent.isActiveAndEnabled)
        {
            ChangeAnim("idle");
            return;
        }

        if (IsAtDestination)
            SetRandomDestination();

        if (agent.velocity.sqrMagnitude > 0.01f) ChangeAnim("run");
        else ChangeAnim("idle");
    }

    private void SetRandomDestination()
    {
        if (agent == null || !agent.isOnNavMesh) return;

        const int attempts = 6;
        for (int i = 0; i < attempts; i++)
        {
            Vector3 rnd = startingPosition + Random.insideUnitSphere * patrolRadius;
            rnd.y = startingPosition.y;

            if (NavMesh.SamplePosition(rnd, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                destination = hit.position;
                agent.SetDestination(destination);
                return;
            }
        }
    }

    public override void OnDespawn()
    {
        // nếu dùng pooling: SimplePool.Despawn(this);
        gameObject.SetActive(false);
    }
}
