using UnityEngine;
using Zenject;

namespace SonicFramework.Installers
{
    public class GameLifeTimeScope : MonoInstaller
    {
        [SerializeField] private Settings settings;
        [SerializeField] private SonicCameraConfig cameraConfig;
        [SerializeField] private PlayerMovementConfig playerConfig;

        public override void InstallBindings()
        {
            Container.Bind<Settings>().FromInstance(settings).AsSingle();
            Container.Bind<SonicCameraConfig>().FromInstance(cameraConfig).AsSingle();
            Container.Bind<PlayerMovementConfig>().FromInstance(playerConfig).AsSingle();
        }
    }
}