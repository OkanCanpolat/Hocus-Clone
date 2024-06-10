using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class Node : MonoBehaviour
{
    public UnityEvent OnEnterEvent;
    [SerializeField] private float gizmoRadius = 0.1f;
    [SerializeField] private Color gizmoColor = Color.blue;
    [SerializeField] private Color gizmoColorWithEvent = Color.yellow;
    [SerializeField] private List<Edge> edges;
    [SerializeField] private List<Node> excludedNodes;
    private Graph graph;
    private bool isIllegal;

    public List<Edge> Edges => edges;
    public bool IsIllegal { get => isIllegal; set => isIllegal = value; }

    public Vector3[] neighborDirections;

    [Inject]
    public void Construct(Graph graph)
    {
        this.graph = graph;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = OnEnterEvent.GetPersistentEventCount() > 0 ? gizmoColorWithEvent : gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        Gizmos.color = Color.red;

        foreach (Edge e in edges)
        {
            if (e.Node != null)
            {
                Gizmos.DrawLine(transform.position, e.Node.transform.position);
            }
        }
    }
    public void FindNeighbors()
    {
        if (isIllegal) return;

        foreach (Vector3 direction in neighborDirections)
        {
            Node newNode = graph?.FindNodeAt(transform.position + direction);

            if (newNode != null && !newNode.isIllegal && !excludedNodes.Contains(newNode))
            {
                Edge newEdge = new Edge(newNode, direction);
                edges.Add(newEdge);
            }
        }
    }
    public Edge GetEdge(Vector3 direction)
    {
        foreach (Edge e in edges)
        {
            if (e.Direction == direction) return e;
        }
        return null;
    }
    public Node GetJointEdge()
    {
        foreach (Edge e in edges)
        {
            if (e.Joint) return e.Node;
        }
        return null;
    }
}
