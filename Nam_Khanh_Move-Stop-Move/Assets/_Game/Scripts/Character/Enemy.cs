using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}

public class Enemy : Character
{
    public EnemyState state = EnemyState.Idle;

    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float idleTime = 1.5f;

    public NavMeshAgent agent;
    private Vector3 startPos;

    [Header("Chase Settings")]
    public float visionRange = 7f;
    public float stopChaseDistance = 10f;
    private Transform player;
    private Transform target;

    [Header("Attack Settings")]
    public float AttackRange = 1.6f;
    public float attackCooldown = 1f;
    private bool canAttack = true;
    private Coroutine attackCoroutine;
    [SerializeField] private int Damage = 10;

    protected override void Awake()
    {
        base.Awake();
        // Enemy uses its own attack routine, not the AttackRange automatic handler
        if (attackRange) attackRange.enabled = false;
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        startPos = TF != null ? TF.position : transform.position;
        StartCoroutine(WaitForNavMesh());
    }

    private IEnumerator WaitForNavMesh()
    {
        yield return new WaitForSeconds(0.15f);

        if (agent == null || !agent.isOnNavMesh)
        {
            // try enabling agent later or keep idle
            yield break;
        }

        ChangeState(EnemyState.Idle);
    }

    private void Update()
    {
        if (state == EnemyState.Dead) return;
        if (player == null) return;

        DetectPlayer();
        DetectTarget();


        switch (state)
        {
            case EnemyState.Idle: IdleUpdate(); break;
            case EnemyState.Patrol: PatrolUpdate(); break;
            case EnemyState.Chase: ChaseUpdate(); break;
            case EnemyState.Attack: AttackUpdate(); break;
        }
    }

    private void DetectPlayer()
    {
        if (player == null) return;

        float dist = Vector3.Distance(TF.position, player.position);

        // Attack condition
        if (dist <= AttackRange && state != EnemyState.Attack)
        {
            ChangeState(EnemyState.Attack);
            return;
        }
        // Chase condition
        if (dist <= visionRange && dist > AttackRange && state != EnemyState.Chase)
        {
            ChangeState(EnemyState.Chase);
            return;
        }
        // Lose target: if too far stop chasing/attacking and go patrol
        if (dist >= stopChaseDistance && (state == EnemyState.Chase || state == EnemyState.Attack))
        {
            ChangeState(EnemyState.Patrol);
            return;
        }
    }
    private void DetectTarget()
    {
        float closest = Mathf.Infinity;
        Transform best = null;

        // tìm t?t c? Character trên scene
        foreach (var c in FindObjectsOfType<Character>())
        {
            if (c == this) continue;            // b? qua b?n thân
            if (c.Health <= 0) continue;        // b? qua ch?t

            float dist = Vector3.Distance(TF.position, c.TF.position);

            // n?m trong t?m nhìn?
            if (dist <= stopChaseDistance && dist < closest)
            {
                closest = dist;
                best = c.transform;
            }
        }

        target = best;
        if (target == null)
        {
            // không th?y ai ? quay v? patrol
            if (state != EnemyState.Patrol)
                ChangeState(EnemyState.Patrol);
            return;
        }

        float distance = Vector3.Distance(TF.position, target.position);

        // Attack
        if (distance <= AttackRange)
        {
            ChangeState(EnemyState.Attack);
            return;
        }

        // quá xa ? v? patrol
        if (distance >= stopChaseDistance)
        {
            ChangeState(EnemyState.Patrol);
        }
    }


    // STATE CHANGE
    public void ChangeState(EnemyState newState)
    {
        // stop attack coroutine when changing state
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }

        state = newState;

        switch (newState)
        {
            case EnemyState.Idle:
                if (agent != null) agent.ResetPath();
                ChangeAnim("idle");
                StartCoroutine(IdleTimer());
                break;

            case EnemyState.Patrol:
                SetRandomDestination();
                ChangeAnim("run");
                break;

            case EnemyState.Chase:
                ChangeAnim("run");
                break;

            case EnemyState.Attack:
                if (agent != null) agent.ResetPath();
                ChangeAnim("attack");
                canAttack = true;
                attackCoroutine = StartCoroutine(AttackRoutine());
                break;

            case EnemyState.Dead:
                if (agent != null) agent.enabled = false;
                ChangeAnim("dead");
                StartCoroutine(DespawnDelay());
                break;
        }
    }

    // STATE UPDATES
    private void IdleUpdate()
    {
        // idle behavior - maybe look around
    }

    private IEnumerator IdleTimer()
    {
        yield return new WaitForSeconds(idleTime);

        if (state == EnemyState.Idle)
            ChangeState(EnemyState.Patrol);
    }

    private void PatrolUpdate()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    private void ChaseUpdate()
    {
        if (agent == null || !agent.isOnNavMesh) return;
        if (player == null) return;

        agent.SetDestination(player.position);
    }

    private void AttackUpdate()
    {
        if (player != null)
        {
            Vector3 lookDir = (player.position - TF.position).normalized;
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.001f)
                TF.forward = lookDir;
        }
    }

    private IEnumerator AttackRoutine()
    {
        while (state == EnemyState.Attack && target != null)
        {
            float dist = Vector3.Distance(TF.position, target.position);
            if (dist <= AttackRange + 0.3f)
            {
                var dmg = target.GetComponent<IDamageAble>();
                dmg?.TakeDamage(Damage);
            }

            yield return new WaitForSeconds(attackCooldown);

            if (Vector3.Distance(TF.position, target.position) > AttackRange)
            {
                ChangeState(EnemyState.Chase);
                yield break;
            }
        }

        while (state == EnemyState.Attack && player != null)
        {
            if (!canAttack)
            {
                yield return null;
                continue;
            }

            // start attack
            canAttack = false;

            // face player
            Vector3 dir = (player.position - TF.position);
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
                TF.forward = dir.normalized;

            // play attack animation (already set when state changed) - optionally call ChangeAnim("attack");
            // deal damage to player if in range
            float dist = Vector3.Distance(TF.position, player.position);
            if (dist <= AttackRange + 0.3f)
            {
                var playerChar = player.GetComponent<Character>();
                if (playerChar != null)
                {
                    playerChar.TakeDamage(Damage);
                }
                else
                {
                    var dmgAble = player.GetComponent<IDamageAble>();
                    dmgAble?.TakeDamage(Damage);
                }
            }

            // wait cooldown
            yield return new WaitForSeconds(attackCooldown);
            canAttack = true;

            // if player moved out of attack range -> chase
            if (Vector3.Distance(TF.position, player.position) > AttackRange)
            {
                ChangeState(EnemyState.Chase);
                yield break;
            }
        }
    }

    // NAVMESH LOGIC
    private void SetRandomDestination()
    {
        Vector3 random = Random.insideUnitSphere * patrolRadius + startPos;
        random.y = startPos.y; // keep on same y plane

        if (NavMesh.SamplePosition(random, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            if (agent != null && agent.isOnNavMesh)
                agent.SetDestination(hit.position);
            else
                ChangeState(EnemyState.Idle);
        }
        else
        {
            ChangeState(EnemyState.Idle);
        }
    }

    // CHARACTER OVERRIDE
    private IEnumerator DespawnDelay()
    {
        yield return new WaitForSeconds(1.2f);
        OnDespawn();
    }
}
