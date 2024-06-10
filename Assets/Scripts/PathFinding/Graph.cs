using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private List<Node> allNodes = new List<Node>();
    private List<Platform> allPlatforms = new List<Platform>();

    private void Awake()
    {
        allNodes = FindObjectsOfType<Node>().ToList();
        allPlatforms = FindObjectsOfType<Platform>().ToList();
    }

    private void Start()
    {
        foreach (Node n in allNodes)
        {
            n.FindNeighbors();
        }
    }

    public Node FindNodeAt(Vector3 pos)
    {
        foreach (Node n in allNodes)
        {
            Vector3 diff = n.transform.position - pos;

            if (diff.sqrMagnitude < 0.01f)
            {
                return n;
            }
        }
        return null;
    }

    public Platform FindPlatformAt(Vector3 pos)
    {
        foreach (Platform p in allPlatforms)
        {
            Vector3 diff = p.transform.position - pos;

            if (diff.sqrMagnitude < 0.01f)
            {
                return p;
            }
        }
        return null;
    }
}
