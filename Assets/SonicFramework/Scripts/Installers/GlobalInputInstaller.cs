using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace SonicFramework.Installers
{
    public class GlobalInputInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInputService playerInputService;
        [SerializeField] private MenuInputService menuInputService;
        [SerializeField] private PhotoInputService photoInputService;

        public override void InstallBindings()
        {
            Container.Bind<PlayerInputService>().FromInstance(playerInputService).AsSingle().NonLazy();
            Container.Bind<MenuInputService>().FromInstance(menuInputService).AsSingle().NonLazy();
            Container.Bind<PhotoInputService>().FromInstance(photoInputService).AsSingle().NonLazy();
        }
    }
}