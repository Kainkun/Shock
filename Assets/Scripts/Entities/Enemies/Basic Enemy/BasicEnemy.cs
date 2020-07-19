using System;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : MonoBehaviour
{

    StateMachine stateMachine;
    NavMeshAgent navMeshAgent;
    public float chaseSpeed = 2;

    private void Awake()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }

        navMeshAgent = GetComponent<NavMeshAgent>();

        stateMachine = new StateMachine();

        var idle = new Idle(this);
        var chasePlayer = new ChasePlayer(this, navMeshAgent, chaseSpeed);

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
