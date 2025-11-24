using UnityEngine;

public class Player : Character
{
    [HideInInspector] public StateMachine stateMachine;

    [Header("Attack Settings")]
    public float AttackRange = 1.6f;
    public float attackCooldown = 1f;
    public VisionRange visionRange;

    [HideInInspector] public Transform currentTarget;

    public IState idleState;
    public IState moveState;
    public IState attackState;
    public IState deadState;

    [SerializeField] private float speed = 5f;
    public float Speed => speed;

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new StateMachine();

        idleState = new StatePlayerIdle(this);
        moveState = new StatePlayerMove(this);
        attackState = new StatePlayerAttack(this);
        deadState = new StatePlayerDie(this);
        visionRange.OnTargetEnter += HandleTargetEnter;
        visionRange.OnTargetExit += HandleTargetExit;
    }
    void HandleTargetEnter(Transform target)
    {
        currentTarget = target;
    }

    void HandleTargetExit(Transform target)
    {
        if (currentTarget == target)
            currentTarget = null;
    }

    void Start()
    {
        stateMachine.ChangeState(idleState);
    }

    void Update()
    {
        stateMachine.Update();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);

        if (Health <= 0)
        {
            stateMachine.ChangeState(deadState);
        }
    }

    protected override void OnDead()
    {
        stateMachine.ChangeState(deadState);
    }
}
