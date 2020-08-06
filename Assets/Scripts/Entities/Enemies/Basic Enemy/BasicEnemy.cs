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
    public float alertSpeed = 1.5f;
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
    public float hearingSensitivity = 0.5f;
    Transform eyesPosition;
    BoxCollider wanderArea;
    [HideInInspector]
    public Vector3 wanderAreaBoundsMin;
    [HideInInspector]
    public Vector3 wanderAreaBoundsMax;
    public float wanderAcceleration = 5;
    public float maxSearchTime = 10;
    public float casualSearchTime = 5;
    [HideInInspector]
    public Vector3 lastHeardSoundPosition;
    public float forwardAnimationLerpSpeed = 1;
    public float timeSinceHeard = 0;
    public float reactionTime = 0.5f;

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
        SetBoxBounds(wanderArea, out wanderAreaBoundsMin, out wanderAreaBoundsMax);

        stateMachine = new StateMachine();

        //STATES
        var idle = new Idle(this);
        var chasePlayer = new ChasePlayer(this);
        var dead = new Dead(this, playerMovementCollider);
        var attack = new Attack(this);
        var wander = new Wander(this);
        var wanderInArea = new WanderInArea(this);
        var searchForPlayer = new SearchForPlayer(this);
        var searchForSound = new SearchForSound(this);
        var lookForSound = new LookForSound(this);

        //TRANSITIONS
        AT(idle, chasePlayer, SeePlayer());
        AT(wander, chasePlayer, SeePlayer());
        AT(wanderInArea, chasePlayer, SeePlayer());
        AT(searchForPlayer, chasePlayer, SeePlayer());
        AT(searchForSound, chasePlayer, SeePlayer());

        AT(idle, lookForSound, HearsPlayer());
        AT(wander, lookForSound, HearsPlayer());
        AT(wanderInArea, lookForSound, HearsPlayer());
        AT(lookForSound, searchForSound, TimeToGoToSound());

        AT(chasePlayer, attack, InAttackRange());
        AT(attack, wanderInArea, AttackDone());
        AT(chasePlayer, searchForPlayer, CantFindPlayer());
        AT(searchForPlayer, wanderInArea, SearchOver());
        AT(chasePlayer, wanderInArea, LoseAgro());



        //ANY TRANSITIONS
        stateMachine.AddAnyTransition(dead, Dead());
        //stateMachine.AddAnyTransition(idle, PlayerMissing());

        //BEGINNING STATE
        stateMachine.SetState(wanderInArea);

        void AT(State to, State from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
        Func<bool> Dead() => () => currentHealth <= 0;
        Func<bool> PlayerMissing() => () => Player.instance == null;
        Func<bool> PlayerIsClose() => () => Vector3.Distance(transform.position, Player.instance.transform.position) < 7;
        Func<bool> LoseAgro() => () => Vector3.Distance(transform.position, Player.instance.transform.position) > 50;
        Func<bool> AttackDone() => () => attack.time >= attackTime;
        Func<bool> InAttackRange() => () => Vector3.Distance(transform.position, Player.instance.transform.position) < attackAttemptRange;
        Func<bool> SeePlayer() => () => RayHitsPlayer() && Vector3.Angle(eyesPosition.forward, Player.instance.mainCamera.transform.position - eyesPosition.position) < sightFOV;
        Func<bool> CantFindPlayer() => () => !RayHitsPlayer();
        Func<bool> SearchOver() => () =>  searchForPlayer.searchTime >= maxSearchTime;
        Func<bool> HearsPlayer() => () => Hearing();
        Func<bool> TimeToGoToSound() => () =>
        {
            return timeSinceHeard >= reactionTime;
        };

    }

    void Update() => stateMachine.Tick();

    public void SetMovementAnimation()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(navMeshAgent.velocity);

        animator.SetFloat("ForwardSpeed", Mathf.Lerp(animator.GetFloat("ForwardSpeed"), localVelocity.z / chaseSpeed, Time.deltaTime * forwardAnimationLerpSpeed));
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
    public bool Hearing()
    {
        if(Player.instance.GetLoudestSoundDistance() > Manager.CalculatePathLength(navMeshAgent, Player.instance.transform.position))
        {
            lastHeardSoundPosition = Player.instance.transform.position;
            return true;
        }

        return false;
    }

    void SetBoxBounds(BoxCollider col, out Vector3 boundsMin, out Vector3 boundsMax)
    {
        wanderArea.enabled = true;
        boundsMin = wanderArea.bounds.min;
        boundsMax = wanderArea.bounds.max;
        wanderArea.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        if (eyesPosition != null)
            Gizmos.matrix = Matrix4x4.TRS(eyesPosition.position, eyesPosition.rotation, Vector3.one);
        else
            Gizmos.matrix = Matrix4x4.TRS(transform.position + Vector3.up * 1.5f, transform.rotation, Vector3.one);

        Gizmos.DrawFrustum(Vector3.zero, sightFOV, sightDistance, 0.1f, 1);
    }

}
