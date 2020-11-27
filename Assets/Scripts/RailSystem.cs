using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RailSystem : MonoBehaviour
{
    [System.Serializable]
    public class NodeConnection
    {
        public Transform nodeA;
        public Transform nodeB;
    }

    public class Node
    {
        public Node(Transform transform) => this.transform = transform;

        public Transform transform;

        public SortedList<float, Node> connections = new SortedList<float, Node>(); //key is distance

        public float shortestDistanceFromStart;
        public Node previousNode;
    }

    [SerializeField]
    NodeConnection[] nodeConnections;
    public Dictionary<Transform, Node> nodes = new Dictionary<Transform, Node>();
    public Transform startingNode;

    void Start()
    {
        InitNodes();
    }

    void InitNodes()
    {
        foreach (NodeConnection nodeConnection in nodeConnections)
        {
            if (!nodes.ContainsKey(nodeConnection.nodeA))
                nodes.Add(nodeConnection.nodeA, new Node(nodeConnection.nodeA));

            if (!nodes.ContainsKey(nodeConnection.nodeB))
                nodes.Add(nodeConnection.nodeB, new Node(nodeConnection.nodeB));

            Node nodeA = nodes[nodeConnection.nodeA];
            Node nodeB = nodes[nodeConnection.nodeB];
            float distance = Vector3.Distance(nodeA.transform.position, nodeB.transform.position);
            nodeA.connections.Add(distance, nodeB);
            nodeB.connections.Add(distance, nodeA);
        }
    }

    public Queue<Transform> PathFindDijkstra(Transform start, Transform end)
    {
        ClearPathfindingData();

        Queue<Transform> queue = new Queue<Transform>();

        var nodeToVisit = nodes[start];
        VisitNode(ref nodeToVisit);
        nodes[start] = nodeToVisit;

        //visit starting node
        //while shortestDistanceNode has nodes
        //vist shortest node

        return queue;
    }

    void VisitNode(ref Node node)
    {
        for (int i = 0; i < node.connections.Count; i++)
        {
            float distanceFromCurrentNode = node.connections.Keys[i];
        }

    }

    void ClearPathfindingData()
    {
        foreach (var node in nodes)
        {
            node.Value.shortestDistanceFromStart = Mathf.Infinity;
            node.Value.previousNode = null;
        }
    }

    private void OnDrawGizmos()
    {
        foreach (NodeConnection nodeConnection in nodeConnections)
        {
            Gizmos.DrawLine(nodeConnection.nodeA.position, nodeConnection.nodeB.position);
        }
    }
}
