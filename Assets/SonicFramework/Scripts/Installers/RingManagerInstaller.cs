using UnityEngine;
using Zenject;

namespace SonicFramework.Installers
{
    public class RingManagerInstaller : MonoInstaller
    {
        [SerializeField] private float rotationSpeed = 100f;
        
        public override void InstallBindings()
        {
            var manager = new RingManager(rotationSpeed);
            Container.BindInterfacesAndSelfTo<RingManager>().FromInstance(manager).AsSingle().NonLazy();
        }
    }
}