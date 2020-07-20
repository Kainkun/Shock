using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : State
{

    readonly BasicEnemy basicEnemy;
    NavMeshAgent navMeshAgent;
    float startingMoveSpeed;
    float chasingTime;

    public ChasePlayer(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {
        chasingTime += Time.deltaTime;
        navMeshAgent.SetDestination(Player.instance.transform.position);
        navMeshAgent.speed = Mathf.Lerp(startingMoveSpeed, basicEnemy.chaseSpeed, chasingTime * basicEnemy.chaseAcceleration);
        Vector3 localVelocity = basicEnemy.transform.InverseTransformDirection(navMeshAgent.velocity);
        basicEnemy.animator.SetFloat("ForwardSpeed", localVelocity.z/basicEnemy.chaseSpeed);

    }
    public void OnEnter()
    {
        chasingTime = 0;
        navMeshAgent = basicEnemy.navMeshAgent;
        navMeshAgent.enabled = true;
        startingMoveSpeed = navMeshAgent.speed;
    }
    public void OnExit()
    {
        //basicEnemy.animator.SetFloat("ForwardSpeed", 0);
        navMeshAgent.speed = startingMoveSpeed;
        navMeshAgent.SetDestination(basicEnemy.transform.position);
        navMeshAgent.enabled = false;
    }
}
