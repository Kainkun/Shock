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

        //STATES
        var idle = new Idle(this);
        var chasePlayer = new ChasePlayer(this);
        var dead = new Dead(this);

        //TRANSITIONS
        AT(idle, chasePlayer, PlayerIsClose());
        AT(chasePlayer, idle, LoseAgro());

        //ANY TRANSITIONS
        stateMachine.AddAnyTransition(dead, Dead());
        stateMachine.AddAnyTransition(idle, PlayerMissing());

        //BEGINNING STATE
        stateMachine.SetState(idle);

        void AT(State to, State from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
        Func<bool> Dead() => () => currentHealth <= 0;
        Func<bool> PlayerMissing() => () => Player.instance == null;
        Func<bool> PlayerIsClose() => () => Vector3.Distance(transform.position, Player.instance.transform.position) < 4;
        Func<bool> LoseAgro() => () => Vector3.Distance(transform.position, Player.instance.transform.position) > 10;
    }

    void Update() => stateMachine.Tick();


}
