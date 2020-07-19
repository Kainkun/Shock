using System;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{

    StateMachine stateMachine;
    [HideInInspector]
    public NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Animator animator;
    public float chaseSpeed = 3;
    public float chaseAcceleration = 0.1f;

    private void Awake()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        stateMachine = new StateMachine();

        var idle = new Idle(this);
        var chasePlayer = new ChasePlayer(this);

        AT(idle, chasePlayer, PlayerIsClose());
        AT(chasePlayer, idle, LoseAgro());

        stateMachine.AddAnyTransition(idle, PlayerMissing());

        stateMachine.SetState(idle);

        void AT(State to, State from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
        Func<bool> PlayerMissing() => () => Player.instance == null;
        Func<bool> PlayerIsClose() => () => Vector3.Distance(transform.position, Player.instance.transform.position) < 4;
        Func<bool> LoseAgro() => () => Vector3.Distance(transform.position, Player.instance.transform.position) > 10;
    }

    void Update() => stateMachine.Tick();


}
