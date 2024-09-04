using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Player player;

    [SerializeField] private InputManager inputManager;
    [SerializeField] private BuildingManager buildingManager;
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle();

        Container.Bind<InputManager>().FromInstance(inputManager).AsSingle();
        Container.Bind<BuildingManager>().FromInstance(buildingManager).AsSingle();

    }
}