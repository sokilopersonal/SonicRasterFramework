using UnityEngine;
using Zenject;

namespace SonicFramework.Installers
{
    public class PlayerHUDScope : MonoInstaller
    {
        [SerializeField] private PlayerHUD playerHUDPrefab;

        public override void InstallBindings()
        {
            var instance = Container.InstantiatePrefabForComponent<PlayerHUD>(playerHUDPrefab);
            Container.Bind<PlayerHUD>().FromInstance(instance).AsSingle();
            Container.QueueForInject(instance);
        }
    }
}