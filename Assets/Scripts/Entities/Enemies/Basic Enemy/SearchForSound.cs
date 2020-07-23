using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForSound : State
{
    readonly BasicEnemy basicEnemy;
    public float searchTime;
    float startingMoveSpeed;
    float timeSinceDestination;
    float timeToWaitAfterDestination;
    float currentRadius;
    Vector3 mainSearchPosition;

    public SearchForSound(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {
        if(basicEnemy.Hearing())
        {
            OnEnter();
        }

        searchTime += Time.deltaTime;
        basicEnemy.navMeshAgent.speed = Manager.Remap(basicEnemy.maxSearchTime - 5, basicEnemy.maxSearchTime, basicEnemy.alertSpeed, startingMoveSpeed, searchTime);

        Vector3 localVelocity = basicEnemy.transform.InverseTransformDirection(basicEnemy.navMeshAgent.velocity);
        basicEnemy.animator.SetFloat("ForwardSpeed", localVelocity.z / basicEnemy.chaseSpeed);

        if (Vector3.Distance(basicEnemy.transform.position, basicEnemy.navMeshAgent.destination) < 1)
        {
            timeSinceDestination += Time.deltaTime;
            if(timeSinceDestination >= timeToWaitAfterDestination)
            {
                GoToRandomPlaceWithinRadius(currentRadius, mainSearchPosition);
                currentRadius += 0.2f;
            }
        }

    }
    public void OnEnter()
    {
        basicEnemy.navMeshAgent.enabled = true;
        startingMoveSpeed = basicEnemy.navMeshAgent.speed;
        searchTime = 0;
        currentRadius = 2;
        mainSearchPosition = basicEnemy.lastHeardSoundPosition;
        GoToRandomPlaceWithinRadius(2, mainSearchPosition);
    }
    public void OnExit()
    {
        basicEnemy.navMeshAgent.speed = startingMoveSpeed;
        basicEnemy.navMeshAgent.SetDestination(basicEnemy.transform.position);
        basicEnemy.navMeshAgent.enabled = false;
    }

    void GoToRandomPlaceWithinRadius(float radius)
    {
        GoToRandomPlaceWithinRadius(radius, basicEnemy.transform.position);
    }
    void GoToRandomPlaceWithinRadius(float radius, Vector3 position)
    {
        timeSinceDestination = 0;
        timeToWaitAfterDestination = Random.Range(0, 2);
        basicEnemy.navMeshAgent.SetDestination(GetRandomPositionOnNavmesh(position, radius));
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
