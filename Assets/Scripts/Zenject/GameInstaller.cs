using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public AudioSource AudioSource;
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<LevelFinishedSignal>().OptionalSubscriber();
        Container.DeclareSignal<DestinationReachedSignal>().OptionalSubscriber();
        Container.Bind<AudioSource>().FromInstance(AudioSource).AsSingle();
        Container.Bind<ISoundSystem>().To<SoundSystem>().AsSingle();
        Container.Bind<Cube>().FromComponentInHierarchy().AsSingle();
        Container.Bind<Graph>().FromComponentInHierarchy().AsSingle();
    }
}

public class LevelFinishedSignal { }
public class DestinationReachedSignal { }