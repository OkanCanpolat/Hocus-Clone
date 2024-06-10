using System.Collections;
using UnityEngine;
using Zenject;

public class FinishPoint : MonoBehaviour
{
    [SerializeField] private Node node;
    [SerializeField] private float cubeClearTime = 1.5f;
    [SerializeField] private AudioClip finishClip;
    private Cube cube;
    private SignalBus signalBus;
    private ISoundSystem soundSystem;

    [Inject]
    public void Construct(SignalBus signalBus, ISoundSystem soundSystem, Cube cube)
    {
        this.signalBus = signalBus;
        this.soundSystem = soundSystem;
        this.cube = cube;
    }
    private void Awake()
    {
        transform.position = node.transform.position;
        transform.up = node.transform.up;
        node.OnEnterEvent.AddListener(OnFinish);
    }

    private void OnFinish()
    {
        signalBus.TryFire<DestinationReachedSignal>();
        cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        StartCoroutine(ClearCube());
    }

    private IEnumerator ClearCube()
    {
        float t = 0;
        Vector3 current = cube.transform.position;
        Vector3 target = node.transform.position - node.transform.up * 0.5f;

        yield return new WaitForSeconds(.15f);
        soundSystem.Play(finishClip);

        while (t < 1)
        {
            cube.transform.position = Vector3.Lerp(current, target, t);
            t += Time.deltaTime / cubeClearTime;
            yield return null;
        }

        signalBus.TryFire<LevelFinishedSignal>();
    }
}
