using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[Serializable]
public class DirectionMap
{
    public Vector3 Direction;
    public GameObject DirectionArrow;
}
public class DiractionControllerUI : MonoBehaviour
{
    [SerializeField] private List<DirectionMap> directionMaps;
    private List<Vector3> activeDirections;
    private List<Vector3> changedDirections;
    private SignalBus signalBus;
    private Cube cube;

    [Inject]
    public void Construct(SignalBus signalBus, Cube cube)
    {
        this.signalBus = signalBus;
        this.cube = cube;
    }
    private void Awake()
    {
        signalBus.Subscribe<DestinationReachedSignal>(CloseAll);
        signalBus.Subscribe<DestinationReachedSignal>(UnsubscribeNodeChanged);

        activeDirections = new List<Vector3>();
        changedDirections = new List<Vector3>();
        cube.OnNodeChanged += OnDirectionChange;
    }
    public void OnDirectionChange(Node node)
    {
        foreach (Edge edge in node.Edges)
        {
            changedDirections.Add(edge.Direction);
        }

        if (!IsSameDirections(changedDirections))
        {
            ChangeDirections(changedDirections);
        }

        changedDirections.Clear();
    }

    private bool IsSameDirections(List<Vector3> directions)
    {
        if (directions.Count != activeDirections.Count) return false;

        int count = 0;

        foreach (Vector3 direction in directions)
        {
            foreach (Vector3 current in activeDirections)
            {
                if (direction == current) count++;
            }
        }

        if (count == activeDirections.Count) return true;

        return false;
    }
    private void ChangeDirections(List<Vector3> directions)
    {
        CloseAll();
        foreach (Vector3 direction in directions)
        {
            GetDirectionMap(direction).DirectionArrow.SetActive(true);
            activeDirections.Add(direction);
        }
    }

    private DirectionMap GetDirectionMap(Vector3 direction)
    {
        foreach (DirectionMap directionMap in directionMaps)
        {
            if (directionMap.Direction == direction)
            {
                return directionMap;
            }
        }
        return null;
    }
    private void CloseAll()
    {
        foreach (Vector3 activeDirection in activeDirections)
        {
            GetDirectionMap(activeDirection).DirectionArrow.SetActive(false);
        }
        activeDirections.Clear();
    }
    private void UnsubscribeNodeChanged()
    {
        cube.OnNodeChanged -= OnDirectionChange;
    }
}
