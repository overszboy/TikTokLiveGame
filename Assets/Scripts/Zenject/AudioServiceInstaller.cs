using UnityEngine;
using Zenject;

public class AudioServiceInstaller : MonoInstaller
{[SerializeField] private AudioService audioService;
    public override void InstallBindings()
    {
        Container.Bind<AudioService>().FromInstance(audioService).AsSingle().NonLazy();
    }
}