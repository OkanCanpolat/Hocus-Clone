using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class Cube : MonoBehaviour
{
    public Node currentNode;
    private bool isMoving;
    private bool canMove;
    [SerializeField] private float rollSpeed = 3;
    [SerializeField] private AudioClip movementClip;
    private SignalBus signalBus;
    public Action<Node> OnNodeChanged;
    private ISoundSystem soundSystem;

    [Inject]
    public void Construct(SignalBus signalBus, ISoundSystem soundSystem)
    {
        this.signalBus = signalBus;
        this.soundSystem = soundSystem;
    }
    private void Awake()
    {
        signalBus.Subscribe<DestinationReachedSignal>(OnDestinationReach);
    }
    private void Start()
    {
        canMove = true;
        transform.position = currentNode.transform.position + currentNode.transform.up * 0.5f;
        ChangeCurrentNode(currentNode);
    }
    void Update()
    {
        if (isMoving || !canMove) return;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(Roll(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(Roll(Vector3.left));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(Roll(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(Roll(Vector3.back));
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(Roll(Vector3.up));
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(Roll(Vector3.down));
        }
    }
    public void ChangeLayer(string layer)
    {
        gameObject.layer = LayerMask.NameToLayer(layer);
    }
    private IEnumerator Roll(Vector3 direction)
    {
        Edge edgee = currentNode.GetEdge(direction);

        if (edgee == null) yield break;

        if (edgee.Joint)
        {
            currentNode = edgee.Node;
            edgee = currentNode.GetEdge(direction);
        }

        isMoving = true;

        while (edgee != null)
        {
            var anchor = GetAnchorPoint(direction);
            var axis = GetAxis(direction);
            soundSystem.Play(movementClip);

            for (int i = 0; i < (90 / rollSpeed); i++)
            {
                transform.RotateAround(anchor, axis, rollSpeed);
                yield return null;
            }

            ChangeCurrentNode(edgee.Node);
            edgee = currentNode.GetEdge(direction);

            if (currentNode.Edges.Count > 2) break;
        }

        isMoving = false;
    }
    private Vector3 GetAnchorPoint(Vector3 direction)
    {
        Vector3 currentNodeDown = -(currentNode.transform.up * 0.5f);
        Vector3 anchor = (transform.position + currentNodeDown) + direction * 0.5f;/*currentNode.transform.position + direction * 0.5f;*/
        return anchor;
    }
    private Vector3 GetAxis(Vector3 direction)
    {
        Vector3 up = currentNode.transform.up;
        Vector3 axis = Vector3.Cross(up, direction);
        return axis;
    }
    private void ChangeCurrentNode(Node node)
    {
        currentNode = node;
        transform.position = currentNode.transform.position + currentNode.transform.up * 0.5f;
        currentNode.OnEnterEvent?.Invoke();
        OnNodeChanged?.Invoke(currentNode);
        Node jointNode = currentNode.GetJointEdge();
        if (jointNode != null) jointNode.OnEnterEvent?.Invoke();
    }
    private void OnDestinationReach()
    {
        canMove = false;
    }
}
