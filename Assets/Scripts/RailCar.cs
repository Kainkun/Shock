using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailCar : MonoBehaviour
{
    public float speed = 1;
    public RailSystem railSystem;

    [SerializeField]
    Transform[] startingPath;
    Queue<Transform> travelQueue = new Queue<Transform>();
    Transform currentTargetNode;

    void Start()
    {
        for (int i = 0; i < startingPath.Length; i++)
        {
            travelQueue.Enqueue(startingPath[i]);
        }
        transform.position = GoToNextNode().position;

    }
    protected virtual void Update()
    {
        if (currentTargetNode)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTargetNode.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, currentTargetNode.position) < 0.1f)
                GoToNextNode();
        }

    }

    public void GoToNode(Transform node)
    {
        //make queue
    }

    Transform GoToNextNode()
    {
        if (travelQueue.Count > 0)
        {
            var nextNode = travelQueue.Dequeue();
            currentTargetNode = nextNode;
            return nextNode;
        }
        else
        {
            currentTargetNode = null;
            return null;
        }
    }
}
