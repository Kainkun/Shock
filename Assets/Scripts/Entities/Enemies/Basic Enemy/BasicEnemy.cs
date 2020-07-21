using System;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : Entity
{

    StateMachine stateMachine;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Animator animator;
    public float chaseSpeed = 3;
    public float chaseAcceleration = 0.1f;
    public float attackMoveSpeed = 0.5f;
    public float attackTime = 2;
    public float attackAttemptRange = 0.7f;
    public float attackRange = 1;
    public float attackWidthArc = 30f;
    public float damage = 20;
    Collider playerMovementCollider;
    public float sightFOV = 45;
    public float sightDistance = 10;
    Transform eyesPosition;
    [HideInInspector]
    public BoxCollider wanderArea;
    public float wanderAcceleration = 5;

    private void Awake()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        playerMovementCollider = GetComponentInChildren<Collider>();
        eyesPosition = GetComponentInChildren<EyesLocation>().transform;
        wanderArea = GetComponentInChildren<BoxCollider>();
        wanderArea.transform.parent = null;

        stateMachine = new StateMachine();

        //STATES
        var idle = new Idle(this);
        var chasePlayer = new ChasePlayer(this);
        var dead = new Dead(this, playerMovementCollider);
        var attack = new Attack(this);
        var wander = new Wander(this);
        var wanderInArea = new WanderInArea(this);

        //TRANSITIONS
        //AT(idle, chasePlayer, SeePlayer());
        //AT(wander, chasePlayer, SeePlayer());
        //AT(wanderInArea, chasePlayer, SeePlayer());

        //AT(chasePlayer, attack, InAttackRange());
        //AT(attack, wanderInArea, AttackDone());
        //AT(chasePlayer, wanderInArea, LoseAgro());


        //ANY TRANSITIONS
        stateMachine.AddAnyTransition(dead, Dead());
        stateMachine.AddAnyTransition(idle, PlayerMissing());

        //BEGINNING STATE
        stateMachine.SetState(wanderInArea);

        void AT(State to, State from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
        Func<bool> Dead() => () => currentHealth <= 0;
        Func<bool> PlayerMissing() => () => Player.instance == null;
        Func<bool> PlayerIsClose() => () => Vector3.Distance(transform.position, Player.instance.transform.position) < 7;
        Func<bool> LoseAgro() => () => Vector3.Distance(transform.position, Player.instance.transform.position) > 10;
        Func<bool> AttackDone() => () => attack.time >= attackTime;
        Func<bool> InAttackRange() => () => Vector3.Distance(transform.position, Player.instance.transform.position) < attackAttemptRange;
        Func<bool> SeePlayer() => () => RayHitsPlayer() && Vector3.Angle(eyesPosition.forward, Player.instance.mainCamera.transform.position - eyesPosition.position) < sightFOV;
    }

    void Update() => stateMachine.Tick();

    public void SetMovementAnimation()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);
        animator.SetFloat("ForwardSpeed", localVelocity.z / chaseSpeed);
    }

    bool RayHitsPlayer()
    {
        /*Debug.DrawRay(eyesPosition.position, eyesPosition.forward, Color.white);
        Debug.DrawRay(eyesPosition.position, (Player.instance.mainCamera.transform.position - eyesPosition.position).normalized * sightDistance, Color.red);*/


        RaycastHit hit;
        if(Physics.Raycast(eyesPosition.position, Player.instance.mainCamera.transform.position - eyesPosition.position, out hit, sightDistance))
        {
            if(hit.transform.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 eyes;
        if (eyesPosition != null)
            eyes = eyesPosition.position;
        else
            eyes = transform.position + Vector3.up * 1.5f;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + Vector3.up * 1.5f, transform.rotation, Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, sightFOV, sightDistance, 0.1f, 1);
    }

}
