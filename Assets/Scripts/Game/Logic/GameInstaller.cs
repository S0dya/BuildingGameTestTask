using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;

    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle();

    }
}