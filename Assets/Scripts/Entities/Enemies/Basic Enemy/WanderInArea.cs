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
    Rect area;
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
                GoToRandomPlaceWithinArea(area);
        }

    }
    public void OnEnter()
    {
        basicEnemy.navMeshAgent.enabled = true;
        startingAcceleration = basicEnemy.navMeshAgent.acceleration;
        basicEnemy.navMeshAgent.acceleration = basicEnemy.wanderAcceleration;
        wanderTime = 0;

        area = BoxColliderToRect(basicEnemy.wanderArea);
        GoToRandomPlaceWithinArea(area);
    }
    public void OnExit()
    {
        Debug.Log(basicEnemy.name);
        basicEnemy.navMeshAgent.SetDestination(basicEnemy.transform.position);
        basicEnemy.navMeshAgent.acceleration = startingAcceleration;
        basicEnemy.navMeshAgent.enabled = false;
    }




    void GoToRandomPlaceWithinArea(Rect rect)
    {
        timeSinceDestination = 0;
        timeToWaitAfterDestination = Random.Range(2, 4);
        basicEnemy.navMeshAgent.SetDestination(GetRandomValidPositionInArea());
    }

    Vector3 GetRandomValidPositionInArea()
    {
        Vector3 randInArea = RandomWithinRect(area);
        NavMeshHit hit;
        if( NavMesh.SamplePosition(randInArea, out hit, 2, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return GetRandomValidPositionInArea();
    }

    Rect BoxColliderToRect(BoxCollider box)
    {
        Rect rect = new Rect();

        Vector2 rectCenter = new Vector2();
        rectCenter.x = box.transform.position.x + box.center.x;
        rectCenter.y = box.transform.position.z + box.center.z;
        rect.center = rectCenter;

        rect.width = box.size.x;
        rect.height = box.size.z;

        return rect;
    }

    Vector3 RandomWithinRect(Rect rect)
    {
        Vector3 v = new Vector3();
        v.x = rect.x;
        v.z = rect.y;
        v.x += Random.Range(-rect.width / 2, rect.width / 2);
        v.z += Random.Range(-rect.height / 2, rect.height / 2);
        v.y = basicEnemy.transform.position.y;
        return v;
    }

}
