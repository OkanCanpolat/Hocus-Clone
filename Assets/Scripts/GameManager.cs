using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameManager : MonoBehaviour
{
    private SignalBus signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
    }

    private void Awake()
    {
        signalBus.Subscribe<LevelFinishedSignal>(LoadNextLevel);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        int maxSceneIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (nextSceneIndex > maxSceneIndex)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
    private void OnDestroy()
    {
        signalBus.Unsubscribe<LevelFinishedSignal>(LoadNextLevel);
    }
}
