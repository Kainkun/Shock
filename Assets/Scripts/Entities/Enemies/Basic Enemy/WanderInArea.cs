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
    float timeSinceLooking;
    float randomLookTime;

    public WanderInArea(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {

        #region "Movement"
        wanderTime += Time.deltaTime;
        Vector3 localVelocity = basicEnemy.transform.InverseTransformDirection(basicEnemy.navMeshAgent.velocity);
        basicEnemy.animator.SetFloat("ForwardSpeed", localVelocity.z / basicEnemy.chaseSpeed);

        if (Vector3.Distance(basicEnemy.transform.position, basicEnemy.navMeshAgent.destination) < 1)
        {
            timeSinceDestination += Time.deltaTime;
            
            if(timeSinceDestination >= timeToWaitAfterDestination)
                GoToRandomPlaceWithinArea();
        }
        #endregion

        #region "Looking"
        timeSinceLooking += Time.deltaTime;
        if(timeSinceLooking > randomLookTime)
        {
            if(Random.Range(0,2) == 0)
                basicEnemy.animator.SetTrigger("LookLeft");
            else
                basicEnemy.animator.SetTrigger("LookRight");
            
            timeSinceLooking = 0;
            randomLookTime = Random.Range(4f, 8f);
        }
        #endregion

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
        timeToWaitAfterDestination = Random.Range(4, 6);
        basicEnemy.navMeshAgent.SetDestination(GetRandomValidPositionInArea());
    }

    Vector3 GetRandomValidPositionInArea()
    {
        Vector3 randInArea = RandomWithinBox(basicEnemy.wanderAreaBoundsMin, basicEnemy.wanderAreaBoundsMax);
        randInArea.y = basicEnemy.transform.position.y;

        NavMeshHit hit;
        if( NavMesh.SamplePosition(randInArea, out hit, 2, NavMesh.AllAreas))
        {
            return randInArea;
        }

        return GetRandomValidPositionInArea();
    }


    Vector3 RandomWithinBox(Vector3 boundsMin, Vector3 boundsMax)
    {
        Vector3 v = new Vector3();
        v.x = Random.Range(boundsMin.x, boundsMax.x);
        v.y = Random.Range(boundsMin.y, boundsMax.y);
        v.z = Random.Range(boundsMin.z, boundsMax.z);
        return v;
    }

}
