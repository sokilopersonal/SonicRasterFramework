using UnityEngine;
using Zenject;

namespace SonicFramework.Installers
{
    public class StageInstaller : MonoInstaller
    {
        [SerializeField] private StageInfo _stageInfo;

        public override void InstallBindings()
        {
            Container.Bind<StageInfo>().FromInstance(_stageInfo).AsSingle().NonLazy();
            Container.Bind<Stage>().FromInstance(FindFirstObjectByType<Stage>()).AsSingle().NonLazy();
        }
    }
}