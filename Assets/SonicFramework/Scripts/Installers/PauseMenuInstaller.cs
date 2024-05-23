using SonicFramework.UI;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace SonicFramework.Installers
{
    public class PauseMenuInstaller : MonoInstaller
    {
        [SerializeField] private Pause pause;

        public override void InstallBindings()
        {
            var instance = Container.InstantiatePrefabForComponent<Pause>(pause);
            Container.Bind<Pause>().FromInstance(instance).AsSingle();
        }
    }
}