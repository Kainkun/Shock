using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    readonly BasicEnemy basicEnemy;
    public float time;
    NavMeshAgent navMeshAgent;
    float startingMoveSpeed;


    public Attack(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {
        time += Time.deltaTime;
        navMeshAgent.SetDestination(Player.instance.transform.position);
        navMeshAgent.speed = Mathf.Lerp(startingMoveSpeed, basicEnemy.attackMoveSpeed, time * 3);
        Vector3 playerDirection = Player.instance.transform.position - basicEnemy.transform.position;
        basicEnemy.transform.forward = Vector3.RotateTowards(basicEnemy.transform.forward, playerDirection, Mathf.Deg2Rad * navMeshAgent.angularSpeed * Time.deltaTime, 0);
        basicEnemy.SetMovementAnimation();

    }
    public void OnEnter()
    {
        time = 0;
        navMeshAgent = basicEnemy.navMeshAgent;
        navMeshAgent.enabled = true;

        startingMoveSpeed = navMeshAgent.speed;
        navMeshAgent.speed = basicEnemy.attackMoveSpeed;

        basicEnemy.animator.SetTrigger("Attack");


    }
    public void OnExit()
    {
        time = 0;
        navMeshAgent.speed = startingMoveSpeed;
        navMeshAgent.SetDestination(basicEnemy.transform.position);
        navMeshAgent.enabled = false;
    }

}
