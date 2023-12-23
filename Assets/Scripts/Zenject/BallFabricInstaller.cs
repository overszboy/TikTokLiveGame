using UnityEngine;
using Zenject;

public class BallFabricInstaller : MonoInstaller
{ [SerializeField] private BallFabric ballFabric;
    public override void InstallBindings()
    {
        Container.Bind<BallFabric>().FromInstance(ballFabric).AsSingle().NonLazy();
    }
}