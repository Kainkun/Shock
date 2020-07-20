using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : State
{
    readonly BasicEnemy basicEnemy;
    float wanderTime;
    float startingMoveSpeed;
    float timeSinceDestination;
    float timeToWaitAfterDestination;

    public Wander(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {
        wanderTime += Time.deltaTime;

        Vector3 localVelocity = basicEnemy.transform.InverseTransformDirection(basicEnemy.navMeshAgent.velocity);
        basicEnemy.animator.SetFloat("ForwardSpeed", localVelocity.z / basicEnemy.chaseSpeed);

        if (Vector3.Distance(basicEnemy.transform.position, basicEnemy.navMeshAgent.destination) < 1)
        {
            timeSinceDestination += Time.deltaTime;
            if(timeSinceDestination >= timeToWaitAfterDestination)
                GoToRandomPlaceWithinRadius(15);
        }

    }
    public void OnEnter()
    {
        basicEnemy.navMeshAgent.enabled = true;
        wanderTime = 0;
        GoToRandomPlaceWithinRadius(15);
    }
    public void OnExit()
    {
        basicEnemy.navMeshAgent.SetDestination(basicEnemy.transform.position);
        basicEnemy.navMeshAgent.enabled = false;
    }

    void GoToRandomPlaceWithinRadius(float radius)
    {
        timeSinceDestination = 0;
        timeToWaitAfterDestination = Random.Range(2, 10);
        basicEnemy.navMeshAgent.SetDestination(GetRandomPositionOnNavmesh(basicEnemy.transform.position, radius));
    }

    Vector3 GetRandomPositionOnNavmesh(Vector3 pos, float radius)
    {
        Vector3 randUnitCircle = Random.insideUnitCircle;
        (randUnitCircle.y, randUnitCircle.z) = (randUnitCircle.z, randUnitCircle.y);
        Vector3 attempt = pos + randUnitCircle * radius;
        NavMeshHit hit;
        if( NavMesh.SamplePosition(attempt, out hit, 5, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return GetRandomPositionOnNavmesh(pos, radius);   
    }

}
