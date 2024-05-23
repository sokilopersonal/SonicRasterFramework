using UnityEngine;

namespace SonicFramework
{
    public class Spawnable : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
    }
}