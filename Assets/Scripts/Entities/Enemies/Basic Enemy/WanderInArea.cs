using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderInArea : State
{
    readonly BasicEnemy basicEnemy;
    float wanderTime;
    float timeSinceDestination;
    float timeToWaitAfterDestination;
    float startingAcceleration;

    public WanderInArea(BasicEnemy basicEnemy)
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
                GoToRandomPlaceWithinArea();
        }

    }
    public void OnEnter()
    {
        basicEnemy.navMeshAgent.enabled = true;
        startingAcceleration = basicEnemy.navMeshAgent.acceleration;
        basicEnemy.navMeshAgent.acceleration = basicEnemy.wanderAcceleration;
        wanderTime = 0;

        GoToRandomPlaceWithinArea();
    }
    public void OnExit()
    {
        basicEnemy.navMeshAgent.SetDestination(basicEnemy.transform.position);
        basicEnemy.navMeshAgent.acceleration = startingAcceleration;
        basicEnemy.navMeshAgent.enabled = false;
    }




    void GoToRandomPlaceWithinArea()
    {
        timeSinceDestination = 0;
        timeToWaitAfterDestination = Random.Range(2, 4);
        basicEnemy.navMeshAgent.SetDestination(GetRandomValidPositionInArea());
    }

    Vector3 GetRandomValidPositionInArea()
    {
        Vector3 randInArea = RandomWithinBox(basicEnemy.wanderArea);
        randInArea.y = basicEnemy.transform.position.y;

        NavMeshHit hit;
        if( NavMesh.SamplePosition(randInArea, out hit, 2, NavMesh.AllAreas))
        {
            return randInArea;
        }

        return GetRandomValidPositionInArea();
    }


    Vector3 RandomWithinBox(BoxCollider box)
    {
        Vector3 v = new Vector3();
        v.x = Random.Range(box.bounds.min.x, box.bounds.max.x);
        v.y = Random.Range(box.bounds.min.y, box.bounds.max.y);
        v.z = Random.Range(box.bounds.min.z, box.bounds.max.z);
        return v;
    }

}
