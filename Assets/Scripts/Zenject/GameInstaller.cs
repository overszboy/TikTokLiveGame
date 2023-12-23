using System.ComponentModel;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{ 
     [SerializeField] private Game game;
    public override void InstallBindings()
    {
        Container.Bind<Game>().FromInstance(game).AsSingle().NonLazy();
    }
}