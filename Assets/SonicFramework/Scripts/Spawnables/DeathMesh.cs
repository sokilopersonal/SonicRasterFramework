using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SonicFramework
{
    public class DeathMesh : PlayerContactable // for now it's only reseting player position
    {
        private Vector3 initPos;
        private Quaternion initRot;
        
        private void Awake()
        {
            initPos = player.transform.position;
            initRot = player.transform.rotation;
        }

        public override async void OnContact()
        {
            base.OnContact();

            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), DelayType.Realtime);
            
            player.fsm.rb.velocity = Vector3.zero;
            player.transform.position = initPos;
            player.transform.rotation = initRot;
        }
    }
}