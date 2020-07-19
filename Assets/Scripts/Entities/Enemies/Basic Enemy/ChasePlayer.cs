using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : State
{

    readonly BasicEnemy basicEnemy;
    NavMeshAgent navMeshAgent;
    float startingMoveSpeed;
    float chaseSpeed;

    public ChasePlayer(BasicEnemy basicEnemy, NavMeshAgent navMeshAgent, float chaseSpeed)
    {
        this.basicEnemy = basicEnemy;
        this.navMeshAgent = navMeshAgent;
        this.chaseSpeed = chaseSpeed;
    }

    public void Tick()
    {
        Debug.Log("Chase");
        navMeshAgent.SetDestination(Player.instance.transform.position);

    }
    public void OnEnter()
    {
        startingMoveSpeed = navMeshAgent.speed;
        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.enabled = true;
    }
    public void OnExit()
    {
        navMeshAgent.enabled = false;
        navMeshAgent.speed = startingMoveSpeed;
        navMeshAgent.SetDestination(basicEnemy.transform.position);
    }
}
