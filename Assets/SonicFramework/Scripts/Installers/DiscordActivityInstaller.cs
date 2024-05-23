using SonicFramework.DiscordRPC;
using UnityEngine;
using Zenject;

namespace SonicFramework.Installers
{
    public class DiscordActivityInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var instance = new RasterDiscordActivity();
            Container.BindInterfacesAndSelfTo<RasterDiscordActivity>().FromInstance(instance).AsSingle().NonLazy();
            Container.QueueForInject(instance);
        }
    }
}