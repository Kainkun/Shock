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
        public List<Node> connections = new List<Node>();
    }

    [SerializeField]
    NodeConnection[] nodeConnections;
    public Dictionary<Transform, Node> nodes = new Dictionary<Transform, Node>();
    public Transform startingNode;

    void Start()
    {
        InitNodes();
    }

    void Update()
    {

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
            nodeA.connections.Add(nodeB);
            nodeB.connections.Add(nodeA);
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
