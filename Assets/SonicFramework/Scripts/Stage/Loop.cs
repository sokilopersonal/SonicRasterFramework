using UnityEngine;

namespace SonicFramework
{
    public class Loop : MonoBehaviour
    {
        // private Player player;
        //
        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.transform.parent.CompareTag("Player"))
        //     {
        //         other.transform.parent.TryGetComponent(out Player playerResult);
        //         player = playerResult;
        //         player.Physics.inLoop = true;
        //     }
        // }
        //
        // private void OnTriggerExit(Collider other)
        // {
        //     if (other.transform.parent.CompareTag("Player"))
        //     {
        //         if (player == null)
        //         {
        //             other.transform.parent.TryGetComponent(out Player playerResult);
        //             player = playerResult;
        //         }
        //         
        //         player.Physics.inLoop = false;
        //     }
        // }
    }
}