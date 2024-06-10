using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class NodeDirectionMap
{
    public Node Node;
    public Vector3 Direction;
}
public class Platform : MonoBehaviour
{
    [SerializeField] private List<NodeDirectionMap> nodeDirectionMaps;
    private Graph graph;

    [Inject]
    public void Construct(Graph graph)
    {
        this.graph = graph;
    }
    private void Awake()
    {
        RotateDirections();
        MarkIllegalNodes();
    }
    private void MarkIllegalNodes()
    {
        foreach (NodeDirectionMap map in nodeDirectionMaps)
        {
            Vector3 target = transform.position + map.Direction;

            Platform p = graph.FindPlatformAt(target);

            if (p != null)
            {
                map.Node.IsIllegal = true;
            }
        }
    }
    private void RotateDirections()
    {
        Quaternion rotation = Quaternion.Euler(transform.rotation.eulerAngles);

        foreach (NodeDirectionMap map in nodeDirectionMaps)
        {
            map.Direction = rotation * map.Direction;
        }
    }
}
