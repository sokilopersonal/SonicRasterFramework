using UnityEngine;
using Zenject;

namespace SonicFramework
{
    internal enum PlayerStartMode
    {
        Instant,
        Start,
        Start2
    }
    
    [SelectionBase]
    public class PlayerStart : MonoInstaller
    {
        [Header("Prefab")]
        [SerializeField] private PlayerBase playerPrefab;

        public override void InstallBindings()
        {
            var instance =
                Container.InstantiatePrefabForComponent<PlayerBase>(playerPrefab, transform.position, transform.rotation, null);

            Container.Bind<PlayerBase>().FromInstance(instance).AsSingle().NonLazy();
            
            Destroy(gameObject);
        }
    }
}