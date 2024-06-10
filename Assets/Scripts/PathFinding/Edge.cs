using System;
using UnityEngine;

[Serializable]
public class Edge
{
    public Node Node;
    public Vector3 Direction;
    public bool Joint;

    public Edge(Node node, Vector3 direction)
    {
        Node = node;
        Direction = direction;
    }
}
