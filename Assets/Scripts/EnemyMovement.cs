using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3;
    public float rotationSpeed = 3;
    public float visionDistance = 30;
    protected NavMeshAgent navMeshAgent;
    public Transform attention;
    protected Player player;
    public float sightFOV = 45;
    public Transform eyes;

    protected virtual void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        player = Player.instance;
    }

    protected virtual void Update()
    {
        Variables();
        // RotateToAttention();
        // ForwardToAttention();
        // navMeshAgent.SetDestination(attention.position);
        if(CanSeePlayer())
            navMeshAgent.SetDestination(Player.instance.transform.position);
    }

    protected Vector3 yPlaneForward;
    protected Vector3 yPlaneAttentionDirection;
    protected void Variables()
    {
        yPlaneForward = transform.forward;
        yPlaneForward.y = 0;
        yPlaneForward.Normalize();
        yPlaneAttentionDirection = attention.position - transform.position;
        yPlaneAttentionDirection.y = 0;
        yPlaneAttentionDirection.Normalize();
    }

    protected void ForwardToAttention()
    {
        float speedStrength;
        if(navMeshAgent.path.corners.Length < 3) //if desination is straight path, use speedStrength for better movement
        {
            speedStrength = Vector3.Dot(yPlaneForward, yPlaneAttentionDirection);
            speedStrength = Mathf.Max(speedStrength, 0);
            speedStrength = Manager.Remap(0.9f, 1, 0, 1, speedStrength);
        }
        else
        {
            speedStrength = 1;
        }

        navMeshAgent.speed = speed * speedStrength;
        
        //Debug.DrawRay(transform.position, yPlaneForward);
        //Debug.DrawRay(transform.position, yPlaneAttentionDirection, Color.red);
    }

    protected void RotateToAttention()
    {
        Quaternion lookRotation = Quaternion.LookRotation(yPlaneAttentionDirection, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    protected bool CanSeePlayer()
    {
        RaycastHit hit;

        if(Physics.Raycast(eyes.position, player.mainCamera.transform.position - eyes.position, out hit, visionDistance) && hit.transform.tag == "Player")
            if(Vector3.Angle(eyes.forward, Player.instance.mainCamera.transform.position - eyes.position) < sightFOV)
                return true;
            else
                return false;
        else
            return false;
    }


}
